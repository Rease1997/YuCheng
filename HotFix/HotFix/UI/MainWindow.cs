using HotFix.Common.Utils;
using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
namespace HotFix
{
    internal class MainWindow:Window
    {

        private TouchScreenCamera touchCamera;
        private Camera mainCamera;
        private Button exitBtn, playerBtn, skyBtn, viewBtn, yuBeiBtn, yuHuoBtn, mapBtn,yuBeiPanelCloseBtn,mingXiBtn,zengSongBtn;
        private Text yuBeiText, yuHuoText;
        private Transform yuBeiPanel;
        private bool isFirst=true;
        private bool bagMove = false;
        private GameObject luckyBag;
        private Transform aimTarget;
        private Vector3 velocity;
        private float smoothTime = 1.2f;//原先为2
        private float speed = 15;
        private Transform[] luckyBagPositionsArray=new Transform[100];
        private GameObject[] luckyBags = new GameObject[6];
        public static Action RefreshUserDatasAction;
        public static Action DonateSuccessAction;//赠送鱼呗成功
        public static Action<int,int> StartPrayAction;
        public override void Awake(object param1 = null, object param2 = null, object param3 = null)
        {
            Debug.Log("MainWindow Awake:" + m_GameObject.name);
            GetAllComponents();
            AddAllButtonListeners();
            AddAction();
            mainCamera = Camera.main;
            touchCamera = mainCamera.GetComponent<TouchScreenCamera>();
            
            

        }
        public override void OnShow(object param1 = null, object param2 = null, object param3 = null)
        {
            yuBeiPanel.gameObject.SetActive(false);
            WebRequestFuncitons.FishingOver();
            RefreshData();
            //WebRequestManager.instance.AsyncLoadUnityWebRequest(WebRequestUtils.recordPageFront, WebRequestFuncitons.FishingOver, true, "{}", RFrameWork.instance.token);
            //WebRequestManager.instance.AsyncLoadUnityWebRequest(WebRequestUtils.fishPondUser, RefreshData, true, "{}", RFrameWork.instance.token);
            if(UserInfoManager.isFirstInGame)
            {
                PrayTreeWebRequestResponse();
                //WebRequestManager.instance.AsyncLoadUnityWebRequest(WebRequestUtils.prayTreeListUrl, PrayTreeWebRequestResponse, true, "{}", RFrameWork.instance.token);
                
            }
            UserInfoManager.isFirstInGame = false;
            //IEnumeratorTool.instance.StartCoroutineNew(RefreshOnlineTime());
        }

       /* private IEnumerator RefreshOnlineTime()
        {
            yield return new WaitForSecondsRealtime(300);
            Debug.Log("刷新在线时长");
            WebRequestManager.instance.AsyncLoadUnityWebRequest(WebRequestUtils.dailyRewardUrl, (string data) => { }, true, "{}", RFrameWork.instance.token);
            WebRequestManager.instance.AsyncLoadUnityWebRequest(WebRequestUtils.fishPondUser, RefreshUserDatasAction, true, "{}", RFrameWork.instance.token);
            IEnumeratorTool.instance.StartCoroutineNew(RefreshOnlineTime());
        }*/
        private void GetAllComponents()
        {

            playerBtn = m_Transform.Find("TopBtns/PlayerBtn").GetComponent<Button>();
            skyBtn = m_Transform.Find("TopBtns/SkyBtn").GetComponent<Button>();
            viewBtn = m_Transform.Find("TopBtns/ViewBtn").GetComponent<Button>();
            exitBtn = m_Transform.Find("TopBtns/ExitBtn").GetComponent<Button>();
            yuBeiBtn = m_Transform.Find("PropertyBtns/YuBeiBtn").GetComponent<Button>();
            yuHuoBtn = m_Transform.Find("PropertyBtns/YuHuoBtn").GetComponent<Button>();
            mapBtn = m_Transform.Find("MapBtn").GetComponent<Button>();
            yuBeiText = m_GameObject.transform.Find("PropertyBtns/YuBeiBtn/Text").GetComponent<Text>();
            yuHuoText = m_GameObject.transform.Find("PropertyBtns/YuHuoBtn/Text").GetComponent<Text>();
            yuBeiPanel = m_Transform.Find("PropertyBtns/YuBeiBtn/Text/DesBack");
            yuBeiPanelCloseBtn = m_Transform.Find("PropertyBtns/YuBeiBtn/Text/DesBack/CloseBtn").GetComponent<Button>();
            mingXiBtn = m_Transform.Find("PropertyBtns/YuBeiBtn/Text/DesBack/MingXiBtn").GetComponent<Button>();
            zengSongBtn = m_Transform.Find("PropertyBtns/YuBeiBtn/Text/DesBack/ZengSongBtn").GetComponent<Button>();
            SetLuckBagOnce();

        }  
        
