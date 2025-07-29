using UnityEngine.UI;
using LitJson;
using UnityEngine;
using System;
using System.Collections.Generic;
using HotFix.Common.Utils;

namespace HotFix
{
    /// <summary>
    /// 游戏初始动画界面
    /// </summary>
    class StartWindow : Window
    {
        /// <summary>
        /// 开始游戏按钮
        /// </summary>
        private Button startGame;
        public Text text;

        public static Action<string> setText;

        public override void Awake(object param1 = null, object param2 = null, object param3 = null)
        {
            startGame = m_GameObject.transform.Find("StartGame").GetComponent<Button>();
            startGame.interactable = true;
            text = m_GameObject.transform.Find("StartGame/Text").GetComponent<Text>();
            Debug.Log("开始游戏拉啦啦啦啦啦");
            setText = setTextData;
            AddButtonClickListener(startGame, () =>
            {
                Debug.Log(RFrameWork.instance.token);
                CheckRoleFunc();
                text.text = "开始进入渔光之城";
                //UIManager.instance.PopUpWnd(FilesName.MAINPANEL);

            });

        }

        private void CheckRoleFunc(/*string jsonStr*/)
        {
            string roleFlag = "0"; //是否选择过人物
            string sex = "0"; //选择的角色性别
            string status = "4"; //状态 0:无角色,1:岛民,2:渔民,3:商人,4:全部
            string type = "0"; //选择的角色类型 0:岛民,1:渔民,2:商人
            UserInfoManager.playerSex = sex;
            UserInfoManager.playerType = type;
            UserInfoManager.palyerStatus = status;
            if (roleFlag == "1")
            {

                //进入游戏 TODO
                UIManager.instance.PopUpWnd(FilesName.JOYSTICKPANEL, true, false, false);
                UIManager.instance.PopUpWnd(FilesName.SELECTPLAYERPANEL, true, false, false, true);
            }
            else
            {
                string[] strs = new string[3] { type, sex, status };
                UIManager.instance.PopUpWnd(FilesName.JOYSTICKPANEL, true, false, false);
                UIManager.instance.PopUpWnd(FilesName.SELECTPLAYERPANEL, true, false, false, false, strs);
            }
            UIManager.instance.CloseWnd(this);
            //    JsonData jsonData = JsonMapper.ToObject(jsonStr);
            //    string code = jsonData["code"].ToString();
            //    if (code.Equals("200"))
            //    {
            //        string roleFlag = jsonData["data"]["roleFlag"].ToString(); //是否选择过人物
            //        string sex = jsonData["data"]["sex"].ToString(); //选择的角色性别
            //        string status = jsonData["data"]["status"].ToString(); //状态 0:无角色,1:岛民,2:渔民,3:商人,4:全部
            //        string type = jsonData["data"]["type"].ToString(); //选择的角色类型 0:岛民,1:渔民,2:商人

            //        if(!status.Equals("0"))
            //        {
            //            UserInfoManager.playerSex = sex;
            //            UserInfoManager.playerType = type;
            //            UserInfoManager.palyerStatus = status;
            //            if (roleFlag == "1")
            //            {

            //                //进入游戏 TODO
            //                UIManager.instance.PopUpWnd(FilesName.JOYSTICKPANEL, true, false, false);
            //                UIManager.instance.PopUpWnd(FilesName.SELECTPLAYERPANEL, true, false, false, true);
            //            }
            //            else
            //            {
            //                string[] strs = new string[3] { type, sex, status };
            //                UIManager.instance.PopUpWnd(FilesName.JOYSTICKPANEL, true, false, false);
            //                UIManager.instance.PopUpWnd(FilesName.SELECTPLAYERPANEL, true, false, false, false, strs);

            //            }
            //        }


            //        UIManager.instance.CloseWnd(this);
            //    }
            //    else if(code.Equals("500"))
            //    {
            //        RFrameWork.instance.OpenCommonConfirm("提示", jsonData["errorMsg"].ToString(), () => { ToolManager.ExitGame(); }, () => { });

            //    }
            //    else
            //    {
            //        RFrameWork.instance.OpenCommonConfirm("提示", jsonData["errorMsg"].ToString(), () => { ToolManager.ExitGame(); }, null);
            //    }
        }

        private void setTextData(string obj)
        {
            text.text = obj;
        }

    }
}
