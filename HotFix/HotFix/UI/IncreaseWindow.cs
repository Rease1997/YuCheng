using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UI;
using UnityEngine;
using HotFix.Common.Utils;
using LitJson;
using System.Runtime.InteropServices.ComTypes;
using static System.Net.Mime.MediaTypeNames;
using Text = UnityEngine.UI.Text;

namespace HotFix
{
    internal class IncreaseWindow:Window
    {
        private Text des1;
        private Text des2;
        private Text name;
        private Button closeBtn;
        private Button sureBtn;
        private string num;
        private string phoneNum;

        public override void Awake(object param1 = null, object param2 = null, object param3 = null)
        {
            GetAllComponent();
            AddAllBtnListener();
        }

        public override void OnShow(object param1 = null, object param2 = null, object param3 = null)
        {
            //inputNum.text, inputPhoneNum.text,name
            UpdateUI(param1.ToString(),param2.ToString(),param3.ToString());
        }

        private void UpdateUI(string des1,string des2,string name)
        {
            this.des1.text = "赠送："+des1+"渔贝";
            this.des2.text = "接收账号：" + des2;
            char[] arr = name.ToCharArray();
            this.name.text = "昵称：" + arr[0] + "**" + arr[arr.Length - 1];
            num = des1;
            phoneNum = des2;
        }

        private void GetAllComponent()
        {
            des1 = m_Transform.Find("BackImg/BackImage/DesText1").GetComponent<Text>();
            des2 = m_Transform.Find("BackImg/BackImage/DesText2").GetComponent<Text>();
            name = m_Transform.Find("BackImg/BackImage/Name").GetComponent<Text>();
            closeBtn = m_Transform.Find("BackImg/CloseBtn").GetComponent<Button>();
            sureBtn = m_Transform.Find("BackImg/SureBtn").GetComponent<Button>();
        }

        private void AddAllBtnListener()
        {
            AddButtonClickListener(closeBtn, CloseFunc);
            AddButtonClickListener(sureBtn, SureFunc);
        }

        private void CloseFunc()
        {
            UIManager.instance.CloseWnd(this);
        }

        private void SureFunc()
        {
            //立即赠送 TODO
            PassWordFunc(AddFishFunc);
        }

        private void AddFishFunc(string str)
        {
            JsonData data = new JsonData();
            data["loginName"] = phoneNum;
            data["quantity"] = num;
            data["pwd"] = str;
            string jsonStr = JsonMapper.ToJson(data);
            AddFishServerFunc();
            //WebRequestManager.instance.AsyncLoadUnityWebRequest(WebRequestUtils.reCordCreate, AddFishServerFunc, true, jsonStr, RFrameWork.instance.token);
        }

        private void AddFishServerFunc(/*string jsonStr*/)
        {
            MainWindow.RefreshUserDatasAction();
            //WebRequestManager.instance.AsyncLoadUnityWebRequest(WebRequestUtils.fishPondUser, MainWindow.RefreshUserDatasAction, true, "{}", RFrameWork.instance.token);
            object[] objs = new object[] { "赠送渔贝成功", "共向尾号「" + phoneNum.Substring(phoneNum.Length - 4, 4) + "」用户赠送" + num + "渔贝", true };
            UIManager.instance.PopUpWnd(FilesName.IFSUCCEEDPANEL, true, false, objs);
            MainWindow.DonateSuccessAction();
            //JsonData jsonData = JsonMapper.ToObject(jsonStr);
            //string code = jsonData["code"].ToString();
            //if (code.Equals("200"))
            //{
            //    MainWindow.RefreshUserDatasAction();
            //    //WebRequestManager.instance.AsyncLoadUnityWebRequest(WebRequestUtils.fishPondUser, MainWindow.RefreshUserDatasAction, true, "{}", RFrameWork.instance.token);
            //    object[] objs = new object[] { "赠送渔贝成功", "共向尾号「" + phoneNum.Substring(phoneNum.Length - 4, 4) + "」用户赠送"+ num + "渔贝", true };
            //    UIManager.instance.PopUpWnd(FilesName.IFSUCCEEDPANEL, true, false, objs);
            //     MainWindow.DonateSuccessAction();
            //}
            //else
            //{
            //    object[] objs = new object[] { "赠送失败", jsonData["errorMsg"].ToString(), false };
            //    UIManager.instance.PopUpWnd(FilesName.IFSUCCEEDPANEL, true, false, objs);
            //}
        }

        private void PassWordFunc(Action<string> addFishFunc)
        {
            UIManager.instance.PopUpWnd(FilesName.PASSWORDINFOPANEL, true, false,addFishFunc);
        }
    }
}
