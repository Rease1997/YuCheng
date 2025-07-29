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
    internal class FishingOverWindow : Window
    {
        private RawImage icon;
        private Text name, descTxt;
        private Transform content;
        private Transform item;
        private Button sureBtn;
        private List<FishingOverData> fishDataList;
        public override void Awake(object param1 = null, object param2 = null, object param3 = null)
        {
            GetAllComponent();
            AddAllBtnListener();
        }

        public override void OnShow(object param1 = null, object param2 = null, object param3 = null)
        {
            
            fishDataList = param1 as List<FishingOverData>;
            Debug.Log("页面打开捕鱼的长度是："+fishDataList.Count);
            if (fishDataList.Count > 0)
            {
                OnUpdateData();
                
            }

        }
        private void OnUpdateData()
        {

            UpdateUI(fishDataList[0].image, fishDataList[0].boatName);
            LayoutRebuilder.ForceRebuildLayoutImmediate(content.GetComponent<RectTransform>());

        }
        private void UpdateUI(string photoUrl, string nameStr)
        {
            icon.texture = null;
            //WebRequestManager.instance.AsyncLoadUnityTexture(photoUrl, (tx) =>
            //{
            //    if (icon.texture != null)
            //    {
            //        ToolManager.DestoryDownLoadTexture(icon.texture);
            //    }
            //    icon.texture = tx;
            //});
            name.text = "「" + nameStr + "」捕鱼完成";
            TextExtensionUtils.SetTextWithEllipsis(name, name.text, 20);
            RefreshItem();      
            //OnTest();
        }
        private void OnTest()
        {
            for(int i=0;i<fishDataList.Count;i++)
            {
                Debug.Log("移除后剩余的name:" + fishDataList[i].boatName + "  id:" + fishDataList[i].id);
            }
        }
        private void RefreshItem()
        {
            for (int i = 0; i < content.childCount; i++)
            {
                content.GetChild(i).gameObject.SetActive(false);
            }
            FishingOverData fishData = fishDataList[0];
            if (fishData.recordDetailList.Count <= 0)
            {
                descTxt.text = "很抱歉，您本次出船没有捕到任何鱼货";
                fishDataList.Remove(fishData);
                return;
            }
            descTxt.text = "共收获：";
            int count = 0;           
            foreach (var data in fishData.recordDetailList)
            {
                if (count + 1 <= content.childCount)
                {
                    FishItem item = new FishItem();
                    item.OnInit(content.GetChild(count), data.image, data.fishingQuantity);
                    item = null;
                }
                else
                {
                    Transform obj = Object.Instantiate<GameObject>(this.item.gameObject, content).transform;
                    FishItem item = new FishItem();
                    item.OnInit(obj, data.image, data.fishingQuantity);
                    item = null;
                }
                count++;
            }
            fishDataList.Remove(fishData);
        }

        private void GetAllComponent()
        {
            icon = m_Transform.Find("BackImg/Boat").GetComponent<RawImage>();
            name = m_Transform.Find("BackImg/Name").GetComponent<Text>();
            descTxt = m_Transform.Find("BackImg/DesText").GetComponent<Text>();
            content = m_Transform.Find("BackImg/FishingData");
            item = m_Transform.Find("BackImg/FishingData/Item");
            sureBtn = m_Transform.Find("BackImg/SureBtn").GetComponent<Button>();
        }

        private void AddAllBtnListener()
        {
            AddButtonClickListener(sureBtn, ConfirmClicked);
        }

        private void ConfirmClicked()
        {

            Debug.Log("FishingOverWindos ConfirmClicked: " + fishDataList.Count);
            if (fishDataList.Count > 0)
            {
                OnUpdateData();
            }
            else
            {
                UIManager.instance.CloseWnd(this);
            }
        }
    }
}
