using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UI;
using UnityEngine;
using HotFix.Common.Utils;
using LitJson;

namespace HotFix
{
    internal class PrayWindow:Window
    {
        private RawImage iconImg;
        private InputField desInput;
        private Text nameText;
        private Button closeBtn, sureBtn;
        private string id;
        private PrayData prayData;

        public override void Awake(object param1 = null, object param2 = null, object param3 = null)
        {
            GetAllComponent();
            AddAllBtnListener();
        }

        public override void OnShow(object param1 = null, object param2 = null, object param3 = null)
        {
            prayData = (PrayData)param1;
            UpdateUI(param2 as Texture, prayData.name);
            id = prayData.id;
            
             
        }

        private void UpdateUI(Texture texture, string v)
        {
            desInput.text = "";
            iconImg.texture = texture;
            nameText.text = v;
        }

        private void GetAllComponent()
        {
            iconImg = m_Transform.Find("BackImg/IconImg").GetComponent<RawImage>() ;
            desInput = m_Transform.Find("BackImg/DesText/InputField").GetComponent<InputField>();
            nameText = m_Transform.Find("BackImg/IconImg/Name").GetComponent<Text>();
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
            if (string.IsNullOrEmpty(desInput.text))
            {
                RFrameWork.instance.OpenCommonConfirm("提示", "请输入祝福语～", () => { },null);
            }
            else
            {
                if (UserInfoManager.yuBeiNum < UserInfoManager.prayCostNum)
                {
                    RFrameWork.instance.OpenCommonConfirm("渔贝不足", "您的渔贝不足，暂时无法购买福袋哦～", () => { }, () => { });
                }
                else
                {
                    BuyPrayFunc(AddFishFunc);
                }
                UIManager.instance.CloseWnd(this);
            }
        }



        private void AddFishFunc(string str)
        {
            JsonData data = new JsonData();
            data["propId"] = id;
            data["pwd"] = str;
            data["remarks"] = desInput.text;
            string jsonStr = JsonMapper.ToJson(data);
           // WebRequestManager.instance.AsyncLoadUnityWebRequest(WebRequestUtils.buyOrderCreate, BuyPray, true, jsonStr, RFrameWork.instance.token);
        }

        private void BuyPray(string jsonStr)
        {
            JsonData jsonData = JsonMapper.ToObject(jsonStr);
            string code = jsonData["code"].ToString();
            if (code.Equals("200"))
            {
                string prayMsg= desInput.text.Length>=15 ? desInput.text.Substring(0, 15): desInput.text;               
                int pos = int.Parse(jsonData["data"].ToString());
                //WebRequestManager.instance.AsyncLoadUnityWebRequest(WebRequestUtils.fishPondUser, MainWindow.RefreshUserDatasAction, true, "{}", RFrameWork.instance.token);
                object[] objs = new object[] { "祈福成功", "祈福语：" + prayMsg + "……", true, prayData.type, pos };
                UIManager.instance.CloseWnd(FilesName.PASSWORDINFOPANEL);
                Window tipWindow = UIManager.instance.GetWndByName(FilesName.PRAYTIPSPANEL);
                if (tipWindow != null)
                {
                    if (tipWindow.m_GameObject.activeSelf)
                    {
                        Debug.Log("祈福列表打开中。。。");
                        PrayTipsWindow.PraySuccessAction();
                    }
                    else
                    {
                        Debug.Log("祈福列表关闭中。。。");
                        UIManager.instance.PopUpWnd(FilesName.PRAYTIPSPANEL, true, false);

                    }

                }
                else
                {
                    UIManager.instance.PopUpWnd(FilesName.PRAYTIPSPANEL, true, false);

                }
                UIManager.instance.PopUpWnd(FilesName.IFSUCCEEDPANEL, true, false, objs,true);
                
            }
            else
            {
                UIManager.instance.CloseWnd(FilesName.PASSWORDINFOPANEL);
                RFrameWork.instance.OpenCommonConfirm("提示", jsonData["errorMsg"].ToString(), () => { }, () => { });
            }
        }

        private void BuyPrayFunc(Action<string> addFishFunc)
        {
            UIManager.instance.PopUpWnd(FilesName.PASSWORDINFOPANEL, true, false, addFishFunc);
        }
    }
}
