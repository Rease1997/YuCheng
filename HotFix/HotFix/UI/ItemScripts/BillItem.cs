using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UI;
using UnityEngine;

namespace HotFix
{
    internal class BillItem
    {
        private Transform item;
        private BillData data;
        private Text name;
        private Text num;
        private Text time;

        public void OnInit(Transform itemTrans,BillData data)
        {
            item = itemTrans;
            this.data = data;
            name = item.Find("Name").GetComponent<Text>();
            num = item.Find("DesBack/Num").GetComponent<Text>();
            time = item.Find("Time").GetComponent<Text>();
            item.gameObject.SetActive(true);
            UpdateUI();
        }

        private void UpdateUI()
        {
            name.text = data.bizCategoryNote + " - " + data.remark;
            num.text = data.transAmount;
            float amount = float.Parse(data.transAmount);
            if (amount >= 0)
            {
                num.color = new Color(0.4745098f, 0.8862746f, 0.8627452f);
            }
            else
            {
                num.color = new Color(0.937255f, 0.5254902f, 0.5254902f);
            }
            System.DateTime startTime = System.TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));//获取时间戳
            System.DateTime dt = startTime.AddMilliseconds(double.Parse(data.createDatetimeStr));
            time.text = dt.Year + "-" + dt.Month + "-" + dt.Day + " " + dt.ToString("HH:mm:ss"); // dt.ToString("yyyy/MM/dd HH:mm:ss");//转化为日期时间
        }
    }
}
