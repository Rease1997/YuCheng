using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UI;
using UnityEngine;

namespace HotFix
{
    internal class PrayInfoItem
    {
        private Transform item;
        public Text text;
        

        public void OnInit(Transform itemTrans, PrayInfoData data)
        {
            item = itemTrans;
            text = item.GetComponent<Text>();
            item.gameObject.SetActive(true);
            UpdateUI(data);
        }

        private void UpdateUI(PrayInfoData data)
        {
            char[] arr = data.nickName.ToCharArray();
            text.text = "「" + arr[0] + "**" + arr[arr.Length - 1] + "」在光境祈福—「" + data.propName + "」";
        }
    }
}
