using Cysharp.Threading.Tasks;
using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class WebRequestManager : Singleton<WebRequestManager>
{
    private MonoBehaviour m_StartMono;
    public void Init(MonoBehaviour mono)
    {
    }

    //http://47.96.110.234:1818/api/core/v1/cuser/my_photo
    public async UniTask<string> AsyncLoadUnityWebRequest(string url, Action<string> dealfinish, bool isKey = false, string getByte = "", string key = "")
    {
        string response = null;
        Debug.LogError("发送的url接口为"+url+"  jsonstr:"+getByte);
        using (UnityWebRequest webRequest = new UnityWebRequest(url, "POST"))
        {
            if (isKey)
            {
                byte[] postBytes = System.Text.Encoding.UTF8.GetBytes(getByte);
                webRequest.uploadHandler = (UploadHandler)new UploadHandlerRaw(postBytes);
                webRequest.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
                webRequest.SetRequestHeader("Content-Type", "application/json");
                webRequest.SetRequestHeader("Authorization", key);
            }

            await webRequest.SendWebRequest();
            if (webRequest.result == UnityWebRequest.Result.ProtocolError || webRequest.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.LogError(webRequest.error);
            }
            else
            {
                response = webRequest.downloadHandler.text;
                if (string.IsNullOrEmpty(response))
                {
                    Debug.LogError("获取的值为空 UnityWebRequestManager " + key + " response is null");
                }
                else
                {
                    Debug.Log("WebRequest获取到的Json字符串：\n" + response);
                    JsonData jsonData = JsonMapper.ToObject(response);
                    string code = jsonData["code"].ToString();
                    if (code.Equals("600"))
                    {
                        RFrameWork.instance.OpenCommonConfirm("提示", "未知错误\n"+"发送的url接口为"+url+"\njsonstr:"+getByte, () => { Tools.ExitGame(); }, null);
                    }
                    else
                    {
                        dealfinish(response);
                    }
                    dealfinish = null;
                }
            }
            try
            {
                webRequest.Dispose();
                Debug.Log("request dispose start stop coroutine 结束WebRequest拉取协程 断开链接");
            }
            catch
            {
                Debug.LogError("断开链接失败 request dispose error2" + webRequest.error);
            }
        }
        return response;
    }


    public async UniTask<Texture> AsyncLoadUnityTexture(string key, Action<Texture> dealfinish)
    {
        string imgName = GetImageName(key);
        string imgPath = "CollectionTexture/" + imgName;
        Debug.Log("本地加载图片名字是str:" + imgPath);
        Texture tx = Resources.Load(imgPath, typeof(Texture)) as Texture;
        if (tx == null)
        {
                string url = string.Empty;
                if (!key.Contains("http"))
                {
                    url = "https://fish-meta.oss-accelerate.aliyuncs.com/" + key;
                }
                else
                {
                    url = key;
                }
                Debug.Log(" 启动网络加载 CollectionItem LoadTexture：" + url);
                using (UnityWebRequest uwr = UnityWebRequest.Get(url))
                {
                    DownloadHandlerTexture texture1 = new DownloadHandlerTexture(true);
                    uwr.downloadHandler = texture1;
                    await uwr.SendWebRequest();//开始请求
                    if (uwr.isDone && string.IsNullOrEmpty(uwr.error) && (!uwr.isNetworkError))
                    {
                        Debug.Log("图片加载成功");
                        if (texture1.isDone)
                        {
                            tx = texture1.texture;
                        }
                    }
                    if (uwr.isNetworkError || uwr.isHttpError)
                    {
                        Debug.LogError("网络错误：" + url);
                        RFrameWork.instance.OpenCommonConfirm("网络错误", "网络错误，请检查您的网络", () => {

                        }, null);
                    }
                }
        }
        dealfinish(tx);
        return tx;
    }
    public string GetImageName(string url)
    {
        string imgName = null;
        if (url.Contains("http"))
        {
            string[] strArr = url.Split('/');
            imgName = strArr[strArr.Length - 1].Split('.')[0];
        }
        else
        {
            imgName = url.Split('.')[0];
        }
        return imgName;
    }
    //public async UniTask<Texture> LoadTextureByAb(string imgUrl, Action<Texture> m_action)
    //{
    //    Debug.Log("UIManager LoadTextrue url:" + imgUrl);
    //    string imgName = GetImageName(imgUrl);
    //    string imgPath = "CollectionTexture/" + GetImageName(imgUrl);
    //    Debug.Log("本地加载图片名字是str:" + imgPath);
    //    Texture tx = Resources.Load(imgPath, typeof(Texture)) as Texture;
    //    if (tx != null)
    //    {
    //        m_action(tx);
    //        await UniTask.DelayFrame(1);
    //    }
    //    else if (loadTextures.ContainsKey(imgName) && loadTextures[imgName] != null)
    //    {
    //        Debug.Log("本地不存在，但已经加载过:" + imgPath);
    //        m_action(loadTextures[imgName]);
    //        tx = loadTextures[imgName];
    //        await UniTask.DelayFrame(1);
    //    }
    //    else
    //    {
    //        if (!imgUrl.Contains("http"))
    //        {
    //            imgUrl = "https://metaculture.oss-accelerate.aliyuncs.com/" + imgUrl;

    //        }
    //        string playForm = "";
    //        if(RFrameWork.instance.platform == "iOS")
    //        {
    //            playForm = "IOS";
    //        }
    //        else
    //        {
    //            playForm = "Android";
    //        }
    //        string abUrl = "https://metaculture.oss-cn-hangzhou.aliyuncs.com/u3d/" + playForm + "/" + GetImageName(imgUrl) + ".ab";
    //        Debug.Log(" 启动网络加载 CollectionItem LoadTexture AB：" + abUrl);
    //        using (UnityWebRequest request = UnityWebRequestAssetBundle.GetAssetBundle(abUrl))
    //        {
    //            await request.SendWebRequest();
    //            if (request.result == UnityWebRequest.Result.ConnectionError)
    //            {
    //                m_action(null);
    //                Debug.LogError("图片加载失败:" + request.result);
    //                //UIManager.Instance.toastInfoManager.ShowSingleMsg("图片地址不存在或者加载失败", null);
    //            }
    //            if (request.result == UnityWebRequest.Result.ProtocolError)
    //            {
    //                Debug.LogError("图片ab地址不存在加载失败:" + request.result);
    //                await AsyncLoadUnityTexture(imgUrl, m_action);
    //            }
    //            if (request.isDone && string.IsNullOrEmpty(request.error))
    //            {
    //                if (loadTextures.ContainsKey(imgName) && loadTextures[imgName] != null)
    //                {
    //                    Debug.Log("本地不存在，但已经加载过:" + imgPath);
    //                    m_action(loadTextures[imgName]);
    //                    tx = loadTextures[imgName];
    //                    await UniTask.DelayFrame(1);
    //                }
    //                AssetBundle assetBundle = (request.downloadHandler as DownloadHandlerAssetBundle).assetBundle;
    //                if (assetBundle != null)
    //                {
    //                    Debug.Log("AB包不为空 成功");
    //                    if (!string.IsNullOrEmpty(imgName))
    //                    {
    //                        Texture tx2 = assetBundle.LoadAsset<Texture>(imgName);
    //                        m_action(tx2);
    //                        tx = tx2;
    //                        if (!loadTextures.ContainsKey(imgName))
    //                        {
    //                            loadTextures.Add(imgName, tx2);
    //                        }
    //                    }
    //                    await UniTask.DelayFrame(1);
    //                    assetBundle.Unload(false);
    //                }

    //            }
    //            try
    //            {
    //                request.Dispose();

    //                Debug.Log("request dispose start");
    //            }
    //            catch
    //            {
    //                Debug.LogError("request dispose error2" + request.error);
    //            }
    //        }
    //    }
    //    return tx;
    //}
}
