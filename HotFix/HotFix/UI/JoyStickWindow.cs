using UnityEngine.UI;
using LitJson;
using UnityEngine;
using System;
using System.Collections.Generic;
using HotFix.Common.Utils;
using StarterAssets;

namespace HotFix
{

    class JoyStickWindow : Window
    {
        private TriggerEvent triggerEvent1, triggerEvent2, triggerEvent3, triggerEvent4, triggerEventLight, triggerEventPray, triggerEnterOldTown, triggerEnterEastIsLand;
        private Transform telepoteAim1, telepoteAim2, telepoteAim3, telepoteAim4, telepoteLightAnim;
        private bool cansShow, isFirst;
        private TouchScreenCamera touchCamera;
        private Button uploadBtn, removeBtn;
        public static Action<GoodsItem> ExhibitionAction;
        private GoodsItem curExhibition;       
        public static Action<string,string,string,string> ShowExhibition3DAction;

        public override void Awake(object param1 = null, object param2 = null, object param3 = null)
        {
            Debug.Log("JoyStickWindow Awake:" + m_GameObject.name);
            GetAllComponents();
            AddAllButtonListeners();
            cansShow = (bool)param1;
            if (param2 != null)
            {
                isFirst = (bool)param2;
            }
            touchCamera = Camera.main.GetComponent<TouchScreenCamera>();
            m_GameObject.SetActive(cansShow);
            if (cansShow)
            {
                if (isFirst)
                {
                    OnEnterGame();

                }
                OnSetTrigger();
            
            }
           
            ExhibitionAction = OnTirggerExhibition3D;
            ShowExhibition3DAction = ShowExhibition3D;
        }

        private void OnSetTrigger()
        {
            Transform teleport = GameObject.Find("TeleportSpot").transform;
            telepoteAim1 = teleport.Find("Teleport1/Bridge_Spot").transform;
            telepoteAim2 = teleport.Find("Teleport2/Bridge_Spot").transform;
            telepoteAim3 = teleport.Find("Teleport3/Bridge_Spot").transform;
            telepoteAim4 = teleport.Find("Teleport4/Bridge_Spot").transform;
            telepoteLightAnim = GameObject.Find("TeleportGuangjing").transform;
            Transform eastLand = GameObject.Find("Teleport_LightArea").transform;
            triggerEvent1 = teleport.Find("Teleport1/Bridge").GetComponent<TriggerEvent>();
            triggerEvent2 = teleport.Find("Teleport2/Bridge").GetComponent<TriggerEvent>();
            triggerEvent3 = teleport.Find("Teleport3/Bridge").GetComponent<TriggerEvent>();
            triggerEvent4 = teleport.Find("Teleport4/Bridge").GetComponent<TriggerEvent>();
            triggerEventLight = eastLand.GetComponent<TriggerEvent>();
            triggerEventPray = GameObject.Find("PrayStartTrigger").GetComponent<TriggerEvent>();
            triggerEnterOldTown = GameObject.Find("Trigger_MainLand").GetComponent<TriggerEvent>();
            triggerEnterEastIsLand = GameObject.Find("Trigger_EastGateIsland").GetComponent<TriggerEvent>();
            triggerEvent1.TriggerEnter = TelepoteTriggerEvent1;
            triggerEvent2.TriggerEnter = TelepoteTriggerEvent2;
            triggerEvent3.TriggerEnter = TelepoteTriggerEvent3;
            triggerEvent4.TriggerEnter = TelepoteTriggerEvent4;
            triggerEventLight.TriggerEnter = TelepoteTriggerEventLight;
            triggerEventPray.TriggerEnter = StartPrayTriggerEnter;
            triggerEventPray.TriggerExit = StartPrayTriggerExit;
            triggerEnterOldTown.TriggerEnter = OnTriggerEnterOldTown;
            triggerEnterEastIsLand.TriggerEnter = OnTriggerEnterIsLand;

        }

