using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class RFrameWork : UnitySingleton<RFrameWork>
{
    public string token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiI1MDMzNTYwODMxNTU1MDkyNDgiLCJleHAiOjE2NzAzOTI3MzV9.3Z2nCXESVJwj52bL-i60jhYNFR5mbkAJLMIj_P-PWTc";
    public string frontUrl = "http://192.168.1.21:80/";// "http://192.168.10.8:6903/v1/"; 研发：http://m.dev.ygzcmeta.com/core/v1/ 测试：http://m.test.hzyuzhouyuan.com//core/v1/
    public string platform = "Android";
    public string httpPath = "http://192.168.1.7:80/";
    public string ip = "47.97.123.168";
    public int ipPort = 8107;
    public bool useHotFix = true;
    public bool checkReconnect = true;
    private bool isStartGame;
    public AudioSource audioManager;
    public AudioSource otherAudio;
    private AndroidJavaObject mJavaObject;
    
#if UNITY_IPHONE
    [DllImport("__Internal")]
    static extern void StartUnity();
    [DllImport("__Internal")]
    static extern void ExitUnity();
    [DllImport("__Internal")]
    static extern void payYeeBank(string url1, string url2);
    [DllImport("__Internal")]
    static extern void payWX(string merchantId, string nonceStr, string prepayId, string timeStamp, string wechatPackage, string paySign);
    [DllImport("__Internal")]
    static extern void payAli(string signOrder);
    [DllImport("__Internal")]
    static extern void payH5(string url);
#endif
    public void SetFrontUrlAndToken(string jsonStr)
    {
        Debug.LogError("app传来的字符串" + jsonStr);
        JsonData data = JsonMapper.ToObject(jsonStr);
        frontUrl = data["fronturl"].ToString();
        token = data["token"].ToString();
        string[] ipArray= data["ip"].ToString().Split('|') ;
        ip = ipArray[0];
        ipPort = int.Parse(ipArray[1]);

    }
    public void PurchaseResultCallBack(string jsonStr)
    {
        Debug.LogError("app传来的字符串" + jsonStr);

        var payMgr = ILRuntimeManager.instance.ILRunAppDomain.Instantiate("HotFix.PurchaseManager");
        ILRuntimeManager.instance.ILRunAppDomain.Invoke("HotFix.PurchaseManager", "SetStateOfPayment", payMgr, jsonStr);
    }

    public override void Awake()
    {
//        StartApp();
//        Debug.LogError("RFrameWork执行awake  token:" + token + "   url:" + frontUrl);
//#if UNITY_ANDROID 
//        platform = "Android";
//        httpPath = "https://m.ygzcmeta.com/resource";
//        HotPatchManager.instance.xmlUrl = "https://m.ygzcmeta.com/resource/AndroidServerInfo.xml";
//#endif
//#if UNITY_IPHONE
//        platform = "iOS";
//        httpPath = "https://m.ygzcmeta.com/resource"; //https://m.ygzcmeta.com/resource 
//        HotPatchManager.instance.xmlUrl = "https://m.ygzcmeta.com/resource/iOSServerInfo.xml";
//        if(!string.IsNullOrEmpty(IOSManager.tokenInfo))
//        {
//            SetFrontUrlAndToken(IOSManager.tokenInfo);
//        }
        

//#endif
        //#if UNITY_EDITOR
        //        httpPath = "http://192.168.20.224";
        //        HotPatchManager.Instance.xmlUrl = "http://192.168.20.224/"+platform+"ServerInfo.xml";
        //#endif 
        base.Awake();
        DontDestroyOnLoad(gameObject);
        ResourceManager.instance.m_LoadFormAssetBundle = useHotFix;
        WebRequestManager.instance.Init(this);
        ResourceManager.instance.Init(this);
        //AssetBundleManager.Instance.LoadAssetBundleConfig();
        ObjectManager.instance.Init(transform.Find("UIRoot/RecyclePoolTrs"), transform.Find("UIRoot/SceneTrs"));
        GameMapManager.instance.Init(this);
        HotPatchManager.instance.Init(this);
        ObjectsManager.instance.Init();
        InitUiManager();
        RegisterUI();
        if (audioManager == null)
        {
            audioManager = GameObject.Find("AudiosManager").GetComponent<AudioSource>();
        }
        if (otherAudio == null)
        {
            otherAudio = GameObject.Find("OtherAudio").GetComponent<AudioSource>();
        }
    }
    
    // Start is called before the first frame update
    private void StartApp()
    {
        try
        {


#if UNITY_ANDROID
            AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            if (jc != null)
            {
                mJavaObject = jc.GetStatic<AndroidJavaObject>("currentActivity");
                if (mJavaObject == null)
                {
                    Debug.LogError("AndroidJavaObject is null...");
                    return;
                }
                else
                {
                    DontDestroyOnLoad(this);
                    mJavaObject.Call("StartUnity");
                }
            }
            else
            {
                Debug.LogError("AndroidJavaClass is null...");
                return;

            }
#elif UNITY_IPHONE
            StartUnity();
#endif
        }
        catch (Exception e)
        {
            Debug.LogError("error:" + e.Message);
        }
    }

    public void QuitTheGame()
    {
        try
        {
            Application.Quit();
#if UNITY_EDITOR
            
#endif
            //#if UNITY_ANDROID
            //                if (mJavaObject == null)
            //                {
            //                    Debug.LogError("AndroidJavaObject is null...");
            //                    return;
            //                }
            //                else
            //                {
            //                    mJavaObject.Call("ExitUnity");
            //                }

            //#elif UNITY_IPHONE
            //            IOSManager.isBack = true;
            //            IOSManager.ExitUnityToOtherUI();
            //            DestroyImmediate(gameObject);
            //#endif
        }
        catch (Exception e)
        {
            Debug.LogError("error:" + e.Message);
        }
    }

    public void CallWeChatPay(string appId, string merchantId, string prepayId, string wechatPackage, string nonceStr, string timeStamp, string sign)
    {
        Debug.Log("AndroidManager CallWeChatPay:" + appId + "  " + merchantId + "  " + prepayId);
#if UNITY_ANDROID
        mJavaObject.Call("payWx", appId, merchantId, prepayId, wechatPackage, nonceStr, timeStamp, sign);
#elif UNITY_IPHONE
        payWX(merchantId, nonceStr, prepayId, timeStamp, wechatPackage, sign);
#endif
    }
    public void CallALiPay(string signOrder)
    {
        Debug.Log("AndroidManager CallALiPay:" + signOrder);
#if UNITY_ANDROID
        mJavaObject.Call("payAli", signOrder);
#elif UNITY_IPHONE
        payAli(signOrder);
#endif

    }
    public void CallH5Pay(string url)
    {
        Debug.Log("AndroidManager CallH5Pay:" + url);
#if UNITY_ANDROID
        mJavaObject.Call("payH5", url);
#elif UNITY_IPHONE
        payH5(url);
#endif

    }
    void Start()
    {
        UIManager.instance.PopUpWnd(ConStr.HOTFIXPANEL, true, true);
    }
    /// <summary>
    /// 打开热更新界面
    /// </summary>
    /// <param name="title"></param>
    /// <param name="str"></param>
    /// <param name="confirmAction"></param>
    /// <param name="cancleAction"></param>
    public void OpenCommonConfirm(string title, string str, UnityEngine.Events.UnityAction confirmAction = null, UnityEngine.Events.UnityAction cancleAction = null)
    {
        Debug.Log("RFrameWork OpenCommonConfirm");
        if (UIManager.instance.CommonConfirm == null)
        {
            UIManager.instance.CommonConfirm = Instantiate(Resources.Load<GameObject>("CommonConfirm"));
        }
        GameObject commonObj = UIManager.instance.CommonConfirm;
        commonObj.transform.SetParent(UIManager.instance.m_WndRoot, false);
        CommonConfirm commonItem = commonObj.GetComponent<CommonConfirm>();
        commonItem.Show(title, str, confirmAction, cancleAction);
    }

    private void InitUiManager()
    {
        RectTransform uiRoot = transform.Find("UIRoot") as RectTransform;
        RectTransform winRoot = transform.Find("UIRoot/WndRoot") as RectTransform;
        Camera uiCamera = transform.Find("UIRoot/UICamera").GetComponent<Camera>();
        EventSystem uiEvent = transform.Find("UIRoot/EventSystem").GetComponent<EventSystem>();
        UIManager.instance.Init(uiRoot, winRoot, uiCamera, uiEvent);
    }

    void RegisterUI()
    {
        UIManager.instance.Register<Window>(ConStr.LOADINGPANEL);
        UIManager.instance.Register<HotFixUI>(ConStr.HOTFIXPANEL);
    }

    public IEnumerator StartGame(Slider image, Text text)
    {
        image.value = 0;
        yield return null;
        text.text = "333加载本地数据.....";
        Debug.Log("加载本地数据");
        AssetBundleManager.instance.LoadAssetBundleConfig();
        image.value = 0.1f;
        yield return null;
        text.text = "加载DLL.....";
        Debug.Log("333加载DLL");
        ILRuntimeManager.instance.Init(this);
        image.value = 0.2f;
        yield return null;
        text.text = "加载数据表......";
        //LoadConfiger();
        image.value = 0.3f;
        yield return null;
        text.text = "加载配置......";
        image.value = 0.4f;
        yield return null;
        text.text = "初始化地图......";
        GameMapManager.instance.Init(this);
        image.value = 0.5f;
        yield return null;
        text.text = "初始化地图......";
        image.value = 0.6f;
        yield return null;
        text.text = "连接服务器......";
        var toolMgr = ILRuntimeManager.instance.ILRunAppDomain.Instantiate("HotFix.ToolManager");
        isStartGame = true;
        //if (frontUrl.Contains("dev"))
        //{
        //    Debug.Log("研发");
        //    ip = "47.97.123.168";
        //    NetManager.instance.Connect(ip, ipPort); //本地 192.168.10.8  云服务器：47.96.110.234  192.168.10.8:8107
        //    ILRuntimeManager.instance.ILRunAppDomain.Invoke("HotFix.ToolManager", "SetFrontUrl", toolMgr, "dev");
        //    transform.Find("Reporter").gameObject.SetActive(true);
        //}
        //else if (frontUrl.Contains("test"))
        //{
        //    Debug.Log("测试");
        //    ip = "112.124.2.32";
        //    NetManager.instance.Connect(ip, ipPort); //本地 192.168.10.8  云服务器：47.96.110.234  192.168.10.8:8107
        //    ILRuntimeManager.instance.ILRunAppDomain.Invoke("HotFix.ToolManager", "SetFrontUrl", toolMgr, "test");
        //    transform.Find("Reporter").gameObject.SetActive(true);
        //}
        //else if (frontUrl.Contains("stage"))
        //{
        //    Debug.Log("演示");
        //    ip = "121.41.30.239";
        //    NetManager.instance.Connect(ip, ipPort); //本地 192.168.10.8  云服务器：47.96.110.234  192.168.10.8:8107
        //    ILRuntimeManager.instance.ILRunAppDomain.Invoke("HotFix.ToolManager", "SetFrontUrl", toolMgr, "test");
        //    transform.Find("Reporter").gameObject.SetActive(true);
        //}
        //else
        //{
        //    Debug.Log("线上");
        //    ip = "47.114.87.157";
        //    NetManager.instance.Connect(ip, ipPort);
        //    ILRuntimeManager.instance.ILRunAppDomain.Invoke("HotFix.ToolManager", "SetFrontUrl", toolMgr, "real");
        //    transform.Find("Reporter").gameObject.SetActive(false);
        //}

        image.value = 0.8f;
        yield return null;
        image.value = 1f;
        yield return null;
        //InitWebRequest();
    }

    private void InitWebRequest()
    {
        var serverMgr = ILRuntimeManager.instance.ILRunAppDomain.Instantiate("HotFix.ServerRequestManager");
        ILRuntimeManager.instance.ILRunAppDomain.Invoke("HotFix.ServerRequestManager", "Start", serverMgr, frontUrl, token);
        MessageCenter.instance.Dispatch(-200);
    }

    public void SetBackAudio(string musicName)
    {
        Debug.Log("音乐名字：" + musicName);
        AudioClip clip = ResourceManager.instance.LoadResources<AudioClip>("Assets/GameData/Sounds/" + musicName + ".mp3");
        audioManager.clip = clip;
        audioManager.Play();
    }

    public void SetOtherAudio(string musicName)
    {
        AudioClip clip = ResourceManager.instance.LoadResources<AudioClip>("Assets/GameData/Sounds/" + musicName + ".mp3");
        otherAudio.clip = clip;
        otherAudio.Play();
    }

    // Update is called once per frame
    void Update()
    {
        transform.ResetLocal();
        UIManager.instance.OnUpdate();
        ObjectsManager.instance.OnUpdate();
       // NetManager.instance.Update();
    }

    private void FixedUpdate()
    {
        ObjectsManager.instance.OnFixUpdate();
        UIManager.instance.OnFixUpdate();
    }

    private void LateUpdate()
    {
        ObjectsManager.instance.OnLateUpdate();
        UIManager.instance.OnLateUpdate();
    }
    private void OnApplicationFocus(bool focus)
    {
        //Debug.Log("RFrameWork OnApplicationFocus:" + focus);
        //if(focus)
        //{
        //    if (Application.internetReachability == NetworkReachability.NotReachable)
        //    {
        //        if(isStartGame)
        //        {
        //            NetManager.instance.OnStartReconnect();

        //        }

        //    }
        //    else
        //    {
        //        if (isStartGame)
        //        {
        //            if (!NetManager.instance.HasConnected())
        //            {
        //                NetManager.instance.OnStartReconnect();
        //            }
        //        }
        //    }
        //}
    }
    public void QuitGame()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#endif
    }
}
