using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class IOSManager : MonoBehaviour
{
#if UNITY_IPHONE
    [DllImport("__Internal")]
    static extern void StartUnity();
    [DllImport("__Internal")]
    static extern void ExitUnity();
#endif
    public static string tokenInfo = "";
    public static bool isHave;
    public static bool isBack;
    public static IOSManager instance;
    public Button btn;

    void Start()
    {
        Debug.Log("IOSManager Start:"+ isHave);
        instance = this;
        if (!isHave)
        {

            Debug.Log("IOSManager 启动Unity");
            DontDestroyOnLoad(this);
            isHave = true;
            LaunchUnityToIOS();

        }
        else
        {
            Debug.Log("IOSManager 退出返回到IOS:"+isBack);
            
#if UNITY_IPHONE
            if(isBack)
            {
                isBack = false;
                ExitUnity();
                
            }
            
#endif

        }
    }
 
    public static void ExitUnityToOtherUI()
    {
#if UNITY_IPHONE
        UIManager.instance.ClearAllWnd(true);
        ResourceManager.instance.ClearAllObject();
        ObjectManager.instance.ClearAllPoolObject();
        MessageCenter.instance.ClearAllListener();
        AssetBundleManager.instance.ClearAllAb();
        HotPatchManager.instance.ClearAllData();
        AsyncOperation op = SceneManager.LoadSceneAsync("Loading");
        op.allowSceneActivation = true;
#endif

    }
    public void LaunchUnityToIOS()
    {
#if UNITY_IPHONE
        StartUnity();
#endif
    }
    public void SetFrontUrlAndToken(string jsonStr)
    {
#if UNITY_IPHONE
        tokenInfo = jsonStr;
        Debug.Log("GetTokenCallBack tokenInfo:" + tokenInfo);
         StartCoroutine(LoadScene());
        
#endif

    }
    public void TestRequest()
    {
       
       string tokenId = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiI2NTQ3NjcxMDEyMTg5MjI0OTYiLCJleHAiOjE2Nzk2MzYyMzl9.hkLm9eK2BJDxmAE_SnHHWTcNn6Rv7bQ19ypuUdYwjzg";
        JsonData jsonData = new JsonData();
        jsonData["token"] = tokenId;
        jsonData["fronturl"] = "http://m.dev.hzyuzhouyuan.com//core/v1/";
        jsonData["wxAppId"] = "wxae9f09088940baa1";
        this.SetFrontUrlAndToken(jsonData.ToJson().ToString());

    }
    IEnumerator LoadScene()
    {
        yield return new WaitForSeconds(0.5f);
        int disableProgress = 0;
        int toProgress = 0;
        AsyncOperation op = SceneManager.LoadSceneAsync("Start");
        op.allowSceneActivation = false;//允许场景打开
        while (op.progress < 0.9f)
        {
            toProgress = (int)(op.progress * 100);
            while (disableProgress < toProgress)
            {
                ++disableProgress;
                yield return new WaitForEndOfFrame();
            }
        }
        toProgress = 100;
        while (disableProgress < toProgress)
        {
            ++disableProgress;
             yield return new WaitForEndOfFrame();
        }
        op.allowSceneActivation = true;
    }



   
    
    
}



