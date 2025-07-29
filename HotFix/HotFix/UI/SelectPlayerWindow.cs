using UnityEngine.UI;
using LitJson;
using UnityEngine;
using System;
using System.Collections.Generic;
using HotFix.Common.Utils;
using StarterAssets;

namespace HotFix
{
    public enum SelectPlayerType
    {
        Islander,
        Fisherman,
        Trader

    }
    class SelectPlayerWindow : Window
    {
        private UICanvasControllerInput joyCanvas;
        private TouchScreenCamera touchCamera;
        private Button backBtn, preBtn, nextBtn, enterBtn, islanderBtn, fisherBtn, traderBtn;
        private GameObject islanderBtnSelect, fisherBtnSelect, traderBtnSelect;
        private Text enterText;
        private ThirdPersonController[] landerPlayers = new ThirdPersonController[2];
        private ThirdPersonController[] fisherPlayers = new ThirdPersonController[2];
        private ThirdPersonController[] traderPlayers = new ThirdPersonController[2];
        private ThirdPersonController selectPlayer;
        private int playerIndex;
        private SelectPlayerType playerType = SelectPlayerType.Islander;
        private bool isEnterGame;//在游戏场景中
        private bool readFlag;//是否已经选择过人物
        private Camera uiCamera;
        public override void Awake(object param1 = null, object param2 = null, object param3 = null)
        {

            Debug.Log("SelectPlayerWindow Awake2222:" + m_GameObject.name);
            uiCamera = GameObject.Find("UICamera").GetComponent<Camera>();
            uiCamera.cullingMask = (1 << 5) + (1 << 6);
            GetAllComponents();
            AddAllButtonListeners();
            SetAllActions();
            isEnterGame = (bool)param1;
            readFlag = (bool)param2;
            playerIndex = 0;
            joyCanvas = UIManager.instance.GetWndByName(FilesName.JOYSTICKPANEL).m_Transform.GetComponent<UICanvasControllerInput>();
            touchCamera = Camera.main.GetComponent<TouchScreenCamera>();
            touchCamera.enabled = false;
            OnResetPlayerPos();           
            OnSelectPlayer(playerType);
            UpdateUI();
            
            





        }
        public override void OnShow(object param1 = null, object param2 = null, object param3 = null)
        {
           
            if (isEnterGame)
            {
                enterText.text = "确认选择";
                
            }
            else
            {
                enterText.text = "进入渔光之城";
            }
        }
        private void GetAllComponents()
        {
            UserInfoManager.playerRoot = m_GameObject.transform.Find("PlayerRoot");
            backBtn = m_GameObject.transform.Find("BackBtn").GetComponent<Button>();
            preBtn = m_GameObject.transform.Find("PreBtn").GetComponent<Button>();
            nextBtn = m_GameObject.transform.Find("NextBtn").GetComponent<Button>();
            enterBtn = m_GameObject.transform.Find("EnterGameBtn").GetComponent<Button>();
            islanderBtn = m_GameObject.transform.Find("RightSelectBtn/IslanderBtn").GetComponent<Button>();
            fisherBtn = m_GameObject.transform.Find("RightSelectBtn/FishermanBtn").GetComponent<Button>();
            traderBtn = m_GameObject.transform.Find("RightSelectBtn/TraderBtn").GetComponent<Button>();
            islanderBtnSelect = m_GameObject.transform.Find("RightSelectBtn/IslanderBtn/Selected").gameObject;
            fisherBtnSelect = m_GameObject.transform.Find("RightSelectBtn/FishermanBtn/Selected").gameObject;
            traderBtnSelect = m_GameObject.transform.Find("RightSelectBtn/TraderBtn/Selected").gameObject;
            enterText = m_GameObject.transform.Find("EnterGameBtn/Text").GetComponent<Text>();
            landerPlayers[0] = m_GameObject.transform.Find("PlayerRoot/BoyDaoMin").GetComponent<ThirdPersonController>();
            landerPlayers[1] = m_GameObject.transform.Find("PlayerRoot/GirDaoMin").GetComponent<ThirdPersonController>();
            fisherPlayers[0] = m_GameObject.transform.Find("PlayerRoot/BoyYuMin").GetComponent<ThirdPersonController>();
            fisherPlayers[1] = m_GameObject.transform.Find("PlayerRoot/GirlYuMin").GetComponent<ThirdPersonController>();
            traderPlayers[0] = m_GameObject.transform.Find("PlayerRoot/BoyShangRen").GetComponent<ThirdPersonController>();
            traderPlayers[1] = m_GameObject.transform.Find("PlayerRoot/GirlShangRen").GetComponent<ThirdPersonController>();
        }
        private void AddAllButtonListeners()
        {
            AddButtonClickListener(backBtn, () =>
            {
                OnClosePanel();
                
            });
            AddButtonClickListener(enterBtn, () =>
            {
                OnConfirmSelectPlayer();
            });
            AddButtonClickListener(preBtn, () =>
            {
                OnSelectPrePlayer();
            });
            AddButtonClickListener(nextBtn, () =>
            {
                OnSelectNextPlayer();
            });
            AddButtonClickListener(islanderBtn, () =>
            {
                OnSelectPlayer(SelectPlayerType.Islander);
            });
            AddButtonClickListener(fisherBtn, () =>
            {
                OnSelectPlayer(SelectPlayerType.Fisherman);
            });
            AddButtonClickListener(traderBtn, () =>
            {
                OnSelectPlayer(SelectPlayerType.Trader);
            });

        }
        private void SetAllActions()
        {

        }
        private void UpdateUI()
        {
            HideRightBtns();
            islanderBtn.gameObject.SetActive(true);
            if (UserInfoManager.palyerStatus.Equals("2"))
            {
                fisherBtn.gameObject.SetActive(true);
            }
            else if (UserInfoManager.palyerStatus.Equals("3"))
            {
                traderBtn.gameObject.SetActive(true);
            }
            if (UserInfoManager.palyerStatus.Equals("4"))
            {
                fisherBtn.gameObject.SetActive(true);
                traderBtn.gameObject.SetActive(true);
            }
            if (readFlag)
            {
                selectPlayer = GetEnterGamePlayer();
                StartSelectPlayer(selectPlayer, true);
            }

        }
        private void HideRightBtns()
        {
            islanderBtn.gameObject.SetActive(false);
            fisherBtn.gameObject.SetActive(false);
            traderBtn.gameObject.SetActive(false);
        }
        private ThirdPersonController GetEnterGamePlayer()
        {
            int index = int.Parse(UserInfoManager.playerSex);
            if (UserInfoManager.playerType.Equals("0"))
            {
                return landerPlayers[index];
            }
            else if (UserInfoManager.playerType.Equals("1"))
            {
                return fisherPlayers[index];
            }
            else if (UserInfoManager.playerType.Equals("2"))
            {
                return traderPlayers[index];
            }
            return null;
        }
        private void OnResetPlayerPos()
        {

            for (int i = 0; i < landerPlayers.Length; i++)
            {
                OnResetPlayer(landerPlayers[i]);

            }
            for (int i = 0; i < fisherPlayers.Length; i++)
            {
                OnResetPlayer(fisherPlayers[i]);

            }
            for (int i = 0; i < traderPlayers.Length; i++)
            {
                OnResetPlayer(traderPlayers[i]);

            }
        }
        private void OnResetPlayer(ThirdPersonController player)
        {
            player.transform.localPosition = player.birthPos;
            player.transform.rotation = Quaternion.Euler(0, -180, 0);
            player.enabled = false;
            player.transform.GetComponent<CharacterController>().enabled = false;
            player.transform.GetComponent<Animator>().SetBool("Grounded", true);
            player.transform.GetComponent<Animator>().SetFloat("MotionSpeed", 1);



        }
        private void OnSelectNextPlayer()
        {
            playerIndex++;
            if (playerIndex > 1)
            {
                playerIndex = 0;
            }
            OnShowPlayer(playerIndex);
        }
        private void OnSelectPrePlayer()
        {
            playerIndex--;
            if (playerIndex < 0)
            {
                playerIndex = 1;
            }
            OnShowPlayer(playerIndex);
        }
        private void OnShowPlayer(int index)
        {
            HideAllPlayers();
            if (playerType == SelectPlayerType.Islander)
            {
                selectPlayer = landerPlayers[index];

            }
            else if (playerType == SelectPlayerType.Fisherman)
            {
                selectPlayer = fisherPlayers[index];

            }
            else if (playerType == SelectPlayerType.Trader)
            {
                selectPlayer = traderPlayers[index];

            }
            selectPlayer.gameObject.SetActive(true);
            selectPlayer.GetComponent<Animator>().SetBool("Grounded", true);
            selectPlayer.GetComponent<Animator>().SetFloat("MotionSpeed", 1);

        }
        private void OnSelectPlayer(SelectPlayerType type)
        {
            islanderBtnSelect.SetActive(type == SelectPlayerType.Islander);
            fisherBtnSelect.SetActive(type == SelectPlayerType.Fisherman);
            traderBtnSelect.SetActive(type == SelectPlayerType.Trader);
            playerType = type;
            playerIndex = 0;
            OnShowPlayer(playerIndex);
        }
        private void OnConfirmSelectPlayer()
        {
            StartSelectPlayer(selectPlayer, true);
            if (isEnterGame)
            {
                UIManager.instance.PopUpWnd(FilesName.MAINPANEL, true, false);
                UIManager.instance.PopUpWnd(FilesName.JOYSTICKPANEL, true, false,true,false);
            }
        }

