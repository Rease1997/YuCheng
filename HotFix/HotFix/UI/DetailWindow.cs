using HotFix.Common.Utils;
using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace HotFix
{
    internal class DetailWindow:Window
    {
        
        //基本组件
        private Button backBtn;
        private Button fishingBtn;
        private Button leftBtn;
        private Button rightBtn;
        private Button boatDataBtn;
        private Button historyBtn;
        private GameObject boatDataImage;
        private GameObject historyImage,blankView;
        private GameObject boat1, boat2, boat3, boat4, boat5;
        //船只属性界面
        private Transform boatDataTrans;
        private Text name,time,monthTimes,times,speed, weight, endurance,boatCountTxt,fishingTxt;      
        //出海记录界面
        private Transform historyTrans;
        private Transform content;
        private Transform item;
        private int totalCount,curIndex=0;
        private Animator curAnimator;
        private BoatData curBoatData;
        private Color grayColor;

        public override void Awake(object param1 = null, object param2 = null, object param3 = null)
        {
            GetAllComponent();
            AddAllBtnListener();
            totalCount = UserInfoManager.boatDataList.Count;
            Transform parent = GameObject.Find("BoatDetailGroup").transform;
            boat1 = parent.GetChild(0).gameObject;
            boat2 = parent.GetChild(1).gameObject;
            boat3 = parent.GetChild(2).gameObject;
            boat4 = parent.GetChild(3).gameObject;
            boat5 = parent.GetChild(4).gameObject;
            grayColor = new Color(1, 1, 1, 0.6f);
        }

        public override void OnShow(object param1 = null, object param2 = null, object param3 = null)
        {
            curBoatData = (BoatData)param1;
            UpdateUI();
            UpdateBoatInfo();
        }

        private void GetAllComponent()
        {
            backBtn = m_Transform.Find("ReturnBtn").GetComponent<Button>();
            fishingBtn = m_Transform.Find("FishingBtn").GetComponent<Button>();
            leftBtn = m_Transform.Find("SelectBack/LeftBtn").GetComponent<Button>();
            rightBtn = m_Transform.Find("SelectBack/RightBtn").GetComponent<Button>();
            boatDataBtn = m_Transform.Find("Back/Btns/BoatDataBtn").GetComponent<Button>();
            historyBtn = m_Transform.Find("Back/Btns/HistoryBtn").GetComponent<Button>();
            boatDataImage = m_Transform.Find("Back/Btns/BoatDataImage").gameObject;
            historyImage = m_Transform.Find("Back/Btns/HistoryImage").gameObject;
            boatDataTrans = m_Transform.Find("Back/BoatDataBack");
            name = m_Transform.Find("Back/BoatDataBack/Name").GetComponent<Text>();
            time = m_Transform.Find("Back/BoatDataBack/Time").GetComponent<Text>();
            fishingTxt = m_Transform.Find("FishingBtn/Text").GetComponent<Text>();          
            monthTimes = m_Transform.Find("Back/BoatDataBack/DataBack/DesNum1").GetComponent<Text>();
            times = m_Transform.Find("Back/BoatDataBack/DataBack/DesNum2").GetComponent<Text>();
            speed = m_Transform.Find("Back/BoatDataBack/DataBack/DetailDatas/Speed/DesText").GetComponent<Text>();
            weight = m_Transform.Find("Back/BoatDataBack/DataBack/DetailDatas/Weight/DesText").GetComponent<Text>();
            endurance = m_Transform.Find("Back/BoatDataBack/DataBack/DetailDatas/Endurance/DesText").GetComponent<Text>();
            boatCountTxt = m_Transform.Find("SelectBack/Text").GetComponent<Text>();

            historyTrans = m_Transform.Find("Back/HistoryBack");
            content = m_Transform.Find("Back/HistoryBack/Scroll View/Viewport/Content");
            item = m_Transform.Find("Back/HistoryBack/Scroll View/Viewport/Content/Item");
            blankView = m_Transform.Find("Back/HistoryBack/BlankView").gameObject; 
        }

        private void AddAllBtnListener()
        {
            AddButtonClickListener(backBtn, OnClosePanel);
            AddButtonClickListener(fishingBtn,StartCatchFish);
            AddButtonClickListener(leftBtn,LeftFunc);
            AddButtonClickListener(rightBtn,RightFunc);
            AddButtonClickListener(boatDataBtn, () => { ShowBoatProperty(true); });
            AddButtonClickListener(historyBtn,HistoryFunc);
        }

        private void UpdateUI()
        {
            name.text = curBoatData.name;
            TextExtensionUtils.SetTextWithEllipsis(name, name.text, 20);
            System.DateTime startTime = System.TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));//获取时间戳
            System.DateTime dt = startTime.AddMilliseconds(double.Parse(curBoatData.getTime));
            time.text = "获得日期"+ dt.Year + "-" + dt.Month + "-" + dt.Day + " " + dt.ToString("HH:mm:ss"); // dt.ToString("yyyy/MM/dd HH:mm:ss");//转化为日期时间
            monthTimes.text = curBoatData.sailNumMonth + "次/月";
            times.text = curBoatData.remainFishingNum + "次";
            speed.text = curBoatData.speed + "km/h";
            weight.text = curBoatData.deadweight+"吨";
            endurance.text = curBoatData.enduranceMileage + "km";
            bool canFish = curBoatData.status.Equals("0");
            if(canFish)
            {
                fishingTxt.text = "去捕鱼";
                fishingBtn.enabled = true;
                fishingBtn.GetComponent<Image>().color = Color.white;
            }
            else
            {
                fishingTxt.text = "捕鱼中";
                fishingBtn.enabled = false;
                fishingBtn.GetComponent<Image>().color = grayColor;
            }

            ShowBoatProperty(true);
        }
        
        private void OnClosePanel()
        {
            UIManager.instance.CloseWnd(this);
            UIManager.instance.PopUpWnd(FilesName.MAINPANEL, true, false);
            UIManager.instance.PopUpWnd(FilesName.JOYSTICKPANEL, true, false, true, false);
            Camera.main.GetComponent<TouchScreenCamera>().enabled = true;
            HideAllBoat();
        }
        private void HideAllBoat()
        {
            boat1.gameObject.SetActive(false);
            boat2.gameObject.SetActive(false);
            boat3.gameObject.SetActive(false);
            boat4.gameObject.SetActive(false);
            boat5.gameObject.SetActive(false);
        }
        private void StartCatchFish()
        {
            Debug.Log("StartCatchFish 开始去捕鱼了");
            JsonData data = new JsonData();
             data["userBoatId"] = curBoatData.id;
            CatchFishWebRequestResponse();
            //WebRequestManager.instance.AsyncLoadUnityWebRequest(WebRequestUtils.startCatchFishUrl, CatchFishWebRequestResponse, true, JsonMapper.ToJson(data), RFrameWork.instance.token);

        }
        private IEnumerator StartCatchFishAnim()
        {
            curAnimator.SetTrigger("Move");
            UIManager.instance.CloseWnd(this);            
            yield return new WaitForSecondsRealtime(5f);
            Camera.main.GetComponent<TouchScreenCamera>().enabled = true;
            UIManager.instance.PopUpWnd(FilesName.JOYSTICKPANEL, true, false, true, false);
            UIManager.instance.PopUpWnd(FilesName.MAINPANEL, true, false);
                    
            Debug.Log("StartCatchFish 开始捕鱼成功");
        }
        private void CatchFishWebRequestResponse(/*string jsonStr*/)
        {

            IEnumeratorTool.instance.StartCoroutineNew(StartCatchFishAnim());
            //JsonData jsonData = JsonMapper.ToObject(jsonStr);
            //string code = jsonData["code"].ToString();
            //if (code.Equals("200"))
            //{
            //    IEnumeratorTool.instance.StartCoroutineNew(StartCatchFishAnim());
                                
            //}else
            //{
            //    string errorMsg= jsonData["errorMsg"].ToString();
            //    RFrameWork.instance.OpenCommonConfirm("捕鱼失败", errorMsg, () => { }, null);
            //}
        }
       
        private void LeftFunc()
        {
            curIndex--;
            if(curIndex<0)
            {
                curIndex = totalCount-1;
            }
            UpdateBoatInfo();
            UpdateUI();
            
        }

        private void RightFunc()
        {
           curIndex++;
           if(curIndex>=totalCount)
            {
                curIndex=0;
            }
            UpdateBoatInfo();
            UpdateUI();
            
        }
        private void UpdateBoatInfo()
        {
            boatCountTxt.text = (curIndex+1)+"/"+totalCount;
            curBoatData = UserInfoManager.boatDataList[curIndex];
            boat1.SetActive(curBoatData.type.Equals("0"));
            boat2.SetActive(curBoatData.type.Equals("1"));
            boat3.SetActive(curBoatData.type.Equals("2"));
            boat4.SetActive(curBoatData.type.Equals("3"));
            boat5.SetActive(curBoatData.type.Equals("4"));
            if (curBoatData.type.Equals("0"))
            {
                curAnimator= boat1.GetComponent<Animator>();
            }
            else if (curBoatData.type.Equals("1"))
            {
                curAnimator = boat2.GetComponent<Animator>();
            }
            else if (curBoatData.type.Equals("2"))
            {
                curAnimator = boat3.GetComponent<Animator>();
            }
            else if (curBoatData.type.Equals("3"))
            {
                curAnimator = boat4.GetComponent<Animator>();
            }
            else if (curBoatData.type.Equals("4"))
            {
                curAnimator = boat5.GetComponent<Animator>();
            }
            if (curAnimator)
            {
                curAnimator.Play("Idle");
            }

        }
        private void ShowBoatProperty(bool isShow)
        {
            boatDataTrans.gameObject.SetActive(isShow);
            historyTrans.gameObject.SetActive(!isShow);
            boatDataImage.SetActive(isShow);
            historyImage.SetActive(!isShow);

        }

        private void HistoryFunc()
        {
            blankView.SetActive(false);
            JsonData data = new JsonData();
            data["pageNum"] = 1;
            data["pageSize"] = 30;
            data["userBoatId"] = curBoatData.id;
            string jsonStr = JsonMapper.ToJson(data);
            ShowBoatHistory();
            //WebRequestManager.instance.AsyncLoadUnityWebRequest(WebRequestUtils.pageFront, ShowBoatHistory, true, jsonStr, RFrameWork.instance.token);
        }

        //展示船只历史记录
        private void ShowBoatHistory(/*string jsonStr*/)
        {
            FishingData[] arr =new FishingData[0];
            List<FishingData> list = new List<FishingData>();
            foreach (var item in arr)
            {
                list.Add(item);
            }
            ShowBoatProperty(false);
            RefreshHistoryItems(list);
            //JsonData jsonData = JsonMapper.ToObject(jsonStr);
            //string code = jsonData["code"].ToString();
            //if (code.Equals("200"))
            //{
            //    FishingData[] arr = JsonMapper.ToObject<FishingData[]>(jsonData["data"]["list"].ToJson());
            //    List<FishingData> list = new List<FishingData>();
            //    foreach (var item in arr)
            //    {
            //        list.Add(item);
            //    }
            //    ShowBoatProperty(false);               
            //    RefreshHistoryItems(list);
            //}
            //else
            //{
            //    RFrameWork.instance.OpenCommonConfirm("提示", jsonData["errorMsg"].ToString(), () => { }, null);
            //}
        }

        private void RefreshHistoryItems(List<FishingData> list)
        {
            for (int i = 0; i < content.childCount; i++)
            {
                content.GetChild(i).gameObject.SetActive(false);
            }
            int count = 0;
            foreach (var item in list)
            {
                if (count + 1 <= content.childCount)
                {
                    DetailHistoryItem detailItem = new DetailHistoryItem();
                    detailItem.OnInit(content.GetChild(count), item);
                    detailItem = null;
                }
                else
                {
                    Transform obj = Object.Instantiate<GameObject>(this.item.gameObject, content).transform;
                    DetailHistoryItem detailItem = new DetailHistoryItem();
                    detailItem.OnInit(obj, item);
                    detailItem = null;
                }
                count++;
            }
            if(list.Count > 0)
            {
                blankView.SetActive(false);
            }
            else
            {
                
                blankView.SetActive(true);
            }
        }
    }
}
