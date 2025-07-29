﻿//using Cinemachine;
//using ILRuntime.CLR.Method;
//using ILRuntime.CLR.TypeSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public enum UIMsgID
{
    None = 0,
    StopTipsForBossBattle = 1,
}

public class UIManager : Singleton<UIManager>
{
    public GameObject CommonConfirm;
    //public RectTransform m_HpRoot;

    //UI节点
    public RectTransform m_UiRoot;
    //窗口节点
    public RectTransform m_WndRoot;
    //UI摄像机
    private Camera m_UICamera;
    //EventSystem节点
    private EventSystem m_EventSystem;
    //屏幕的宽高比
    private float m_CanvasRate = 0;

    private string m_UIPrefabPath = "Assets/GameData/Prefabs/UI/";
    //注册的字典
    private Dictionary<string, System.Type> m_RegisterDic = new Dictionary<string, System.Type>();
    //所有打开的窗口
    private Dictionary<string, Window> m_WindowDic = new Dictionary<string, Window>();
    /// <summary>
    /// 打开的窗口列表
    /// </summary>
    private List<Window> m_WindowList = new List<Window>();
    /// <summary>
    /// 存储所有打开过的窗口，用于外部查找窗口管理类
    /// </summary>
    private Dictionary<string, Window> m_GetWindowDic = new Dictionary<string, Window>();
  

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="uiRoot">UI父节点</param>
    /// <param name="wndRoot">窗口父节点</param>
    /// <param name="uiCamera">UI摄像机</param>
    public void Init(RectTransform uiRoot, RectTransform wndRoot, Camera uiCamera, EventSystem eventSystem)
    {
        m_UiRoot = uiRoot;
        m_WndRoot = wndRoot;
        m_UICamera = uiCamera;
        m_EventSystem = eventSystem;
        m_CanvasRate = Screen.height / (m_UICamera.orthographicSize * 2);
    }


    /// <summary>
    /// 设置所有节点UI路径
    /// </summary>
    /// <param name="path"></param>
    public void SetUIPrefabPath(string path)
    {
        m_UIPrefabPath = path;
    }

    /// <summary>
    /// 显示或者隐藏所有UI
    /// </summary>
    public void ShowOrHideUI(bool show)
    {
        if (m_UiRoot != null)
        {
            m_UiRoot.gameObject.SetActive(show);
        }
    }

    /// <summary>
    /// 设置默认选择对象
    /// </summary>
    /// <param name="obj"></param>
    public void SetNormalSelectObj(GameObject obj)
    {
        if (m_EventSystem == null)
        {
            m_EventSystem = EventSystem.current;
        }
        m_EventSystem.firstSelectedGameObject = obj;
    }


    
    /// <summary>
    /// 窗口的更新
    /// </summary>
    public void OnUpdate()
    {
        for (int i = 0; i < m_WindowList.Count; i++)
        {
            Window window = m_WindowList[i];
            if (window != null)
            {
                if (window.IsHotFix)
                {
                    ILRuntimeManager.instance.ILRunAppDomain.Invoke(window.HotFixClassName, "OnUpdate", window);
                }
                else
                {
                    window.OnUpdate();
                }
            }
        }
    }
    
    /// <summary>
    /// 窗口的更新
    /// </summary>
    public void OnLateUpdate()
    {
        for (int i = 0; i < m_WindowList.Count; i++)
        {
            Window window = m_WindowList[i];
            if (window != null)
            {
                if (window.IsHotFix)
                {
                    ILRuntimeManager.instance.ILRunAppDomain.Invoke(window.HotFixClassName, "OnLateUpdate", window);
                }
                else
                {
                    window.OnLateUpdate();
                }
            }
        }
    }

    /// <summary>
    /// 窗口注册方法
    /// </summary>
    /// <typeparam name="T">窗口泛型类</typeparam>
    /// <param name="name">窗口名</param>
    public void Register<T>(string name) where T : Window
    {
        m_RegisterDic[name] = typeof(T);
    }