        public void StartSelectPlayer(ThirdPersonController player, bool ready)
        {
            
            uiCamera.cullingMask = (1 << 5);
            if (player != null)
            {
                UserInfoManager.selectPlayer = this.selectPlayer;
                Debug.LogError("StartSelectPlayer开始选择玩家了：" + UserInfoManager.selectPlayer.gameObject.name + "  ready:" + ready);
                joyCanvas.starterAssetsInputs = player.assetsInputs; //joy中文
                touchCamera.target = player.target.transform;// 
                if (!isEnterGame)
                {

                    if (ready)
                    {
                        Debug.Log("开始加载场景了选择过人物了");
                        JsonData data = new JsonData();
                        data["type"] =int.Parse( UserInfoManager.playerType);
                        data["sex"] = int.Parse(UserInfoManager.playerSex);
                        //WebRequestManager.instance.AsyncLoadUnityWebRequest(WebRequestUtils.enterTheMeta, (string jsonStr) => { }, true, JsonMapper.ToJson(data), RFrameWork.instance.token);
                        GameMapManager.instance.LoadScene("Game", FilesName.MAINPANEL, HouseManager.LoadMainScene);
                    }
                    else
                    {
                        JsonData data = new JsonData();
                        data["type"] = (int)playerType;
                        data["sex"] = playerIndex;
                        SelectPlayerFunc(data);
                        //WebRequestManager.instance.AsyncLoadUnityWebRequest(WebRequestUtils.enterTheMeta, SelectPlayerFunc, true, JsonMapper.ToJson(data), RFrameWork.instance.token);

                    }

                }
                else
                {
                    JsonData data = new JsonData();
                    data["type"] = (int)playerType;
                    data["sex"] = playerIndex;
                    SelectPlayerFunc(data);
                   // WebRequestManager.instance.AsyncLoadUnityWebRequest(WebRequestUtils.enterTheMeta, SelectPlayerFunc, true, JsonMapper.ToJson(data), RFrameWork.instance.token);

                }

                UIManager.instance.CloseWnd(this);
                touchCamera.enabled = false;

            }
            else
            {
                Debug.LogError("玩家不存在为空error");
            }

        }

