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
    internal class BagItems
    {
        private Transform itemParent;
        private Transform itemTrans;
        private Transform content;
        
        public void OnInit(Transform item, List<FishData> data)
        {
            itemParent = item;
            itemParent.gameObject.SetActive(true);
            content = itemParent.Find("Back");
            itemTrans = content.GetChild(0);
            UpdateUI(data);
        }

        private void UpdateUI(List<FishData> list)
        {
            for (int i = 0; i < content.childCount; i++)
            {
                content.GetChild(i).gameObject.SetActive(false);
            }
            int count = 0;
            foreach (var data in list)
            {
                if (count + 1 <= content.childCount)
                {
                    BagItem item = new BagItem();
                    item.OnInit(content.GetChild(count), data);
                    item = null;
                }
                else
                {
                    Transform obj = Object.Instantiate<GameObject>(itemParent.gameObject, content).transform;
                    BagItem item = new BagItem();
                    item.OnInit(obj, data);
                    item = null;
                }
                count++;
            }
        }
    }
    internal class BagItem
    {
        private Transform item;
        private FishData data;
        private RawImage icon;
        private Image bg;
        private Text weight;
        private Text name;
        private Text price;
        private Button marketBtn;
        private Color maskColor = new Color(1, 1, 1, 0.7f);
        public void OnInit(Transform itemTrans, FishData data)
        {
            item = itemTrans;
            this.data = data;
            bg = item.Find("Bg").GetComponent<Image>();
            icon = item.Find("Bg/Icon").GetComponent<RawImage>();
            weight = item.Find("Bg/Icon/Num/Text").GetComponent<Text>();
            name = item.Find("Name").GetComponent<Text>();
            price = item.Find("Price/PriceText").GetComponent<Text>();
            marketBtn = item.Find("SellBtn").GetComponent<Button>();
            marketBtn.onClick.AddListener(() =>
            {
                SellFishFunc();
            });
            item.gameObject.SetActive(true);
            UpdateUI();
        }

        private void SellFishFunc()
        {
            //出售鱼货 TODO
            UIManager.instance.PopUpWnd(FilesName.MARKETFISHPANEL, true, false,data);
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
            if (float.Parse(data.totalQuantity) <= 0)
            {
                bg.color = maskColor;
                icon.color = maskColor;
            }
            else
            {
                icon.color = Color.white;
                bg.color = Color.white;
            }
            name.text = data.name;
            price.text = data.repurchasePrice+"/KG";
            weight.text = " "+data.totalQuantity + "KG ";
            marketBtn.interactable = !(float.Parse(data.totalQuantity) <= 0);
        }
    }
}
