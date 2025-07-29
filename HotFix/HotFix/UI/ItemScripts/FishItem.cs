using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace HotFix
{
    public class FishItem
    {
        private Transform item;
        public Text weight;
        public RawImage icon;

        public void OnInit(Transform itemTrans, string photoUrl,string weightStr)
        {
            item = itemTrans;
            icon = item.Find("Text/Bg/Image").GetComponent<RawImage>();
            weight = item.Find("Text").GetComponent<Text>();
            item.gameObject.SetActive(true);
            UpdateUI(photoUrl,weightStr);
        }

        private void UpdateUI(string url,string weightStr)
        {
            icon.texture = null;
            //WebRequestManager.instance.AsyncLoadUnityTexture(url, (tx) =>
            //{if (icon.texture != null)
            //    {
            //        ToolManager.DestoryDownLoadTexture(icon.texture);
            //    }
            //    icon.texture = tx;
            //});
            weight.text = "         "+ weightStr+"KG  ";
        }
    }
}
