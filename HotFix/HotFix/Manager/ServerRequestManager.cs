using HotFix.Common;
using HotFix.Common.Utils;
using LitJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace HotFix
{
    public class ServerRequestManager
    {
        public void Start(string url,string token)
        {
            //NetManager.instance.AddListener(NetEvent.ConnectFail, OnConnectFailed);
            //NetManager.instance.AddListener(NetEvent.ConnectSucc, OnConnectSuccess);
            WebRequestUtils.InitUrl(url,token);//http://47.96.110.234:1818/api/core/v1/
            Debug.Log("ServerRequestManager Start");
        }
        private void OnConnectFailed(string str)
        {
            Debug.Log("ServerRequestManager OnConnectFailed:");
            //NetManager.instance.OnStartReconnect();

        }
        private void OnConnectSuccess(string str)
        {
            Debug.Log("OnConnectSuccess 连接服务器成功："+str);
            if (UIManager.instance.CommonConfirm)
            {
                Debug.Log("OnConnectSuccess 连接服务器成功：关闭提示弹窗");
                UIManager.instance.CommonConfirm.GetComponent<CommonConfirm>().CloseSelf();
            }
            Debug.Log("OnConnectSuccess 连接服务器成功：" + GameMapManager.instance.CurrentMapName);
            if (string.IsNullOrEmpty(GameMapManager.instance.CurrentMapName))
                return;
            if(GameMapManager.instance.CurrentMapName.Equals("Main"))
            {
                //Debug.Log("OnConnectSuccess 连接服务器成功 JoinScene");
                //NetManager.instance.Send(new MsgBase(RequestCode.JoinScene.ToString(), JsonUtility.ToJson(new JoinScene("1", UserInfoManager.userID))));

            }
            if (GameMapManager.instance.CurrentMapName.Equals("Game"))
            {
                //Debug.Log("ServerRequestManager OnConnectSuccess BackMainScene");
                //UserInfoManager.enterGame = false;
                //Animator cameraAnim = GameObject.Find("Camera").GetComponent<Animator>();
                //cameraAnim.enabled = false;
                //cameraAnim.transform.GetComponent<Cinemachine.CinemachineBrain>().enabled = true;
                //UserInfoManager.isGoToSiyangchang = false;
                //GameMapManager.instance.LoadScene(ConStr.MAINSCENE, FilesName.MAINPANEL, HouseManager.LoadMainScene);
                //UIManager.instance.CloseWnd(FilesName.ROOMPANEL);
                //UIManager.instance.CloseWnd(FilesName.PLAYPANEL);
                //NetManager.instance.Send(new MsgBase(RequestCode.JoinScene.ToString(), JsonUtility.ToJson(new JoinScene("1", UserInfoManager.userID))));

            }
            Debug.Log("ServerRequestManager OnConnectSuccess");
        }
    }
}