    /// <summary>
    /// 发送消息给窗口
    /// </summary>
    /// <param name="name">窗口名</param>
    /// <param name="msgID">消息ID</param>
    /// <param name="paralist">参数数组</param>
    /// <returns></returns>
    public bool SendMessageToWnd(string name, UIMsgID msgID = 0, params object[] param)
    {
        Window wnd = FindWndByName<Window>(name);
        if (wnd != null)
        {
            return wnd.OnMessage(msgID);
        }
        return false;
    }

    /// <summary>
    /// 根据窗口名查找窗口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="name"></param>
    /// <returns></returns>
    private T FindWndByName<T>(string name) where T : Window
    {

        if (m_WindowDic.TryGetValue(name, out Window wnd))
        {
            return (T)wnd;
        }

        return null;
    }

    /// <summary>
    /// 更具名称查找窗口管理类，外部调用
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public Window GetWndByName(string name)
    {
        return m_GetWindowDic.ContainsKey(name) ? m_GetWindowDic[name] : null;
    }

    /// <summary>
    /// 打开窗口
    /// </summary>
    /// <param name="wndName">窗口名称</param>
    /// <param name="bTop">是否最上层</param>
    /// <param name="resource">是否从AB加载</param>
    /// <param name="para1"></param>
    /// <param name="para2"></param>
    /// <param name="para3"></param>
    /// <returns></returns>
    public Window PopUpWnd(string wndName, bool bTop = true,bool resource = false, object param1 = null, object param2 = null, object param3 = null)
    {
        Window wnd = FindWndByName<Window>(wndName);
        if (wnd == null)
        {
            //System.Type tp = null;
            if (m_RegisterDic.TryGetValue(wndName, out System.Type tp))
            {
                //wnd = System.Activator.CreateInstance(tp) as Window;
                if (resource)
                {
                    wnd = System.Activator.CreateInstance(tp) as Window;
                }
                else
                {
                    string hotName = "HotFix." + wndName.Replace("Panel.prefab", "Window");
                    wnd = ILRuntimeManager.instance.ILRunAppDomain.Instantiate<Window>(hotName);
                    wnd.IsHotFix = true;
                    wnd.HotFixClassName = hotName;
                }
            }
            else
            {
                //Debug.LogError("找不到窗口对应的脚本，窗口名是：" + wndName);
            }

            string path = m_UIPrefabPath + wndName;

          GameObject wndObj;
            //wndObj = ObjectManager.Instance.InstantiateObject(path, false, false);
            if (resource)
            {
                wndObj = Object.Instantiate(Resources.Load<GameObject>(wndName.Replace(".prefab", "")));
            }
            else
            {
                wndObj = ObjectManager.instance.InstantiateObject(path, false, false);
            }
            if (wndObj == null)
            {
                Debug.Log("创建窗口Prefab失败：" + wndName);
            }
            if(wnd == null)
            {
                string hotName = "HotFix." + wndName.Replace("Panel.prefab", "Window");
                Debug.LogError(hotName + "   ::::::::::::::::::::");
                wnd = ILRuntimeManager.instance.ILRunAppDomain.Instantiate<Window>(hotName);
                wnd.IsHotFix = true;
                wnd.HotFixClassName = hotName;
            }
            if (!m_WindowDic.ContainsKey(wndName))
            {
                m_WindowList.Add(wnd);
                m_WindowDic.Add(wndName, wnd);
            }
            if (!m_GetWindowDic.ContainsKey(wndName))
                m_GetWindowDic.Add(wndName, wnd);

            wnd.m_GameObject = wndObj;
            wnd.m_Transform = wndObj.transform;
            wnd.m_RectTransform = wndObj.GetComponent<RectTransform>();
            wnd.Name = wndName;

            wnd.Resource = resource;
            wndObj.transform.SetParent(m_WndRoot, false);

            if (!wnd.m_GameObject.activeInHierarchy)
                wnd.m_GameObject.SetActive(true);

            wnd.m_RectTransform.anchorMin = new Vector2(0, 0);
            wnd.m_RectTransform.anchorMax = new Vector2(1, 1);
            wnd.m_RectTransform.localScale = new Vector3(1, 1, 1);
            wnd.m_RectTransform.pivot = new Vector2(0.5f, 0.5f);
            wnd.m_RectTransform.offsetMin = new Vector2(0.0f, 0.0f);
            wnd.m_RectTransform.offsetMax = new Vector2(0.0f, 0.0f);
            if (wnd.IsHotFix)
            {
                ILRuntimeManager.instance.ILRunAppDomain.Invoke(wnd.HotFixClassName, "Awake", wnd, param1, param2, param3);
            }
            else
            {
                wnd.Awake(param1, param2, param3);
            }


            if (bTop)
            {
                wndObj.transform.SetAsLastSibling();
            }
            else
            {
                wndObj.transform.SetAsFirstSibling();
            }

            if (wnd.IsHotFix)
            {
                ILRuntimeManager.instance.ILRunAppDomain.Invoke(wnd.HotFixClassName, "OnShow", wnd, param1, param2, param3);
            }
            else
            {
                wnd.OnShow(param1, param2, param3);
            }
        }
        else
        {
            ShowWnd(wndName, bTop, param1, param2, param3);
        }

        return wnd;
    }

