using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UI;
using UnityEngine;

namespace HotFix
{
    internal class PrayItem
    {
        private Transform item;
        public Text name,priceText;
        public Button btn;
        public RawImage icon;

        public void OnInit(Transform itemTrans,PrayData data)
        {
            item = itemTrans;
            icon = item.GetComponent<RawImage>();
            name = item.Find("Name").GetComponent<Text>();
            priceText = item.Find("Price").GetComponent<Text>();
            btn = icon.GetComponent<Button>();
            btn.onClick.AddListener(() =>
            {
               
                UIManager.instance.PopUpWnd(FilesName.PRAYPANEL, true, false, data, icon.texture);
                UIManager.instance.CloseWnd(FilesName.PRAYSELECTPANEL);
            });
            item.gameObject.SetActive(true);
            UpdateUI(data.image, data.name,data.price);
        }

        private void UpdateUI(string url, string name,string price)
        {
            icon.texture = null;
            this.name.text = name;
            priceText.text = price+"渔贝";
        }
    }
}
