
using HotFix.Common.Utils;
using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEngine;

namespace HotFix
{
    /// <summary>
    /// 工具类
    /// </summary>
    public static class ToolManager
    {
        public static string privacyUrl;
        /// <summary>
        /// 退出游戏
        /// </summary>
        public static void ExitGame()
        {
             
            JsonData data2 = new JsonData();
            data2["isExit"] = "1";            
            string jsonStr = JsonMapper.ToJson(data2);
            //WebRequestManager.instance.AsyncLoadUnityWebRequest(WebRequestUtils.dailyRewardUrl, (string str) => { }, true, jsonStr, RFrameWork.instance.token);            //WebRequestManager.instance.AsyncLoadUnityWebRequest(WebRequestUtils.dailyRewardUrl, (string str) => { }, true, jsonStr, RFrameWork.instance.token);
            JsonData data = new JsonData();
            MsgBase close = new MsgBase("Close", data.ToJson());
            UserInfoManager.Init();
            RFrameWork.instance.QuitTheGame();
            //NetManager.instance.Send(close);
            //NetManager.instance.Close();
        }
        /// <summary>
        /// 退出游戏
        /// </summary>
        public static void StartExitGame()
        {
            JsonData data = new JsonData();
            MsgBase close = new MsgBase("Close", data.ToJson());
            RFrameWork.instance.QuitTheGame();
            //NetManager.instance.Send(close);
            //NetManager.instance.Close();
        }
        /// <summary>                        
        /// thi Transform root给Transform添加扩展方法，扩展方法为在根节点查找name的子物体
        /// 查找子物体
        /// </summary>
        /// <param name="root"></需要查找物体的根节点>
        /// <param name="name"></需要查找子物体的名称>
        /// <returns></returns>
        public static Transform FindRecursively(this Transform root, string name)
        {

            if (root.name == name)
            {
                return root;
            }
            //遍历根节点下的所有子物体
            foreach (Transform child in root)
            {
                //递归查找子物体
                Transform t = FindRecursively(child, name);
                if (t != null)
                {
                    return t;
                }
            }
            return null;
        }

        /// <summary>
        /// 查找root身上的monobehaviour组件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="root"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static T FindBehaviour<T>(this Transform root, string name) where T : MonoBehaviour
        {
            Transform child = FindRecursively(root, name);

            if (child == null)
            {
                return null;
            }

            T temp = child.GetComponent<T>();
            if (temp == null)
            {
                Debug.LogError(name + " is not has component ");
            }

            return temp;
        }


        /// <summary>
        /// 字符串转整型;
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int Str2Int(string str)
        {
            if (IsNumber(str))
            {
                return string.IsNullOrEmpty(str) == true ? 0 : Convert.ToInt32(str);
            }
            return 0;

        }
        /// <summary>
        /// 常量A是否全是数字
        /// </summary>
        public static string A = "^[0-9]+$";
        /// <summary>
        /// 利用正则表达式来判断字符串是否全是数字
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private static bool IsNumber(string str)
        {
            Regex regex = new Regex(A);
            return regex.IsMatch(str) == true ? true : false;
        }
        /// <summary>
        /// 字符串转成float;
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static float Str2Float(string str)
        {
            return string.IsNullOrEmpty(str) == true ? 0 : Convert.ToSingle(str);
        }
        /// <summary>
        /// 字符串转成bool;
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool Str2Boolean(string str)
        {
            if (str == "1" || str.ToLower() == "true" || str.ToLower() == "yes")
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 设置预制体包括全部子级的layer
        /// </summary>
        /// <param name="root"></param>
        /// <param name="layer"></param>
        public static void SetChildrensLayer(Transform root, int layer)
        {
            Transform[] all = root.GetComponentsInChildren<Transform>();
            foreach (var item in all)
            {
                item.gameObject.layer = layer;
            }
        }

        public static IEnumerator WaitSecondsToTranslate(Transform aim, string typeName = null, TranslateType translateType = TranslateType.None)
        {

            UserInfoManager.selectPlayer.enabled = false;
            UserInfoManager.selectPlayer.transform.position = aim.position;
            UserInfoManager.selectPlayer.transform.rotation = aim.rotation;
            yield return new WaitForSeconds(0.2f);
            UserInfoManager.selectPlayer.transform.position = aim.position;
            UserInfoManager.selectPlayer.transform.rotation = aim.rotation;
            UserInfoManager.selectPlayer.enabled = true;
            if (!string.IsNullOrEmpty(typeName))
            {
                UIManager.instance.CloseWnd(typeName);
            }
            if (translateType != TranslateType.None)
            {
                Window window = UIManager.instance.GetWndByName(FilesName.PRAYTIPSPANEL);
                if (window != null)
                {
                    UIManager.instance.CloseWnd(FilesName.PRAYTIPSPANEL);
                }
                if(translateType==TranslateType.Gulf)
                {
                    if (!GetAudioClipName().Equals("ocean"))
                    {
                        RFrameWork.instance.SetBackAudio("SingleSounds/ocean");
                    }
                }
                else if (translateType == TranslateType.IsLand)
                {
                    if (!GetAudioClipName().Equals("island"))
                    {
                        RFrameWork.instance.SetBackAudio("SingleSounds/island");
                    }
                    
                }
                else if (translateType == TranslateType.Lighting)
                {
                    if (!GetAudioClipName().Equals("lighting"))
                    {
                        RFrameWork.instance.SetBackAudio("SingleSounds/lighting");
                    }
                }
                else if (translateType == TranslateType.OldTown)
                {
                    if(!GetAudioClipName().Equals("oldTown"))
                    {
                        RFrameWork.instance.SetBackAudio("SingleSounds/oldTown");
                    }
                    
                }
            }



        }
   
        public static string GetAudioClipName()
        {
            AudioSource audioSource = RFrameWork.instance.audioManager;
            Debug.Log("当前音乐名字："+ audioSource.clip.name);
            return audioSource.clip.name;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="texture"></param>
        public static void DestoryDownLoadTexture(Texture texture)
        {
            MonoBehaviour.DestroyImmediate(texture);
            Resources.UnloadUnusedAssets();
            System.GC.Collect();
        }
    
    }
}