        private void OnEnterGame(bool isEnter,JsonData data)
        {
            if (isEnter)
            {

                UserInfoManager.selectPlayer.transform.position = UserInfoManager.playerScenePos;
                UserInfoManager.selectPlayer.transform.rotation = UserInfoManager.playerSceneRotation;
                UserInfoManager.selectPlayer.transform.parent = null;
                UserInfoManager.selectPlayer.gameObject.SetActive(true);
                UserInfoManager.selectPlayer.enabled = true;
                UserInfoManager.selectPlayer.transform.localScale = Vector3.one*0.8f;
                UserInfoManager.selectPlayer.transform.GetComponent<CharacterController>().enabled = true;
                touchCamera.enabled = true;
            }


        }
        private void LoadMainScene(Action obj)
        {
            obj.Invoke();
        }
        private void LoadSceneFinish()
        {
            Debug.Log("LoadSceneFinish场景加载完成");
        }
        private void SelectPlayerFunc(JsonData data)
        {
            OnEnterGame(true,data);
        }

        private void HideAllPlayers()
        {
            for (int i = 0; i < landerPlayers.Length; i++)
            {
                landerPlayers[i].gameObject.SetActive(false);
            }
            for (int i = 0; i < fisherPlayers.Length; i++)
            {
                fisherPlayers[i].gameObject.SetActive(false);
            }
            for (int i = 0; i < traderPlayers.Length; i++)
            {
                traderPlayers[i].gameObject.SetActive(false);
            }
        }
        private void OnClosePanel()
        {
            if (isEnterGame)
            {
                UIManager.instance.CloseWnd(this);
                UIManager.instance.PopUpWnd(FilesName.MAINPANEL);
                UIManager.instance.PopUpWnd(FilesName.JOYSTICKPANEL, true, false,true,false);
                uiCamera.cullingMask = (1 << 5);
                if (isEnterGame)
                {
                    OnEnterGame(true,null);
                }
            }

            else
            {
                RFrameWork.instance.OpenCommonConfirm("提示", "确认退出渔光之城", () => { ToolManager.ExitGame(); }, () => { });

            }
        }



    }
}