        private void SetLuckBagOnce()
        {
            Transform parent = GameObject.Find("Scene_dashu_001").transform;
            Transform bagParent = parent.transform.Find("FuDaiRoot");
            for (int i = 0; i < bagParent.childCount; i++)
            {
                luckyBags[i] = bagParent.GetChild(i).gameObject;
            }
            Transform bagPosParent = parent.transform.Find("LcukyBagPositions");
            for (int i = 0; i < bagPosParent.childCount; i++)
            {
                luckyBagPositionsArray[i] = bagPosParent.GetChild(i);
            }
          
        }
        private void AddAllButtonListeners()
        {
            AddButtonClickListener(exitBtn, () =>
            {
                OnExitGame();
            });
            AddButtonClickListener(playerBtn, () =>
            {
                OnShowSelectPlayerPanel();
            });
            AddButtonClickListener(skyBtn, () =>
            {
                OnSwitchSky();
            });
            AddButtonClickListener(viewBtn, () =>
            {
                OnSwitchPersonView();
            });
            AddButtonClickListener(yuBeiBtn, () =>
            {
                OnShowYuBeiPanel(true);
            });
            AddButtonClickListener(yuHuoBtn, () =>
            {
                OnShowYuHuoPanel();
            });
            AddButtonClickListener(mapBtn, () =>
            {
                OnShowMapPanel();
            });
            AddButtonClickListener(yuBeiPanelCloseBtn, () =>
            {
                OnShowYuBeiPanel(false);
            });
            AddButtonClickListener(mingXiBtn, () =>
            {
                JsonData data = new JsonData();
                data["pageNum"] = 1;
                data["pageSize"] = 30;
                string jsonStr = JsonMapper.ToJson(data);
                WebRequestFuncitons.GetMyBillData();
                //WebRequestManager.instance.AsyncLoadUnityWebRequest(WebRequestUtils.jourMyPage, WebRequestFuncitons.GetMyBillData, true, jsonStr, RFrameWork.instance.token);
            });
            AddButtonClickListener(zengSongBtn, () =>
            {
                UIManager.instance.PopUpWnd(FilesName.ADDFISHMONEYPANEL, true, false);
            });


        }
        private void AddAction()
        {
            DonateSuccessAction = DonateSuccess;
            RefreshUserDatasAction = RefreshData;
            StartPrayAction = OnStartPray;
        }
        private void DonateSuccess()
        {
            OnShowYuBeiPanel(false);
            UIManager.instance.HideWnd(FilesName.ADDFISHMONEYPANEL);
            UIManager.instance.HideWnd(FilesName.INCREASEPANEL);
            UIManager.instance.HideWnd(FilesName.PASSWORDINFOPANEL);
        }
        private void OnSwitchPersonView()
        {

            if (isFirst)
            {
                touchCamera.distance = touchCamera.firstPerDis;
                mainCamera.fieldOfView =38;
                mainCamera.cullingMask &= ~(1 << 6);
                 
            }
            else
            {
                touchCamera.distance = 3.7f;
                mainCamera.fieldOfView = 40;
                //mainCamera.cullingMask = -1;
                mainCamera.cullingMask |= (1 << 6);


            }
           // IEnumeratorTool.instance.StartCoroutineNew(CullMaskRemove());
            isFirst = !isFirst;


        }
        private IEnumerator CullMaskRemove()
        {
            Debug.Log("相机不显示13层 CullMaskRemove");
            yield return new WaitForSecondsRealtime(2f);
            mainCamera.cullingMask &= ~(1 << 13);
            Debug.Log("相机不显示13层");
        }
        private void OnShowYuBeiPanel(bool isShow)
        {
            yuBeiPanel.gameObject.SetActive(isShow);
        }
        private void OnShowYuHuoPanel()
        {
            UserInfoManager.refreshBag = false;
            //WebRequestFuncitons.MyFishList();
           // WebRequestManager.instance.AsyncLoadUnityWebRequest(WebRequestUtils.listFront, WebRequestFuncitons.MyFishList, true, "{}", RFrameWork.instance.token);
        }
        private void OnShowMapPanel()
        {
            UIManager.instance.PopUpWnd(FilesName.MAPPANEL,true,false);
        }
        
        private void OnShowSelectPlayerPanel()
        {
            UserInfoManager.selectPlayer.transform.parent = UserInfoManager.playerRoot;
            UserInfoManager.selectPlayer.transform.localScale = Vector3.one*1.4f;
            UserInfoManager.playerScenePos=UserInfoManager.selectPlayer.transform.position;
            UserInfoManager.playerSceneRotation = UserInfoManager.selectPlayer.transform.rotation;
            UIManager.instance.CloseAllWnd();
            UIManager.instance.PopUpWnd(FilesName.SELECTPLAYERPANEL,true,false,true,false);
            OnClosePanel();
        }
        private void OnSwitchSky()
        {
            //TODO切换黑夜白天
        }
      
