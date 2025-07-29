using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class GameMapManager : Singleton<GameMapManager>
{

    //加载场景完成回调
    public Action LoadSceneOverCallBack;
    //加载场景开始回调
    public Action LoadSceneEnterCallBack;
    //当前场景名
    public string CurrentMapName { get; set; }

    //场景加载是否完成
    public bool AlreadyLoadScene { get; set; }

    //切换场景进度条
    public static int LoadingProgress = 0;

    private MonoBehaviour m_Mono;

    /// <summary>
    /// 场景管理初始化
    /// </summary>
    /// <param name="mono"></param>
    public void Init(MonoBehaviour mono)
    {
        m_Mono = mono;
    }

    /// <summary>
    /// 加载场景
    /// </summary>
    /// <param name="name">场景名</param>
    public void LoadScene(string name, string panelName, Action<Action> action = null, object[] objs = null)
    {
        LoadingProgress = 0;
        Debug.Log("scene name:" + name + " panelName:" + panelName);
        m_Mono.StartCoroutine(LoadSceneAsync(name));
        UIManager.instance.PopUpWnd(ConStr.LOADINGPANEL, true, false, panelName, action, objs);
    }

    /// <summary>
    /// 加载游戏场景
    /// </summary>
    /// <param name="name">场景名称</param>
    /// <param name="panelName">跳转场景之后第一个显示的界面</param>
    /// <param name="action">我进入这个场景要加载的东西 异步加载</param>
    /// <param name="data">topreparepanle界面所需的参数</param>
    /// <param name="objs">跳转场景之后第一个显示的界面所需要的参数</param>
    public void LoadGameScene(string name, string panelName, Action<Action> action = null,object[] data = null, object[] objs = null)
    {
        LoadingProgress = 0;
        Debug.Log("scene name:" + name + " panelName:" + panelName);
        m_Mono.StartCoroutine(LoadSceneAsync(name));
        UIManager.instance.CloseAllWnd();
        object[] objs2 = new object[2]; //param2转换为object[]数组 然后解析出跳转场景之后第一个显示的界面名字和异步的方法
        objs2[0] = panelName;
        objs2[1] = action;
        UIManager.instance.PopUpWnd("ToPreparePanel.prefab", true, false, data, objs2, objs);
    }

    /// <summary>
    /// 设置场景环境
    /// </summary>
    /// <param name="name"></param>
    void SetSceneSetting(string name)
    {
        //设置各种场景环境，可以根据配表来,TODO:
    }

    IEnumerator LoadSceneAsync(string name)
    {
        LoadSceneEnterCallBack?.Invoke();
        ClearCache();
        AlreadyLoadScene = false;
        AsyncOperation unLoadScene = SceneManager.LoadSceneAsync(ConStr.EMPTYSCENE, LoadSceneMode.Single);
        while (unLoadScene != null && !unLoadScene.isDone)
        {
            yield return new WaitForEndOfFrame();
        }

        LoadingProgress = 0;
        int targetProgress = 0;
        AsyncOperation asyncScene = SceneManager.LoadSceneAsync(name);
        if (asyncScene != null && !asyncScene.isDone)
        {
            asyncScene.allowSceneActivation = false;//没有加载完不允许显示该场景
            while (asyncScene.progress < 0.9f)
            {
                targetProgress = (int)asyncScene.progress * 100;
                yield return new WaitForEndOfFrame();
                //平滑过渡
                while (LoadingProgress < targetProgress)
                {
                    ++LoadingProgress;
                    yield return new WaitForEndOfFrame();
                }
            }

            CurrentMapName = name;
            SetSceneSetting(name);
            //自行加载剩余的10%
            targetProgress = 88;
            while (LoadingProgress < targetProgress - 22)
            {
                ++LoadingProgress;
                yield return new WaitForEndOfFrame();
            }
            LoadingProgress = 88;
            asyncScene.allowSceneActivation = true;
            AlreadyLoadScene = true;

            //LoadSceneOverCallBack?.Invoke();
        }
    }

    /// <summary>
    /// 跳场景需要清除的东西
    /// </summary>
    private void ClearCache()
    {
        ObjectManager.instance.ClearCache();
        ResourceManager.instance.ClearCache();
    }
}
