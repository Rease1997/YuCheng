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
    internal class PrayTipsWindow:Window
    {
        public Button prayBtn;
        public Transform content;
        public RectTransform contentRect;
        public Transform item;
        private List<PrayInfoData> prayDataList = new List<PrayInfoData>();
        private bool canShow;
        public static Action PraySuccessAction;
        public override void Awake(object param1 = null, object param2 = null, object param3 = null)
        {

            GetAllComponent();
            AddButtonClickListener(prayBtn, StartPray);
            canShow = true;
            
        }

        public override void OnShow(object param1 = null, object param2 = null, object param3 = null)
        {
            prayBtn.gameObject.SetActive(true);
            PraySuccessAction = OnPrayListRequest;
            OnPrayListRequest();

        }
        private void OnPrayListRequest()
        {
            //WebRequestManager.instance.AsyncLoadUnityWebRequest(WebRequestUtils.prayScrollUrl, PrayLampWebRequestResponse, true, "{}", RFrameWork.instance.token);

        }
        private void PrayLampWebRequestResponse(string jsonStr)
        {
            Debug.Log("祈福列表刷新："+jsonStr);
            JsonData jsonData = JsonMapper.ToObject(jsonStr);
            string code = jsonData["code"].ToString();
            if (code.Equals("200"))
            {
                
                PrayInfoData[] prayArray = JsonMapper.ToObject<PrayInfoData[]>(jsonData["data"].ToJson());
                if(prayArray != null&&prayArray.Length>0)
                {
                    content.transform.parent.parent.gameObject.SetActive(true);
                    prayDataList.Clear();
                    foreach (var item in prayArray)
                    {
                        prayDataList.Add(item);
                    }
                    UpdateUI();
                }
                else
                {
                    content.transform.parent.parent.gameObject.SetActive(false);
                }
                

               
            }
            else
            {
                RFrameWork.instance.OpenCommonConfirm("提示", jsonData["errorMsg"].ToString(), () => { }, null);
            }
        }
        private void GetAllComponent()
        {
            prayBtn = m_Transform.Find("SureBtn").GetComponent<Button>();
            content = m_Transform.Find("Back/ScrollBar/Content");
            contentRect = content.GetComponent<RectTransform>();
            item = m_Transform.Find("Back/ScrollBar/Content/Text");
        }

        private void StartPray()
        {
            //prayBtn.gameObject.SetActive(false);
            //WebRequestManager.instance.AsyncLoadUnityWebRequest(WebRequestUtils.propPageFront, WebRequestFuncitons.GetPrayList, true, "{}", RFrameWork.instance.token);
        }
        private void OnPraySuccess()
        {

        }
        private void UpdateUI()
        {
            for (int i = 0; i < content.childCount; i++)
            {
                content.GetChild(i).gameObject.SetActive(false);
            }
            for(int i=0;i<prayDataList.Count;i++)
            {
                if(content.childCount>i)
                {
                    PrayInfoItem item = new PrayInfoItem();
                    item.OnInit(content.GetChild(i), prayDataList[i]);
                     
                }
                else
                {
                    Transform obj = Object.Instantiate<GameObject>(this.item.gameObject, content).transform;
                    PrayInfoItem item = new PrayInfoItem();
                    item.OnInit(obj, prayDataList[i]);
                     
                }
            }
           
        }
        public override void OnLateUpdate()
        {
            if(canShow)
            {
                contentRect.transform.Translate(-0.7f * Time.deltaTime, 0, 0);
                if (contentRect.anchoredPosition.x <= -contentRect.rect.width)
                {
                    contentRect.anchoredPosition = new Vector2(450, contentRect.anchoredPosition.y);
                }
            }
           
        }
        public override void OnClose()
        {
            this.canShow = false;
        }

    }
}