        public override void OnShow(object param1 = null, object param2 = null, object param3 = null)
        {
            m_GameObject.transform.SetAsFirstSibling();
            uploadBtn.gameObject.SetActive(false);
            removeBtn.gameObject.SetActive(false);


        }
        /// <summary>
        /// 第一次进入游戏进入东门岛
        /// </summary>
        private void OnEnterGame()
        {
            UserInfoManager.fishAnimator = GameObject.Find("C3_Guangjing").GetComponent<Animator>();
            UserInfoManager.selectPlayer.transform.position = UserInfoManager.playerScenePos;
            UserInfoManager.selectPlayer.transform.rotation = UserInfoManager.playerSceneRotation;
            UserInfoManager.selectPlayer.enabled = true;
            UserInfoManager.selectPlayer.transform.GetComponent<CharacterController>().enabled = true;
            UserInfoManager.selectPlayer.gameObject.SetActive(true);
            UserInfoManager.selectPlayer.transform.localScale = Vector3.one*0.8f;
            UserInfoManager.selectPlayer.transform.parent = null;
            touchCamera.x += 50;
            touchCamera.enabled = true;
            touchCamera.distance = 3.7f;
            RFrameWork.instance.SetBackAudio("SingleSounds/island");
            OnFirstEnterGameShowExhibition();
           /* JsonData data2 = new JsonData();
            data2["isExit"] = "0";
            string jsonStr = JsonMapper.ToJson(data2);
            WebRequestManager.instance.AsyncLoadUnityWebRequest(WebRequestUtils.dailyRewardUrl, (string str) => { }, true, jsonStr, RFrameWork.instance.token);
*/


        }
        private void OnFirstEnterGameShowExhibition()
        {
            GameObject gallery = GameObject.Find("GalleryNew");
            Transform parent = gallery.transform.Find("Collection3D");
            Texture blackTexture=parent.transform.GetChild(0).GetComponent<MeshRenderer>().material.mainTexture;
            for (int i = 0; i < parent.childCount; i++)
            {
                GoodsItem item = new GoodsItem();
                item.OnInit(parent.GetChild(i), uploadBtn.transform.parent, i + 1,GoodsType.Goods3D, blackTexture);
                UserInfoManager.goodsItemList.Add(item);
            }
            Transform parent2d = gallery.transform.Find("Collection2D");
            for (int i = 0; i < 6; i++)
            {
                GoodsItem item = new GoodsItem();
                item.OnInit(parent2d.GetChild(i), uploadBtn.transform.parent, i + 1, GoodsType.Goods2D, blackTexture);
                UserInfoManager.goods2DItemList.Add(item);
            }
            //WebRequestManager.instance.AsyncLoadUnityWebRequest(WebRequestUtils.firstEnterGoodsUrl, GetFirstGoodsWebRequestResponse, true, "{}", RFrameWork.instance.token);
            GetFirstGoodsWebRequestResponse();


        }
        private void GetAllComponents()
        {
            uploadBtn = this.m_GameObject.transform.Find("BottomBtns/UploadGoods").GetComponent<Button>();
            removeBtn = this.m_GameObject.transform.Find("BottomBtns/UnloadGoods").GetComponent<Button>();
        }
        private void AddAllButtonListeners()
        {
            AddButtonClickListener(uploadBtn, OnShowGoodsPanel);
            AddButtonClickListener(removeBtn, RemoveExhibition3D);

        }
        private void TelepoteTriggerEvent1(Collider col)
        {
            if (col.CompareTag("Player") == true)
            {

                IEnumeratorTool.instance.StartCoroutineNew(ToolManager.WaitSecondsToTranslate(telepoteAim2));

            }
        }
        private void TelepoteTriggerEvent2(Collider col)
        {
            if (col.CompareTag("Player") == true)
            {

                IEnumeratorTool.instance.StartCoroutineNew(ToolManager.WaitSecondsToTranslate(telepoteAim1));

            }
        }

        private void TelepoteTriggerEvent3(Collider col)
        {
            if (col.CompareTag("Player") == true)
            {
                IEnumeratorTool.instance.StartCoroutineNew(ToolManager.WaitSecondsToTranslate(telepoteAim4));


            }
        }

