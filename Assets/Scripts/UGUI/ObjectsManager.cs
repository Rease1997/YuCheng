using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class ObjectsManager : Singleton<ObjectsManager>
{
    private List<ObjectParent> m_ObjectList = new List<ObjectParent>();

    public void Init()
    {
        if (m_ObjectList.Count > 0)
        {
            m_ObjectList.Clear();
            m_ObjectList = new List<ObjectParent>();
        }
    }

    public ObjectParent AddObject(GameObject obj,string objName,string className, object param1 = null, object param2 = null, object param3 = null)
    {
        ObjectParent the_obj = new ObjectParent();
        string hotName = "HotFix." + className;
        the_obj = ILRuntimeManager.instance.ILRunAppDomain.Instantiate<ObjectParent>(hotName);
        the_obj.HotFixClassName = hotName;
        if (obj == null)
        {
            Debug.Log("创建Prefab失败：" + objName);
        }
        if(the_obj == null)
        {
            hotName = "HotFix." + className;
            the_obj = ILRuntimeManager.instance.ILRunAppDomain.Instantiate<ObjectParent>(hotName);
            the_obj.HotFixClassName = hotName;
        }

        the_obj.m_GameObject = obj;
        the_obj.m_Transform = obj.transform;
        the_obj.Name = objName;

        if (!the_obj.m_GameObject.activeInHierarchy)
            the_obj.m_GameObject.SetActive(true);
        if (the_obj != null)
        {
            ILRuntimeManager.instance.ILRunAppDomain.Invoke(the_obj.HotFixClassName, "Awake", the_obj, param1, param2, param3);
            ILRuntimeManager.instance.ILRunAppDomain.Invoke(the_obj.HotFixClassName, "OnShow", the_obj, param1, param2, param3);
        }
        m_ObjectList.Add(the_obj);
        return the_obj;
    }

    /// <summary>
    /// 预制体的更新
    /// </summary>
    public void OnUpdate()
    {
        for (int i = 0; i < m_ObjectList.Count; i++)
        {
            ObjectParent obj = m_ObjectList[i];
            if (obj != null&& obj.m_GameObject!=null)
            {
                ILRuntimeManager.instance.ILRunAppDomain.Invoke(obj.HotFixClassName, "OnUpdate", obj);
            }
            else
            {
                obj = null;
                m_ObjectList.RemoveAt(i);
            }
        }
    }

    /// <summary>
    /// 物理逻辑的更新
    /// </summary>
    public void OnFixUpdate()
    {
        for (int i = 0; i < m_ObjectList.Count; i++)
        {
            ObjectParent obj = m_ObjectList[i];
            if (obj != null && obj.m_GameObject != null)
            {
                ILRuntimeManager.instance.ILRunAppDomain.Invoke(obj.HotFixClassName, "OnFixUpdate", obj);
            }
            else
            {
                obj = null;
                m_ObjectList.RemoveAt(i);
            }
        }
    }

    public void OnLateUpdate()
    {
        for (int i = 0; i < m_ObjectList.Count; i++)
        {
            ObjectParent obj = m_ObjectList[i];
            if (obj != null && obj.m_GameObject != null)
            {
                ILRuntimeManager.instance.ILRunAppDomain.Invoke(obj.HotFixClassName, "OnLateUpdate", obj);
            }
            else
            {
                obj = null;
                m_ObjectList.RemoveAt(i);
            }
        }
    }

    public void OnRemove(ObjectParent objP)
    {
        m_ObjectList.Remove(objP);
    }

    public void SetHighLight(GameObject obj)
    {
        Outline highLight = obj.GetComponent<Outline>();
        highLight.enabled = true;
    }

    public void SetHighLightClose(GameObject obj)
    {
        Outline highLight = obj.GetComponent<Outline>();
        highLight.enabled = false;
    }
}
