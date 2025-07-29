using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HotFix
{
    /// <summary>
    /// 加载界面
    /// </summary>
    class LoadingWindow:Window
    {
        /// <summary>
        /// 是否执行过弹出界面的方法
        /// </summary>
        bool isOpen = false;
        /// <summary>
        /// 加载界面挂载脚本 用来获取组件
        /// </summary>
        private LoadingPanel m_MainPanel;
        /// <summary>
        /// 加载界面关闭后要显示的界面
        /// </summary>
        private string m_TargetPanelName;

        private Action<Action> action = null;

        private object[] objs = null;

        public override void Awake(object param1 = null, object param2 = null, object param3 = null)
        {
            m_MainPanel = m_GameObject.GetComponent<LoadingPanel>();
            GameMapManager.instance.LoadSceneOverCallBack = LoadOtherScene;
            m_TargetPanelName = (string)param1;
            action = (Action<Action>)param2;  
            isOpen = false;
            objs = (object[])param3;
            RFrameWork.instance.checkReconnect = false;
            Debug.Log("LoadingWindow Awake");
            
        }

        public override void OnUpdate()
        {
            if (m_MainPanel == null)
                return;

            if(action != null)
            {
                m_MainPanel.m_Text.text = string.Format("加载中：{0}%", UserInfoManager.LoadNum.ToString("f2"));
                m_MainPanel.m_Slider.value = UserInfoManager.LoadNum / 100.0f;
            }
            else
            {
                m_MainPanel.m_Slider.value = GameMapManager.LoadingProgress / 100.0f;
                m_MainPanel.m_Text.text = string.Format("加载中：{0}%", GameMapManager.LoadingProgress);
            }

            if (GameMapManager.LoadingProgress >= 88 && isOpen == false)
            {
                isOpen = true;
                if (action != null)
                {
                    action.Invoke(LoadOtherScene);
                }
                else
                {
                    LoadOtherScene();
                }
            }
        }
      
        /// <summary>
        /// 加载对应场景第一个UI
        /// </summary>
        public void LoadOtherScene()
        {
            Debug.Log("关闭loading界面:"+ SceneManager.GetActiveScene().name);
            UIManager.instance.CloseWnd(ConStr.LOADINGPANEL);
            string tarPanelName = m_TargetPanelName;
            object[] data = new object[3];
            if (objs != null)
            {
                for (int i = 0; i < 3; i++)
                {
                    if (objs[i] != null)
                    {
                        data[i] = objs[i];
                    }
                }
            }
            if (tarPanelName == FilesName.MAINPANEL)
            {
                UIManager.instance.CloseAllWnd();

                ClearAllDatas();

                UIManager.instance.PopUpWnd(tarPanelName, false, false, data[0], data[1], data[2]);
            }
            else
                UIManager.instance.PopUpWnd(tarPanelName,true,false, data[0], data[1], data[2]);


            if(SceneManager.GetActiveScene().name.Equals("Game"))
            {
                UserInfoManager.playerScenePos = GameObject.Find("BirthEastGateIsLand").transform.position;
                UserInfoManager.playerSceneRotation = Quaternion.Euler(0, -137, 0);
                UIManager.instance.PopUpWnd(FilesName.JOYSTICKPANEL, true, false,true,true);              
                Debug.Log("第一次进入Game显示SelectPlayer：" + UserInfoManager.selectPlayer.gameObject.name);

            }
        }

        private void ClearAllDatas()
        {
            UserInfoManager.isFirstInGame = true;
            UserInfoManager.goodsItemList.Clear();
            UserInfoManager.goods2DItemList.Clear();
            UserInfoManager.goodsSelectItemList.Clear();
        }
    }
}