    /// <summary>
    /// 根据窗口名关闭窗口
    /// </summary>
    /// <param name="name"></param>
    /// <param name="destory"></param>
    public void CloseWnd(string name, bool destory = false)
    {
        Window wnd = FindWndByName<Window>(name);
        CloseWnd(wnd, destory);
    }

    public bool IsSignWindowOpen(string name)
    {
        bool isSign = true;
        foreach(var item in m_WindowDic)
        {
            if(name != item.Value.Name)
            {
                if (item.Value.m_GameObject.active)
                    isSign = false;
            }
        }
        if(GetWndByName(name).m_RectTransform.gameObject.active == false)
        {
            isSign = false;
        }
        return isSign;
    }

    /// <summary>
    /// 根据窗口对象关闭窗口
    /// </summary>
    /// <param name="window"></param>
    /// <param name="destory"></param>
    public void CloseWnd(Window window, bool destory = false)
    {
        if (window != null)
        {
            if (window.IsHotFix)
            {
                ILRuntimeManager.instance.ILRunAppDomain.Invoke(window.HotFixClassName, "OnDisable", window);
            }
            else
            {
                window.OnDisable();
            }
            if (window.IsHotFix)
            {
                ILRuntimeManager.instance.ILRunAppDomain.Invoke(window.HotFixClassName, "OnClose", window);
            }
            else
            {
                //if (window.IsHotFix)
                //{
                //    ILRuntimeManager.Instance.ILRunAppDomain.Invoke(window.HotFixClassName, "OnClose", window);
                //}
                //else
                //{
                    window.OnClose();
                //}
            }
            if (m_WindowDic.ContainsKey(window.Name))
            {
                m_WindowDic.Remove(window.Name);
                m_WindowList.Remove(window);
            }

            if (!window.Resource)
            {
                if (destory)
                {
                    ObjectManager.instance.RealaseObject(window.m_GameObject, 0, true);
                }
                else
                {
                    ObjectManager.instance.RealaseObject(window.m_GameObject, recycleParent: false);
                }
            }
            else
            {
                GameObject.Destroy(window.m_GameObject);
            }
            window.m_GameObject = null;
            window = null;
        }
    }

    /// <summary>
    /// 关闭所有窗口
    /// </summary>
    public void CloseAllWnd(bool isClear = false)
    {
        for (int i = m_WindowList.Count - 1; i >= 0; i--)
        {
            CloseWnd(m_WindowList[i],isClear);
        }
    }