        private void TelepoteTriggerEvent4(Collider col)
        {
            if (col.CompareTag("Player") == true)
            {
                IEnumeratorTool.instance.StartCoroutineNew(ToolManager.WaitSecondsToTranslate(telepoteAim3));


            }
        }
        private void TelepoteTriggerEventLight(Collider col)
        {
            if (col.CompareTag("Player") == true)
            {
                UserInfoManager.fishAnimator.speed = 0;
                IEnumeratorTool.instance.StartCoroutineNew(ToolManager.WaitSecondsToTranslate(telepoteLightAnim, null, TranslateType.Lighting));



            }
        }
        private void StartPrayTriggerEnter(Collider col)
        {

            UIManager.instance.PopUpWnd(FilesName.PRAYTIPSPANEL, true, false);
        }
        private void OnTriggerEnterOldTown(Collider col)
        {
            if (col.CompareTag("Player") == true)
            {
                if (!ToolManager.GetAudioClipName().Equals("oldTown"))
                {
                    RFrameWork.instance.SetBackAudio("SingleSounds/oldTown");
                }
            }
        }
        private void OnTriggerEnterIsLand(Collider col)
        {
            if (col.CompareTag("Player") == true)
            {
                if (!ToolManager.GetAudioClipName().Equals("island"))
                {
                    RFrameWork.instance.SetBackAudio("SingleSounds/island");
                }
            }
        }
        private void StartPrayTriggerExit(Collider col)
        {

            UIManager.instance.CloseWnd(FilesName.PRAYTIPSPANEL);
        }
        private void OnTirggerExhibition3D(GoodsItem trans)
        {
            curExhibition = trans;
        }
        private void RemoveExhibition3D()
        {
            if (curExhibition != null)
            {
                Debug.Log("要移除的藏品id："+ curExhibition.id+"  name:"+ curExhibition.Name);
                JsonData data = new JsonData();
                data["collectionDetailId"] = curExhibition.id;
                Debug.Log("藏品移除成功：" + curExhibition.id);
                removeBtn.gameObject.SetActive(false);
                curExhibition.RemoveGoods();
                //WebRequestManager.instance.AsyncLoadUnityWebRequest(WebRequestUtils.removeExhibitionUrl, (string jsonStr) => {
                //    JsonData jsonData = JsonMapper.ToObject(jsonStr);
                //    string code = jsonData["code"].ToString();
                //    if (code.Equals("200"))
                //    {
                //        Debug.Log("藏品移除成功：" + curExhibition.id);
                //       removeBtn.gameObject.SetActive(false);
                //       curExhibition.RemoveGoods();

                //    }
                //    else
                //    {
                //        RFrameWork.instance.OpenCommonConfirm("提示", jsonData["errorMsg"].ToString(), () => { }, () => { });
                //    }
                //}, true, JsonMapper.ToJson(data), RFrameWork.instance.token);
                
            }
        }
        private void ShowExhibition3D(string type,string id,string collectId,string  fileUrl)
        {           
            if (curExhibition != null)
            {
                JsonData data = new JsonData();
                data["collectionDetailId"] = id;
                data["position"] = curExhibition.pos;
                Debug.Log("藏品添加成功：" + id + "  类型" + type);
                curExhibition.ShowGoods(type, id, collectId, fileUrl);
                UIManager.instance.CloseWnd(FilesName.GOODSELECTSPANEL);
                uploadBtn.gameObject.SetActive(false);
                //WebRequestManager.instance.AsyncLoadUnityWebRequest(WebRequestUtils.addExhibitionUrl, (string jsonStr) => {
                //    JsonData jsonData = JsonMapper.ToObject(jsonStr);
                //    string code = jsonData["code"].ToString();
                //    if (code.Equals("200"))
                //    {
                //        Debug.Log("藏品添加成功：" + id + "  类型"+ type);
                //        curExhibition.ShowGoods(type, id, collectId,fileUrl);
                //        UIManager.instance.CloseWnd(FilesName.GOODSELECTSPANEL);
                //        uploadBtn.gameObject.SetActive(false);

                //    }
                //    else
                //    {
                //        RFrameWork.instance.OpenCommonConfirm("提示", jsonData["errorMsg"].ToString(), () => { }, () => { });
                //    }
                //}, true, JsonMapper.ToJson(data), RFrameWork.instance.token);

            }
        }
        

