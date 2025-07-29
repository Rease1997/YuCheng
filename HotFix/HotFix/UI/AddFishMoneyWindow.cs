using HotFix.Common.Utils;
using LitJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

namespace HotFix
{
    internal class AddFishMoneyWindow:Window
    {
        private Button returnBtn;
        private Button sureBtn;
        private Button addBtn;
        private Button minusBtn;
        private InputField inputNum;
        private InputField inputPhoneNum;
        private Text fishMoneyText;
        private int count = 1;

        public override void Awake(object param1 = null, object param2 = null, object param3 = null)
        {
            FindAllComponent();
            AddAllBtnListener();
        }

        public override void OnShow(object param1 = null, object param2 = null, object param3 = null)
        {
            count = 1;
            inputNum.text = count.ToString();

            UpdateUI();
            inputNum.text = "1";
            inputPhoneNum.text = "";
        }

        private void FindAllComponent()
        {
            returnBtn = m_Transform.Find("ReturnBtn").GetComponent<Button>();
            sureBtn = m_Transform.Find("Back/SureBtn").GetComponent<Button>();
            addBtn = m_Transform.Find("Back/InputBabk/AddBtn").GetComponent<Button>();
            minusBtn = m_Transform.Find("Back/InputBabk/MinusBtn").GetComponent<Button>();
            inputNum = m_Transform.Find("Back/InputBabk/InputField").GetComponent<InputField>();
            inputPhoneNum = m_Transform.Find("Back/InputBabk2/InputField").GetComponent<InputField>();
            fishMoneyText = m_Transform.Find("Back/InputBabk/Des2").GetComponent<Text>();
        }

        private void AddAllBtnListener()
        {
            AddButtonClickListener(returnBtn, ReturnFunc);
            AddButtonClickListener(sureBtn, SureFunc);
            AddButtonClickListener(addBtn, AddFunc);
            AddButtonClickListener(minusBtn, MinusFunc);
            this.inputNum.onValueChanged.RemoveListener(OnValueChanged);
            this.inputNum.onValueChanged.AddListener(OnValueChanged);
        }

        private void OnValueChanged(string arg0)
        {
            count = int.Parse(arg0);
        }

        private void UpdateUI()
        {
            fishMoneyText.text = "拥有：" + UserInfoManager.yuBeiNum + "渔贝";
        }

        private void ReturnFunc()
        {
            UIManager.instance.CloseWnd(this);
        }

        private void SureFunc()
        {
            //确定赠送 TODO
            if(int.Parse(inputNum.text)<= UserInfoManager.yuBeiNum)
            {
                JsonData data = new JsonData();
                data["mobile"] = inputPhoneNum.text;
                string jsonStr = JsonMapper.ToJson(data);
                if(string.IsNullOrEmpty(inputPhoneNum.text))
                {
                    RFrameWork.instance.OpenCommonConfirm("提示", "请输入正确的手机号", () => { }, null);

                }
                else
                {
                    //WebRequestManager.instance.AsyncLoadUnityWebRequest(WebRequestUtils.getUserPhoto, GetUserName, true, jsonStr, RFrameWork.instance.token);
                    GetUserName();
                }
            }
            else
            {
                UIManager.instance.PopUpWnd(FilesName.TIPSPANEL, true, false, "  渔贝不足  ", 3);
            }
        }

        private void GetUserName(/*string jsonStr*/)
        {
            string name = "渔光之城用户";
            UIManager.instance.PopUpWnd(FilesName.INCREASEPANEL, true, false, inputNum.text, inputPhoneNum.text, name);
            MainWindow.RefreshUserDatasAction();
            //WebRequestManager.instance.AsyncLoadUnityWebRequest(WebRequestUtils.fishPondUser, MainWindow.RefreshUserDatasAction, true, "{}", RFrameWork.instance.token);
        }

        private void AddFunc()
        {
            count++;
            inputNum.text = count.ToString();
        }
        private void MinusFunc()
        {
            count--;
            if (count < 1)
            {
                count = 1;
            }
            inputNum.text = count.ToString();
        }
    }

}