    /// <summary>
    /// 切换到唯一窗口
    /// </summary>
    public void SwitchStateByName(string name, bool bTop = true, bool resource = false, object param1 = null, object param2 = null, object param3 = null)
    {
        CloseAllWnd();
        PopUpWnd(name, bTop, resource, param1, param2, param3);
    }

    /// <summary>
    /// 根据名字隐藏窗口
    /// </summary>
    /// <param name="name"></param>
    public void HideWnd(string name)
    {
        Window wnd = FindWndByName<Window>(name);
        HideWnd(wnd);
    }

    /// <summary>
    /// 根据窗口对象隐藏窗口
    /// </summary>
    /// <param name="wnd"></param>

    public void HideWnd(Window wnd)
    {
        if (wnd != null)
        {
            wnd.m_GameObject.SetActive(false);
            wnd.OnDisable();
        }
    }

    /// <summary>
    /// 根据窗口名字显示窗口
    /// </summary>
    /// <param name="name"></param>
    /// <param name="paralist"></param>
    public void ShowWnd(string name, bool bTop = true, object param1 = null, object param2 = null, object param3 = null)
    {
        Window wnd = FindWndByName<Window>(name);
        ShowWnd(wnd, bTop, param1, param2, param3);
    }

    /// <summary>
    /// 根据窗口对象显示窗口
    /// </summary>
    /// <param name="wnd"></param>
    /// <param name="paralist"></param>
    public void ShowWnd(Window wnd, bool bTop = true, object param1 = null, object param2 = null, object param3 = null)
    {
        if (wnd != null)
        {
            if (wnd.m_GameObject != null && wnd.m_GameObject.activeSelf == false) wnd.m_RectTransform.gameObject.SetActive(true);

            wnd.m_RectTransform.anchorMin = new Vector2(0, 0);
            wnd.m_RectTransform.anchorMax = new Vector2(1, 1);
            wnd.m_RectTransform.localScale = new Vector3(1, 1, 1);
            wnd.m_RectTransform.pivot = new Vector2(0.5f, 0.5f);
            wnd.m_RectTransform.offsetMin = new Vector2(0.0f, 0.0f);
            wnd.m_RectTransform.offsetMax = new Vector2(0.0f, 0.0f);
            if (wnd.IsHotFix)
            {
                ILRuntimeManager.instance.ILRunAppDomain.Invoke(wnd.HotFixClassName, "OnShow", wnd, param1, param2, param3);
            }
            else
            {
                wnd.OnShow(param1, param2, param3);
            }
        }
    }

    internal void OnFixUpdate()
    {
        for (int i = 0; i < m_WindowList.Count; i++)
        {
            Window window = m_WindowList[i];
            if (window != null)
            {
                if (window.IsHotFix)
                {
                    ILRuntimeManager.instance.ILRunAppDomain.Invoke(window.HotFixClassName, "OnFixUpdate", window);
                }
                else
                {
                    window.OnFixUpdate();
                }
            }
        }
    }

    internal void ClearAllWnd(bool v)
    {
        if (m_GetWindowDic != null && m_GetWindowDic.Count != 0)
        {
            m_GetWindowDic.Clear();
            m_GetWindowDic = new Dictionary<string, Window>();
        }
        if (m_RegisterDic != null && m_RegisterDic.Count != 0)
        {
            m_RegisterDic.Clear();
            m_RegisterDic = new Dictionary<string, System.Type>();
        }
        if (m_WindowDic != null && m_WindowDic.Count != 0)
        {
            m_WindowDic.Clear();
            m_WindowDic = new Dictionary<string, Window>();
        }
        if (m_WindowList != null && m_WindowList.Count != 0)
        {
            m_WindowList.Clear();
            m_WindowList = new List<Window>();
        }
    }
}
