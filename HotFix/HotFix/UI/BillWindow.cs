using HotFix.Common.Utils;
using LitJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace HotFix
{
    internal class BillWindow : Window
    {
        private Button returnBtn;
        private Button yuBeiBtn;
        private Text yuBeiNum;
        private Transform yuBeiDesBack;
        private Button closeBackBtn;
        private Button mingXiBtn;
        private Button zengSongBtn;
        private Transform content;
        private Transform item;
        List<BillData> list = new List<BillData>();
        public override void Awake(object param1 = null, object param2 = null, object param3 = null)
        {
            GetAllComponent();
            AddAllBtnListener();
        }

        public override void OnShow(object param1 = null, object param2 = null, object param3 = null)
        {
            yuBeiDesBack.gameObject.SetActive(false);
            list = param1 as List<BillData>;
            UpdateUI();
        }

        private void GetAllComponent()
        {
            returnBtn = m_Transform.Find("ReturnBtn").GetComponent<Button>();
            yuBeiNum = m_Transform.Find("YuBeiBtn/Text").GetComponent<Text>();
            yuBeiBtn = m_Transform.Find("YuBeiBtn").GetComponent<Button>();
            yuBeiDesBack = m_Transform.Find("YuBeiBtn/Text/DesBack");
            closeBackBtn = m_Transform.Find("YuBeiBtn/Text/DesBack/CloseBtn").GetComponent<Button>();
            mingXiBtn = m_Transform.Find("YuBeiBtn/Text/DesBack/MingXiBtn").GetComponent<Button>();
            zengSongBtn = m_Transform.Find("YuBeiBtn/Text/DesBack/ZengSongBtn").GetComponent<Button>();
            content = m_Transform.Find("Scroll View/Viewport/Content");
            item = m_Transform.Find("Scroll View/Viewport/Content/Item");
        }

        private void AddAllBtnListener()
        {
            AddButtonClickListener(returnBtn, ReturnFunc);
            AddButtonClickListener(yuBeiBtn, () => { ShowYuBeiPanel(true); });
            AddButtonClickListener(closeBackBtn, () => { ShowYuBeiPanel(false); });
            AddButtonClickListener(mingXiBtn, MingXiFunc);
            AddButtonClickListener(zengSongBtn, ZengSongFunc);
        }

        private void UpdateUI()
        {
            yuBeiNum.text = "          " + UserInfoManager.yuBeiNum + "         ";
            for (int i = 0; i < content.childCount; i++)
            {
                content.GetChild(i).gameObject.SetActive(false);
            }
            int count = 0;
            foreach (var data in list)
            {
                if (count + 1 <= content.childCount)
                {
                    BillItem item = new BillItem();
                    item.OnInit(content.GetChild(count), data);
                    item = null;
                }
                else
                {
                    Transform obj = Object.Instantiate<GameObject>(this.item.gameObject, content).transform;
                    BillItem item = new BillItem();
                    item.OnInit(obj, data);
                    item = null;
                }
                count++;
            }
        }

        private void ReturnFunc()
        {
            UIManager.instance.CloseWnd(this);
        }

        private void ShowYuBeiPanel(bool isShow)
        {
            yuBeiDesBack.gameObject.SetActive(isShow);
        }



        private void MingXiFunc()
        {
            //查询渔贝明细 TODO
            JsonData data = new JsonData();
            data["appid"] = "6541654124";
            data["ticket"] = "4654576454";
            string jsonStr = JsonMapper.ToJson(data);
            WebRequestManager.instance.AsyncLoadUnityWebRequest(WebRequestUtils.jourMyPage, Func, true, jsonStr, "key");
            WebRequestFuncitons.GetMyBillData();
        }

        private void Func(string jsonStr)
        {
            //jsonstr = (3)result：
            //            {
            //“code”:0,
            //“msg”:”ok”,
            //“data”:”steamid” //steam返回的用户唯一标识
            //}
        }

        private void ZengSongFunc()
        {
            //赠送渔贝 TODO
            UIManager.instance.PopUpWnd(FilesName.ADDFISHMONEYPANEL, true, false);
        }
    }
}
