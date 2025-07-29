using HotFix.Common.Utils;
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
    internal class BagWindow : Window
    {
        private Button returnBtn;
        private Button fishingBtn;
        private Transform content;
        private Transform itemsTrans;
        private List<List<FishData>> list = new List<List<FishData>>();

        public override void Awake(object param1 = null, object param2 = null, object param3 = null)
        {
            GetAllComponent();
            AddAllBtnListener();
        }

        public override void OnShow(object param1 = null, object param2 = null, object param3 = null)
        {
            list.Clear();
            list = new List<List<FishData>>();
            list = param1 as List<List<FishData>>;
            UpdateUI();
            WebRequestFuncitons.GetAllBoatData();
            //WebRequestManager.instance.AsyncLoadUnityWebRequest(WebRequestUtils.getAllBoatData, WebRequestFuncitons.GetAllBoatData, true, "{}", RFrameWork.instance.token);

        }

        private void GetAllComponent()
        {
            returnBtn = m_Transform.Find("ReturnBtn").GetComponent<Button>();
            fishingBtn = m_Transform.Find("FishingBtn").GetComponent<Button>();
            content = m_Transform.Find("BagItemScrollView/Viewport/Content");
            itemsTrans = m_Transform.Find("BagItemScrollView/Viewport/Content/Items");
        }

        private void AddAllBtnListener()
        {
            AddButtonClickListener(returnBtn, ReturnFunc);
            AddButtonClickListener(fishingBtn, ShowDetailPanel);
        }

        private void UpdateUI()
        {
            for (int i = 0; i < content.childCount; i++)
            {
                content.GetChild(i).gameObject.SetActive(false);
            }
            int count = 0;
            foreach (var data in list)
            {
                if (count + 1 <= content.childCount)
                {
                    BagItems item = new BagItems();
                    item.OnInit(content.GetChild(count), data);
                    item = null;
                }
                else
                {
                    Transform obj = Object.Instantiate<GameObject>(itemsTrans.gameObject, content).transform;
                    BagItems item = new BagItems();
                    item.OnInit(obj, data);
                    item = null;
                }
                count++;
            }
        }

        private void ReturnFunc()
        {
            UIManager.instance.CloseWnd(this);
             
        }

        private void ShowDetailPanel()
        {
            Debug.Log("ShowDetailPanel 船只数量："+ UserInfoManager.boatDataList.Count);
            if (UserInfoManager.boatDataList != null && UserInfoManager.boatDataList.Count > 0)
            {
                Transform mainCamera = Camera.main.transform;
                mainCamera.GetComponent<TouchScreenCamera>().enabled = false;
                SetCameraPos(mainCamera);
                UIManager.instance.CloseAllWnd();
                UIManager.instance.PopUpWnd(FilesName.DETAILPANEL, true, false, UserInfoManager.boatDataList[0]);
                
            }
            else
            {
                RFrameWork.instance.OpenCommonConfirm("提示", "暂无船只！！！", () => { }, null);

            }
        }
        private void SetCameraPos(Transform mainCamera)
        {
            mainCamera.transform.position = new Vector3(-83.72f, -2.3f, 48.24f) ;
            mainCamera.transform.rotation = Quaternion.Euler(0, 140.047f,0);
        }
        private void GetDetailInfoResponse(string jsonStr)
        {
            Debug.Log("GetDetailInfoResponse:" + jsonStr);
        }
    }
}
