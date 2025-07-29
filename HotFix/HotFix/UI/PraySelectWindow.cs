using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace HotFix
{
    internal class PraySelectWindow:Window
    {
        private Button returnBtn;
        private Text expendPriceTxt;
        private Transform item;
        private Transform content;
        private List<PrayData> list = new List<PrayData>();
        public override void Awake(object param1 = null, object param2 = null, object param3 = null)
        {

            GetAllComponent();
            AddButtonClickListener(returnBtn, () =>
            {
                UIManager.instance.CloseWnd(this);
            });
        }

        public override void OnShow(object param1 = null, object param2 = null, object param3 = null)
        {
            list.Clear();
            list = param1 as List<PrayData>;
            UpdateUI();
        }

        private void GetAllComponent()
        {
            returnBtn = m_Transform.Find("CloseBtn").GetComponent<Button>();
            expendPriceTxt = m_Transform.Find("Back/TitleText/Des").GetComponent<Text>();
            content = m_Transform.Find("Back/PrayList");
            item = m_Transform.Find("Back/PrayList/IconImg");
        }

        private void UpdateUI()
        {
            if(list != null&&list.Count>0)
            {
                UserInfoManager.prayCostNum =float.Parse(list[0].price);
               // expendPriceTxt.text = string.Format("一次祈福需消耗{0}渔贝", list[0].price);
            }
            for (int i = 0; i < content.childCount; i++)
            {
                content.GetChild(i).gameObject.SetActive(false);
            }
            int count = 0;
            foreach (var data in list)
            {
                if (count + 1 <= content.childCount)
                {
                    PrayItem item = new PrayItem();
                    item.OnInit(content.GetChild(count), data);
                    item = null;
                }
                else
                {
                    Transform obj = Object.Instantiate<GameObject>(this.item.gameObject, content).transform;
                    PrayItem item = new PrayItem();
                    item.OnInit(obj, data);
                    item = null;
                }
                count++;
            }
        }
    }
}