        private void GetFirstGoodsWebRequestResponse()
        {
            GoodsData[] dataArr = new GoodsData[0];
            for (int i = 0; i < dataArr.Length; i++)
            {
                GoodsData goodsData = dataArr[i];
                int index = int.Parse(goodsData.position) - 1;
                if (goodsData.u3dFlag.Equals("1"))
                {
                    GoodsItem item = UserInfoManager.goodsItemList[index];
                    string type = (int.Parse(goodsData.boatType) + 1).ToString();
                    item.ShowGoods(type, goodsData.collectionDetailId, goodsData.collectionId);

                }
                else
                {
                    GoodsItem item = UserInfoManager.goods2DItemList[index];
                    item.ShowGoods("-1", goodsData.collectionDetailId, goodsData.collectionId, goodsData.coverFileUrl);


                }

            }
            //Debug.Log("GetFirstGoodsWebRequestResponse:"+jsonStr);
            //JsonData jsonData = JsonMapper.ToObject(jsonStr);
            //string code = jsonData["code"].ToString();
            //if (code.Equals("200"))
            //{
            //    GoodsData[] dataArr = JsonMapper.ToObject<GoodsData[]>(jsonData["data"].ToJson());
            //    for(int i=0;i<dataArr.Length;i++)
            //    {
            //        GoodsData goodsData = dataArr[i];
            //        int index = int.Parse(goodsData.position) - 1;
            //        if (goodsData.u3dFlag.Equals("1"))
            //        {
            //            GoodsItem item = UserInfoManager.goodsItemList[index];
            //            string type = (int.Parse(goodsData.boatType) + 1).ToString();
            //            item.ShowGoods(type, goodsData.collectionDetailId, goodsData.collectionId);

            //        }
            //        else
            //        {
            //            GoodsItem item = UserInfoManager.goods2DItemList[index];
            //            item.ShowGoods("-1", goodsData.collectionDetailId, goodsData.collectionId, goodsData.coverFileUrl);
                         

            //        }

            //    }


            //}
            //else
            //{
            //    RFrameWork.instance.OpenCommonConfirm("提示", jsonData["errorMsg"].ToString(), () => { }, () => { });
            //}

        }
    
        private void OnShowGoodsPanel()
        {
            JsonData data = new JsonData();
            int flag = (int)curExhibition.goodsType;
            
            data["u3dFlag"] = flag.ToString();
            data["pageNum"] = 0;
            data["pageSize"] = 2000;
            string sendJson = JsonMapper.ToJson(data);
            GoodsSelectData[] dataArr = new GoodsSelectData[0];
            Debug.Log("个人藏品的长度：" + dataArr.Length);
            UIManager.instance.PopUpWnd(FilesName.GOODSELECTSPANEL, true, false, dataArr, curExhibition.goodsType);
            //WebRequestManager.instance.AsyncLoadUnityWebRequest(WebRequestUtils.exhibitionListUrl, (string jsonStr) => {
            //    JsonData jsonData = JsonMapper.ToObject(jsonStr);
            //    string code = jsonData["code"].ToString();
            //    Debug.Log("个人藏品："+jsonStr);
            //    if (code.Equals("200"))
            //    {
            //        GoodsSelectData[] dataArr = JsonMapper.ToObject<GoodsSelectData[]>(jsonData["data"]["list"].ToJson());
            //        Debug.Log("个人藏品的长度："+dataArr.Length);
            //        UIManager.instance.PopUpWnd(FilesName.GOODSELECTSPANEL, true, false, dataArr,curExhibition.goodsType);

            //    }
            //    else
            //    {
            //        RFrameWork.instance.OpenCommonConfirm("提示", jsonData["errorMsg"].ToString(), () => { }, () => { });
            //    }
            //}, true, sendJson, RFrameWork.instance.token);
        }

    }
}



