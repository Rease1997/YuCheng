using HotFix.Common.Utils;
using LitJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace HotFix
{
    internal class MarketFishWindow : Window
    {
        private Button returnBtn;
        private Text title;
        private RawImage icon;
        private InputField num;
        private Button allBtn;
        private Text haveNum;
        private Text price;
        private Text numText;
        private Button payBtn;

        private FishData data;

        public override void Awake(object param1 = null, object param2 = null, object param3 = null)
        {
            GetAllComponent();
            AddAllBtnListener();
        }

        public override void OnShow(object param1 = null, object param2 = null, object param3 = null)
        {
            data = param1 as FishData;
            UpdateUI();
        }

        private void UpdateUI()
        {
            icon.texture = null;
            //WebRequestManager.instance.AsyncLoadUnityTexture(data.image, (tx) =>
            //{
            //    if (icon.texture != null)
            //    {
            //        ToolManager.DestoryDownLoadTexture(icon.texture);
            //    }
            //    icon.texture = tx;
            //});
            if (float.Parse(data.totalQuantity) > 1)
            {
                num.text = "1";
            }
            else
            {
                num.text = data.totalQuantity;
            }
            title.text = "出售" + data.name;
            haveNum.text = "拥有" + data.totalQuantity + "KG";
            price.text = data.repurchasePrice + "渔贝/KG";
            numText.text = float.Parse(num.text) * float.Parse(data.repurchasePrice) + "渔贝";
        }

        private void GetAllComponent()
        {
            returnBtn = m_Transform.Find("ReturnBtn").GetComponent<Button>();
            title = m_Transform.Find("Back/TitleText").GetComponent<Text>();
            icon = m_Transform.Find("Back/IconBack/IconImg").GetComponent<RawImage>();
            num = m_Transform.Find("Back/NumBack/InputField").GetComponent<InputField>();
            allBtn = m_Transform.Find("Back/NumBack/AllBtn").GetComponent<Button>();
            haveNum = m_Transform.Find("Back/HaveNum").GetComponent<Text>();
            price = m_Transform.Find("Back/PriceText").GetComponent<Text>();
            numText = m_Transform.Find("Back/NumText").GetComponent<Text>();
            payBtn = m_Transform.Find("Back/PayBtn").GetComponent<Button>();
        }

        private void AddAllBtnListener()
        {
            AddButtonClickListener(returnBtn, ReturnFunc);
            AddButtonClickListener(allBtn, AllFunc);
            AddButtonClickListener(payBtn, PayFunc);
            this.num.onValueChanged.RemoveListener(OnValueChanged);
            this.num.onValueChanged.AddListener(OnValueChanged);
        }

        private void OnValueChanged(string str)
        {
            numText.text = float.Parse(str) * float.Parse(data.repurchasePrice) + "渔贝";
        }

        private void ReturnFunc()
        {
            UIManager.instance.CloseWnd(this);
        }

        private void AllFunc()
        {
            //出售全部 TODO
            num.text = data.totalQuantity;
        }

        private void PayFunc()
        {
            //出售 TODO
            if (float.Parse(num.text) <= float.Parse(data.totalQuantity))
            {
                JsonData data = new JsonData();
                data["varietyId"] = this.data.id;
                data["repurchaseNumber"] = this.num.text;
               // WebRequestManager.instance.AsyncLoadUnityWebRequest(WebRequestUtils.fishPondSale, MarketFish, true, JsonMapper.ToJson(data), RFrameWork.instance.token);
            }
            else
            {
                UIManager.instance.PopUpWnd(FilesName.TIPSPANEL, true, false, "  鱼货不足  ", 3);
            }
        }

        private void MarketFish(string jsonStr)
        {
            //JsonData jsonData = JsonMapper.ToObject(jsonStr);
            //string code = jsonData["code"].ToString();
            //if (code.Equals("200"))
            //{
            //    object[] objs = new object[] { "出售成功", "获得：" + jsonData["data"]["totalAmount"].ToString() + "渔贝", true };
            //    UIManager.instance.PopUpWnd(FilesName.IFSUCCEEDPANEL, true, false, objs);
            //    WebRequestManager.instance.AsyncLoadUnityWebRequest(WebRequestUtils.fishPondUser, MainWindow.RefreshUserDatasAction, true, "{}", RFrameWork.instance.token);
            //    UserInfoManager.refreshBag = true;
            //    WebRequestManager.instance.AsyncLoadUnityWebRequest(WebRequestUtils.listFront, WebRequestFuncitons.MyFishList, true, "{}", RFrameWork.instance.token);
            //    UIManager.instance.CloseWnd(FilesName.MARKETFISHPANEL);
            //}
            //else
            //{
            //    object[] objs = new object[] { "出售失败", jsonData["errorMsg"].ToString(), false };
            //    UIManager.instance.PopUpWnd(FilesName.IFSUCCEEDPANEL, true, false, objs);
            //}
        }
    }
}
