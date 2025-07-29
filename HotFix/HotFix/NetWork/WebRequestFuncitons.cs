using HotFix.Common;
using HotFix.Common.Utils;
using LitJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace HotFix
{
    public class WebRequestFuncitons
    {
        internal static void FishingOver()
        {
            FishingOverData[] dataArr = new FishingOverData[0];
            List<FishingOverData> list = new List<FishingOverData>();
            foreach (var item in dataArr)
            {
                list.Add(item);
            }
            Debug.Log("获取到捕鱼的长度是：" + list.Count);
            if (list.Count > 0)
            {
                UIManager.instance.PopUpWnd(FilesName.FISHINGOVERPANEL, true, false, list);
                /*list.Clear();
                list = null;*/
            }
            //Debug.Log("FishingOver:" + jsonStr);
            //JsonData jsonData = JsonMapper.ToObject(jsonStr);
            //string code = jsonData["code"].ToString();
            //if (code.Equals("200"))
            //{
            //    FishingOverData[] dataArr = JsonMapper.ToObject<FishingOverData[]>(jsonData["data"].ToJson());
            //    List<FishingOverData> list = new List<FishingOverData>();
            //    foreach (var item in dataArr)
            //    {
            //        list.Add(item);
            //    }
            //    Debug.Log("获取到捕鱼的长度是：" + list.Count);
            //    if (list.Count > 0)
            //    {
            //        UIManager.instance.PopUpWnd(FilesName.FISHINGOVERPANEL, true, false, list);
            //        /*list.Clear();
            //        list = null;*/
            //    }

            //}
            //else
            //{
            //    RFrameWork.instance.OpenCommonConfirm("提示", jsonData["errorMsg"].ToString(), () => { }, null);
            //}
        }

        internal static void GetAllBoatData(/*string jsonStr*/)
        {  //  public string id;
           //public string name;
           //public string type; //类型  {0:小渔船,1:钓鱼船,2:远洋舰}
           //public string regionType; // 捕鱼区域 {0:不可捕鱼,1:近海捕鱼,2:远海捕鱼}
           //public string speed;
           //public string enduranceMileage; //续航里程
           //public string getTime; //获得时间
           //public string remainFishingNum; //剩余捕鱼次数
           //public string deadweight; //载重量
           //public string sailNumMonth; //月度出海次数
           //public string status; //状态 {0空闲，1捕鱼中}
            BoatData[] dataArr = new BoatData[1] { new BoatData("101", "Boat", "2", "2","50","500","2025年7月11日","10000","5000","5","0") };
            UserInfoManager.boatDataList.Clear();
            foreach (var item in dataArr)
            {
                Debug.Log("获取船只：" + item.name + "  id:" + item.id + "  tyoe:" + item.type);
                UserInfoManager.boatDataList.Add(item);
            }
            Debug.Log("总船只数量：" + UserInfoManager.boatDataList.Count);
        }

        internal static void GetMyBillData(/*string jsonStr*/)
        {
            BillData[] dataArr = new BillData[0];
            List<BillData> list = new List<BillData>();
            foreach (var item in dataArr)
            {
                list.Add(item);
            }
            Debug.Log("GetMyBillData " + list.Count);
            UIManager.instance.PopUpWnd(FilesName.BILLPANEL, true, false, list);
            //JsonData jsonData = JsonMapper.ToObject(jsonStr);
            //string code = jsonData["code"].ToString();
            //if (code.Equals("200"))
            //{
            //    BillData[] dataArr = JsonMapper.ToObject<BillData[]>(jsonData["data"]["list"].ToJson());
            //    List<BillData> list = new List<BillData>();
            //    foreach (var item in dataArr)
            //    {
            //        list.Add(item);
            //    }
            //    Debug.Log("GetMyBillData " + list.Count);
            //    UIManager.instance.PopUpWnd(FilesName.BILLPANEL, true, false, list);
            //}
            //else
            //{
            //    RFrameWork.instance.OpenCommonConfirm("提示", jsonData["errorMsg"].ToString(), () => { }, null);
            //}
        }

        internal static void GetPrayList(string jsonStr)
        {
            JsonData jsonData = JsonMapper.ToObject(jsonStr);
            string code = jsonData["code"].ToString();
            if (code.Equals("200"))
            {
                PrayData[] arr = JsonMapper.ToObject<PrayData[]>(jsonData["data"].ToJson());
                List<PrayData> list = new List<PrayData>();
                foreach (var item in arr)
                {
                    if (item.status.Equals("1"))
                        list.Add(item);
                }

                UIManager.instance.PopUpWnd(FilesName.PRAYSELECTPANEL, true, false, list);
            }
            else
            {
                RFrameWork.instance.OpenCommonConfirm("提示", jsonData["errorMsg"].ToString(), () => { }, null);
            }
        }

        internal static void MyFishList(string jsonStr)
        {
            FishData[] dataArr = new FishData[0];
            List<List<FishData>> list = new List<List<FishData>>();
            int index = 0;
            int count = 0;
            List<FishData> dataList = new List<FishData>();
            foreach (var item in dataArr)
            {
                index++;
                count++;
                dataList.Add(item);
                if (index >= 4)
                {
                    List<FishData> newlist = new List<FishData>(dataList);
                    list.Add(newlist);
                    dataList.Clear();
                    dataList = new List<FishData>();
                    index = 0;
                }
                if (count == dataArr.Length && dataList.Count > 0)
                {
                    list.Add(dataList);
                }
            }
            if (UserInfoManager.refreshBag)
                UIManager.instance.PopUpWnd(FilesName.BAGPANEL, false, false, list);
            else
                UIManager.instance.PopUpWnd(FilesName.BAGPANEL, true, false, list);
            //JsonData jsonData = JsonMapper.ToObject(jsonStr);
            //string code = jsonData["code"].ToString();
            //if (code.Equals("200"))
            //{
            //    FishData[] dataArr = JsonMapper.ToObject<FishData[]>(jsonData["data"].ToJson());
            //    List<List<FishData>> list = new List<List<FishData>>();
            //    int index = 0;
            //    int count = 0;
            //    List<FishData> dataList = new List<FishData>();
            //    foreach (var item in dataArr)
            //    {
            //        index++;
            //        count++;
            //        dataList.Add(item);
            //        if (index >= 4)
            //        {
            //            List<FishData> newlist = new List<FishData>(dataList);
            //            list.Add(newlist);
            //            dataList.Clear();
            //            dataList = new List<FishData>();
            //            index = 0;
            //        }
            //        if (count == dataArr.Length && dataList.Count > 0)
            //        {
            //            list.Add(dataList);
            //        }
            //    }
            //    if (UserInfoManager.refreshBag)
            //        UIManager.instance.PopUpWnd(FilesName.BAGPANEL, false, false, list);
            //    else
            //        UIManager.instance.PopUpWnd(FilesName.BAGPANEL, true, false, list);
            //}
            //else
            //{
            //    RFrameWork.instance.OpenCommonConfirm("提示", jsonData["errorMsg"].ToString(), () => { }, null);
            //}
        }


    }
}
