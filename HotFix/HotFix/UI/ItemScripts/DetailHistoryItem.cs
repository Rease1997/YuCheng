using HotFix.Common.Utils;
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
    internal class DetailHistoryItem
    {
        private Transform item;
        private Text time,backTimeTxt;
        private Transform content;
        private Transform itemTrans;

        public void OnInit(Transform itemTrans, FishingData data)
        {
            item = itemTrans;
            time = item.Find("DesBack/TimeText").GetComponent<Text>();
            backTimeTxt = item.Find("DesBack/BackTimeText").GetComponent<Text>();
            content = item.Find("FishingData");
            this.itemTrans = item.Find("FishingData/Item");
            item.gameObject.SetActive(true);
            UpdateUI(data);
        }

        private void UpdateUI(FishingData data)
        {

            System.DateTime startTime = System.TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));//获取时间戳
            System.DateTime dt = startTime.AddMilliseconds(double.Parse(data.sailDatetime));
            time.text ="出海时间："+ TimeUtils.MilSecondsTimestampToTime(data.sailDatetime)+"至"+ TimeUtils.MilSecondsTimestampToTime(data.backDatetime); // dt.ToString("yyyy/MM/dd HH:mm:ss");//转化为日期时间
            if(data.recordDetailList.Count<=0)
            {
                backTimeTxt.gameObject.SetActive(true);
                if (data.status.Equals("2")|| data.status.Equals("3"))
                {
                    backTimeTxt.text = "很抱歉，您本次出船没有捕到任何鱼货。" ;

                }
                else
                {
                    backTimeTxt.text = "您的船只正在捕鱼中，预计" + TimeUtils.MilSecondsTimestampToTime(data.backDatetime) + "返航";
                    
                }

            }
            else
            {
                backTimeTxt.gameObject.SetActive(false);
            }
            RefreshItem(data);
            UnityEngine.UI.LayoutRebuilder.ForceRebuildLayoutImmediate(content.GetComponent<RectTransform>());
        }

        private void RefreshItem(FishingData data)
        {
            for (int i = 0; i < content.childCount; i++)
            {
                content.GetChild(i).gameObject.SetActive(false);
            }
            int count = 0;
            foreach (var itemData in data.recordDetailList)
            {
                if (count + 1 <= content.childCount)
                {
                    FishItem item = new FishItem();
                    item.OnInit(content.GetChild(count), itemData.image, itemData.fishingQuantity);
                    item = null;
                }
                else
                {
                    Transform obj = Object.Instantiate<GameObject>(this.itemTrans.gameObject, content).transform;
                    FishItem item = new FishItem();
                    item.OnInit(obj, itemData.image, itemData.fishingQuantity);
                    item = null;
                }
                count++;
            }
        }
    }
}
