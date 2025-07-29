using HotFix.Common;
using HotFix.Common.Utils;
using LitJson;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace HotFix
{
    public class PurchaseManager
    {
        public static void SetStateOfPayment(string info)
        {
            JsonData jsonData = JsonMapper.ToObject(info);
            string result = (string)jsonData["result"];
            if (result.Equals("succeed"))
            {

            }
            else
            {
                 
                RFrameWork.instance.OpenCommonConfirm("提示", "支付失败:"+ result, () => { }, null);
            }
        }

        public static void CallWeChatPay(string appId, string merchantId, string prepayId, string wechatPackage, string nonceStr, string timeStamp, string sign)
        {
        Debug.Log("AndroidManager CallWeChatPay:" + appId + "  " + merchantId + "  " + prepayId); 
        RFrameWork.instance.CallWeChatPay(appId, merchantId, prepayId, wechatPackage, nonceStr, timeStamp, sign);

        }
        public static void CallALiPay(string signOrder)
        {
            Debug.Log("AndroidManager CallALiPay:" + signOrder);
            RFrameWork.instance.CallALiPay(signOrder);

        }
        public static void CallH5Pay(string url)
        {
            Debug.Log("AndroidManager CallH5Pay:" + url);
            RFrameWork.instance.CallH5Pay(url);

        }
    }
}