        private void RefreshData(/*string jsonStr*/)
        {
            string totalCount = "500";
            UserInfoManager.yuBeiNum = (float)Math.Round(float.Parse(totalCount), 2);
            yuBeiText.text = "          " + UserInfoManager.yuBeiNum + "         ";
            yuHuoText.text = "          " + "500" + "KG         ";
            UnityEngine.UI.LayoutRebuilder.ForceRebuildLayoutImmediate(yuBeiBtn.transform.parent.GetComponent<RectTransform>());
        }

        private void  OnExitGame()
        {
            RFrameWork.instance.OpenCommonConfirm("提示", "确认退出渔光之城？", () => { ToolManager.ExitGame(); }, () => { });
        }
        private void OnClosePanel()
        {
            UIManager.instance.CloseWnd(this);
        }
        private void PrayTreeWebRequestResponse()
        {
            //if (jsonData["data"].Count > 0)
            //{
            //    for (int i = 0; i < jsonData["data"].Count; i++)
            //    {
            //        int pos = int.Parse(jsonData["data"][i]["position"].ToString()) - 1;
            //        int typeIndex = int.Parse(jsonData["data"][i]["propType"].ToString());
            //        GameObject bag = GameObject.Instantiate(luckyBags[typeIndex]);
            //        if (luckyBagPositionsArray.Length > pos)
            //        {
            //            if (bag.GetComponent<Animator>())
            //            {
            //                bag.GetComponent<Animator>().enabled = true;
            //            }
            //            SetLuckyBagParent(bag, luckyBagPositionsArray[pos]);

            //        }

            //    }
            //}
            //Debug.Log("PrayTreeWebRequestResponse 祈福列表："+jsonStr);
            //JsonData jsonData = JsonMapper.ToObject(jsonStr);
            //string code = jsonData["code"].ToString();
            //if (code.Equals("200"))
            //{
            //    if(jsonData["data"].Count>0)
            //    {
            //        for (int i = 0; i < jsonData["data"].Count; i++)
            //        {
            //            int pos =int.Parse(jsonData["data"][i]["position"].ToString())-1;
            //            int typeIndex = int.Parse(jsonData["data"][i]["propType"].ToString());
            //            GameObject bag = GameObject.Instantiate(luckyBags[typeIndex]);
            //            if(luckyBagPositionsArray.Length> pos)
            //            {
            //                if(bag.GetComponent<Animator>())
            //                {
            //                    bag.GetComponent<Animator>().enabled = true;
            //                }
            //                SetLuckyBagParent(bag, luckyBagPositionsArray[pos]);
                            
            //            }
                        
            //        }
            //    }
               
            // }
        }

        private void OnStartPray(int bagIndex,int pos)
        {
            
            if (!bagMove)
            {
                bagMove = true;
                luckyBag = GameObject.Instantiate(luckyBags[bagIndex]);
                if (luckyBag.GetComponent<Animator>())
                {
                    luckyBag.GetComponent<Animator>().enabled = false;
                }
               if (luckyBag != null)
                {
                    Debug.Log("克隆的luckyBag的名字：" + luckyBag.gameObject.name);
                    luckyBag.gameObject.SetActive(true);
                    luckyBag.transform.position = UserInfoManager.selectPlayer.prayTarget.transform.position;
                    luckyBag.transform.localScale = Vector3.one * 1;
                    luckyBag.transform.localRotation = Quaternion.identity;                    
                    Debug.Log("luckyBag的目标位置：" + pos);
                    aimTarget = luckyBagPositionsArray[pos];
                }
                else
                {
                    Debug.LogError("克隆的luckyBag咋为空了");
                }

            }
        }
        private void SetLuckyBagParent(GameObject bag,Transform parent)
        {
            bag.gameObject.SetActive(true);
            bag.transform.parent = parent;
            bag.transform.localPosition = Vector3.zero;
            bag.transform.transform.localRotation = Quaternion.identity;
            bag.transform.transform.localScale = Vector3.one * 1;
        }
        public override void OnUpdate()
        {
            
            if(bagMove)
            {
                if (luckyBag != null)
                {

                   luckyBag.transform.position = Vector3.SmoothDamp(luckyBag.transform.position, aimTarget.position, ref velocity, smoothTime, speed);
                    float distance = Vector3.Distance(luckyBag.transform.position, aimTarget.position);
                    if (distance < 0.1f)
                    {
                        SetLuckyBagParent(luckyBag, aimTarget);
                        bagMove = false;
                    }
                }
                
            }
        }
    }
}
