﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ObjectManager :Singleton<ObjectManager>
{
    /// <summary>
    /// 对象池节点
    /// </summary>
    public Transform RecyclePoolTrs;
    /// <summary>
    /// 场景节点
    /// </summary>
    public Transform SceneTrs;
    /// <summary>
    /// 对象池
    /// </summary>
    protected Dictionary<uint, List<ResourceObj>> m_ObjctPoolDic = new Dictionary<uint, List<ResourceObj>>();
    /// <summary>
    /// 暂存ResObj的Dic
    /// </summary>
    protected Dictionary<int, ResourceObj> m_ResourceObjDic = new Dictionary<int, ResourceObj>();
    /// <summary>
    /// ResourceObj的类对象池
    /// </summary>
    protected ClassObjectPool<ResourceObj> m_ResourceObjClassPool = null;
    /// <summary>
    /// 根据异步的guid储存ResourceObj,来判断是否正在异步加载
    /// </summary>
    protected Dictionary<long, ResourceObj> m_AsyncResObjs = new Dictionary<long, ResourceObj>();


    internal void ClearAllPoolObject()
    {
        m_ObjctPoolDic.Clear();
        m_ResourceObjDic.Clear();
        m_AsyncResObjs.Clear();
    }
    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="recycleTrs"></param>
    /// <param name="sceneTrs"></param>
    public void Init(Transform recycleTrs, Transform sceneTrs)
    {
        m_ResourceObjClassPool = GetOrCreatClassPool<ResourceObj>(1000);
        RecyclePoolTrs = recycleTrs;
        SceneTrs = sceneTrs;
    }
    /// <summary>
    /// 根据路径实例化物体
    /// </summary>
    /// <param name="path"></param>
    /// <param name="setSceneObj"></param>
    /// <param name="bClear">在跳转场景时候是否删除</param>
    /// <returns></returns>
    public GameObject InstantiateObject(string path,bool setSceneObj=false, bool bClear=true)
    {
        uint crc = Crc32.GetCrc32(path);
        ResourceObj resourceObj = GetObjectFromPool(crc);
        if (resourceObj == null)
        {
            resourceObj = m_ResourceObjClassPool.Spawn(true);
            resourceObj.m_Crc = crc;
            resourceObj.m_bClear = bClear;
            resourceObj = ResourceManager.instance.LoadResource(path, resourceObj);

            if (resourceObj.m_ResItem.m_Obj != null)
            {
                resourceObj.m_CloneObj = GameObject.Instantiate(resourceObj.m_ResItem.m_Obj) as GameObject;
                resourceObj.m_offlineData = resourceObj.m_CloneObj.GetComponent<OfflineData>();
            }
        }
        if (setSceneObj)
        {
            resourceObj.m_CloneObj.transform.SetParent(SceneTrs, false);
        }
        else
        {
            resourceObj.m_CloneObj.transform.parent = null;
        }
        int tempID = resourceObj.m_CloneObj.GetInstanceID();
        if (!m_ResourceObjDic.ContainsKey(tempID))
        {
            m_ResourceObjDic.Add(tempID, resourceObj);
        }
        return resourceObj.m_CloneObj;
    }
    /// <summary>
    /// 从对象池取对象
    /// </summary>
    /// <param name="crc"></param>
    /// <returns></returns>
    protected ResourceObj GetObjectFromPool(uint crc)
    {
       if(m_ObjctPoolDic.TryGetValue(crc,out List<ResourceObj> st) && st != null&&st.Count>0)
        {
            ResourceManager.instance.IncreaseResourceRef(crc);
            ResourceObj resObj = st[0];
            st.RemoveAt(0);
            GameObject obj = resObj.m_CloneObj;
            if (!ReferenceEquals(obj, null))
            {
                if (!ReferenceEquals(resObj.m_offlineData, null))
                {
                    resObj.m_offlineData.ResetProp();
                }
                resObj.m_Already = false;
#if UNITY_EDITOR
                if (obj.name.EndsWith("(Recycle)"))
                {
                    obj.name = obj.name.Replace("(Recycle)", "");
                }
#endif
            }
            return resObj;
        }
        return null;
    }
    /// <summary>
    /// 回收资源
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="maxCacheCount"></param>
    /// <param name="destoryCache">是否清空缓存</param>
    /// <param name="recycleParent"></param>
    public void RealaseObject(GameObject obj,int maxCacheCount=-1,bool destoryCache=false,bool recycleParent = true)
    {
        if (obj == null)
        {
            return;
        }
        int tempID = obj.GetInstanceID();
        if(!m_ResourceObjDic.TryGetValue(tempID,out ResourceObj resObj))
        {
            Debug.Log(obj.name + "对象不是ObjectManager创建的!");
            return;
        }
        if (resObj == null)
        {
            Debug.LogError("缓存的ResourceObj为空");
            return;
        }
        if (resObj.m_Already)
        {
            Debug.LogError("该对象已经放回对象池了，检测自己是否情况引用!");
            return;
        }
#if UNITY_EDITOR
        obj.name += "(Recycle)";
#endif
        List<ResourceObj> st = null;
        if (maxCacheCount == 0)
        {
            m_ResourceObjDic.Remove(tempID);
            ResourceManager.instance.RealaseResource(resObj, destoryCache);
            resObj.Reset();
            m_ResourceObjClassPool.Recycle(resObj);
        }
        else//回收到对象池
        {
            if(!m_ObjctPoolDic.TryGetValue(resObj.m_Crc,out st) || st == null)
            {
                st = new List<ResourceObj>();
                m_ObjctPoolDic.Add(resObj.m_Crc, st);
            }
            if (resObj.m_CloneObj)
            {
                if (recycleParent)
                {
                    resObj.m_CloneObj.transform.SetParent(RecyclePoolTrs);
                }
                else
                {
                    resObj.m_CloneObj.SetActive(false);
                }
            }
            if (maxCacheCount < 0||st.Count<maxCacheCount)
            {
                st.Add(resObj);
                resObj.m_Already = true;
                ResourceManager.instance.DecreaseResourcesRef(resObj);
            }
            else
            {
                m_ResourceObjDic.Remove(tempID);
                ResourceManager.instance.RealaseResource(resObj, destoryCache);
                resObj.Reset();
                m_ResourceObjClassPool.Recycle(resObj);
            }
        }
    }

    /// <summary>
    /// 预加载
    /// </summary>
    /// <param name="path">预加载路径</param>
    /// <param name="count">预加载数量</param>
    /// <param name="clear">跳场景是否清除</param>
    public void PreloadGameObject(string path,int count=1,bool clear=false)
    {
        List<GameObject> tempGameObjectList = new List<GameObject>();
        for (int i = 0; i < count; i++)
        {
            GameObject obj = InstantiateObject(path, false, bClear: clear);
            tempGameObjectList.Add(obj);
        }
        for (int i = 0; i < count; i++)
        {
            GameObject obj = tempGameObjectList[i];
            RealaseObject(obj);
            obj = null;
        }
        tempGameObjectList.Clear();
    }

    /// <summary>
    /// 异步对象加载(预制体)
    /// </summary>
    /// <param name="path"></param>
    /// <param name="dealFinish"></param>
    /// <param name="priority"></param>
    /// <param name="setSceneObject"></param>
    /// <param name="param1"></param>
    /// <param name="param2"></param>
    /// <param name="param3"></param>
    /// <param name="bClear"></param>
    /// <returns></returns>
    public long InstantiateObjectAsync(string path,OnAsyncResFinish dealFinish,LoadResPriority priority,bool setSceneObject=false,
        object param1=null,object param2=null,object param3=null,bool bClear = true)
    {
        if (string.IsNullOrEmpty(path))
        {
            return 0;
        }
        uint crc = Crc32.GetCrc32(path);
        ResourceObj resObj = GetObjectFromPool(crc);
        if (resObj != null)
        {
            if (setSceneObject)
            {
                if (!resObj.m_CloneObj.activeSelf)
                    resObj.m_CloneObj.SetActive(true);
                resObj.m_CloneObj.transform.SetParent(SceneTrs, false);
            }
            dealFinish?.Invoke(path, resObj.m_CloneObj, param1, param2, param3);
            return resObj.m_Guid;
        }
        long guid = ResourceManager.instance.CreatGuid();
        resObj = m_ResourceObjClassPool.Spawn(true);
        resObj.m_Crc = crc;
        resObj.m_SetSceneParent = setSceneObject;
        resObj.m_bClear = bClear;
        resObj.m_DealFinish = dealFinish;
        resObj.m_Guid = guid;
        resObj.m_Param1 = param1;
        resObj.m_Param2 = param2;
        resObj.m_Param3 = param3;
        if (!m_AsyncResObjs.ContainsKey(guid))
        {
            m_AsyncResObjs.Add(guid, resObj);
        }
        //调用ResourceManager的异步加载接口
        ResourceManager.instance.AsyncLoadResource(path, resObj, OnLoadResourceObjFinish, priority);
        return guid;
    }
    /// <summary>
    /// 资源加载完成回调
    /// </summary>
    /// <param name="path">路径</param>
    /// <param name="resObj">中间类</param>
    /// <param name="param1">参数1</param>
    /// <param name="param2">参数2</param>
    /// <param name="param3">参数3</param>
    void OnLoadResourceObjFinish(string path,ResourceObj resObj,object param1=null,object param2=null,object param3 = null)
    {
        if (resObj == null)
            return;
        if (resObj.m_ResItem.m_Obj == null)
        {
#if UNITY_EDITOR
            Debug.LogError("异步加载的资源为空：" + path);
#endif
        }
        else
        {
            if (resObj.m_SetSceneParent)
            {
                resObj.m_CloneObj = GameObject.Instantiate(resObj.m_ResItem.m_Obj,SceneTrs) as GameObject;
            }
            else
            {
                resObj.m_CloneObj = GameObject.Instantiate(resObj.m_ResItem.m_Obj) as GameObject;
            }
            resObj.m_offlineData = resObj.m_CloneObj.GetComponent<OfflineData>();
        }
        //加载完成就从正在加载的异步中移除
        if (m_AsyncResObjs.ContainsKey(resObj.m_Guid))
        {
            m_AsyncResObjs.Remove(resObj.m_Guid);
        }

        if(resObj.m_CloneObj!=null && resObj.m_SetSceneParent)
        {
            resObj.m_CloneObj.transform.SetParent(SceneTrs, false);
        }

        if (resObj.m_DealFinish != null)
        {
            int tempID = resObj.m_CloneObj.GetInstanceID();
            if (!m_ResourceObjDic.ContainsKey(tempID))
            {
                m_ResourceObjDic.Add(tempID, resObj);
            }
            resObj.m_DealFinish(path, resObj.m_CloneObj, resObj.m_Param1, resObj.m_Param2, resObj.m_Param3);//执行自己的方法
        }
    }

   
    /// <summary>
    /// 该对象是否是对象池创建的
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public bool IsObjectManagerCreat(GameObject obj)
    {
        ResourceObj resObj = m_ResourceObjDic[obj.GetInstanceID()];
        return resObj != null;
    }
    /// <summary>
    /// 取消异步加载
    /// </summary>
    /// <param name="guid"></param>
    public void CancleLoad(long guid)
    {
        if(m_AsyncResObjs.TryGetValue(guid,out ResourceObj resObj) && ResourceManager.instance.CancleLoad(resObj))
        {
            m_AsyncResObjs.Remove(guid);
            resObj.Reset();
            m_ResourceObjClassPool.Recycle(resObj);
        }
    }
    /// <summary>
    /// 根据实例化对象直接获取离线数据
    /// </summary>
    /// <param name="obj">GameObject</param>
    /// <returns></returns>
    public OfflineData FindOfflineData(GameObject obj)
    {
        OfflineData data = null;
        ResourceObj resObj = null;
        m_ResourceObjDic.TryGetValue(obj.GetInstanceID(),out resObj);
        if (resObj != null)
        {
            data = resObj.m_offlineData;
        }
        return data;
    }
    /// <summary>
    /// 清除某个资源在对象池中所有对象
    /// </summary>
    /// <param name="m_Crc"></param>
    public void ClearPoolObject(uint crc)
    {
        List<ResourceObj> st = null;
        if(!m_ObjctPoolDic.TryGetValue(crc,out st) || st == null)
        {
            return;
        }
        for (int i = st.Count; i >= 0; i--) 
        {
            ResourceObj resObj = st[i];
            if (resObj.m_bClear)
            {
                st.Remove(resObj);
                int tempID = resObj.m_CloneObj.GetInstanceID();
                GameObject.Destroy(resObj.m_CloneObj);
                resObj.Reset();
                m_ResourceObjDic.Remove(tempID);
                m_ResourceObjClassPool.Recycle(resObj);
            }
        }
        if (st.Count <= 0)
        {
            m_ObjctPoolDic.Remove(crc);
        }
    }
    /// <summary>
    /// 清空对象池
    /// </summary>
    public void ClearCache()
    {
        List<uint> tempList = new List<uint>();
        foreach (uint key in m_ObjctPoolDic.Keys)
        {
            List<ResourceObj> st = m_ObjctPoolDic[key];
            for (int i = st.Count - 1; i >= 0; i--) 
            {
                ResourceObj resObj = st[i];
                if (!System.Object.ReferenceEquals(resObj.m_CloneObj, null) && resObj.m_bClear)
                {
                    GameObject.Destroy(resObj.m_CloneObj);
                    m_ResourceObjDic.Remove(resObj.m_CloneObj.GetInstanceID());
                    resObj.Reset();
                    m_ResourceObjClassPool.Recycle(resObj);
                    st.Remove(resObj);
                }
            }
            if (st.Count <= 0)
            {
                tempList.Add(key);
            }
        }
        for (int i = 0; i < tempList.Count; i++)
        {
            uint temp = tempList[i];
            if (m_ObjctPoolDic.ContainsKey(temp))
            {
                m_ObjctPoolDic.Remove(temp);
            }
        }
        tempList.Clear();
    }

    #region 类对象池的使用
    protected Dictionary<Type, object> m_ClassPoolDic = new Dictionary<Type, object>();
    /// <summary>
    /// 创建类对象池，创建完成后外面可以保存ClassObjectPool<T>,然后调用Spawn和Recycle来创建和回收类对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="maxcount"></param>
    /// <returns></returns>
    public ClassObjectPool<T> GetOrCreatClassPool<T>(int maxcount)where T:class ,new()
    {
        Type type = typeof(T);
        if(!m_ClassPoolDic.TryGetValue(type,out object outObj) || outObj == null)
        {
            ClassObjectPool<T> newPool = new ClassObjectPool<T>(maxcount);
            m_ClassPoolDic.Add(type, newPool);
            return newPool;
        }
        return outObj as ClassObjectPool<T>;
    }
    #endregion
}
