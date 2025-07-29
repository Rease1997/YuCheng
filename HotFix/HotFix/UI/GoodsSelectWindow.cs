using UnityEngine.UI;
using LitJson;
using UnityEngine;
using System;
using System.Collections.Generic;
using HotFix.Common.Utils;
using StarterAssets;

namespace HotFix
{

    class GoodsSelectWindow : Window
    {
        private Button backBtn, serachBtn, clearBtn;
        private InputField serachInput;
        private GoodsSelectData[] goodsSelectDatas;
        private Transform contentRoot;
        private GameObject itemClone,blankView;
        private Text titleText;
        private GoodsType goodsType;

        public override void Awake(object param1 = null, object param2 = null, object param3 = null)
        {
            Debug.Log("GoodsSelectWindow Awake" + m_GameObject.name);
            this.goodsSelectDatas = param1 as GoodsSelectData[];
            goodsType = (GoodsType)param2;
            GetAllComponents();
            AddAllButtonListeners();
            serachInput.text = "";

        }
        public override void OnShow(object param1 = null, object param2 = null, object param3 = null)
        {
            UpdateUI();
            
        }
        private void GetAllComponents()
        {
            backBtn = m_GameObject.transform.Find("BackBtn").GetComponent<Button>();
            serachBtn = m_GameObject.transform.Find("TopLefBtn/SearchBtn").GetComponent<Button>();
            clearBtn = m_GameObject.transform.Find("TopLefBtn/SearchInput/ClearBtn").GetComponent<Button>();
            serachInput = m_GameObject.transform.Find("TopLefBtn/SearchInput").GetComponent<InputField>();
            contentRoot = m_GameObject.transform.Find("Scroll View/Viewport/Content");
            itemClone = m_GameObject.transform.Find("GoodsSelectItem").gameObject;
            titleText = m_GameObject.transform.Find("BackBtn/Title").GetComponent<Text>();
            blankView = m_GameObject.transform.Find("BlankView").gameObject;




        }

        private void AddAllButtonListeners()
        {
            AddButtonClickListener(backBtn, () =>
            {
                OnClosePanel();

            });
            AddButtonClickListener(serachBtn, () =>
            {
                OnSearchExhibitions();

            });
            AddButtonClickListener(clearBtn, () =>
            {
                OnShowAllExhibition();

            });

        }
        private void UpdateUI()
        {
            if(goodsType==GoodsType.Goods2D)
            {
                titleText.text = "选择2D藏品";
            }
            else
            {
                titleText.text = "选择3D藏品";

            }
            OnHideAllItem();
            if (goodsSelectDatas.Length<=0)
            {              
                blankView.gameObject.SetActive(true);
                return;
            }else
            {
                blankView.gameObject.SetActive(false);
            }
            for (int i = 0; i < goodsSelectDatas.Length; i++)
            {
                GoodsSelectData data = goodsSelectDatas[i];
                if (UserInfoManager.goodsSelectItemList.Count > i)
                {
                    GoodsSelectItem goodsItem = UserInfoManager.goodsSelectItemList[i];
                    goodsItem.UpdateUI(data);
                }
                else
                {
                    GameObject clone = GameObject.Instantiate(itemClone);
                    clone.transform.parent = contentRoot.transform;
                    GoodsSelectItem goodsItem = new GoodsSelectItem();
                    goodsItem.OnInit(clone.transform, data);
                    UserInfoManager.goodsSelectItemList.Add(goodsItem);

                }
            }
        }
        private void OnHideAllItem()
        {
            for (int i = 0; i < UserInfoManager.goodsSelectItemList.Count; i++)
            {
                GoodsSelectItem goodsSelectItem = UserInfoManager.goodsSelectItemList[i];
                goodsSelectItem.OnHideUI();
            }
        }
        private void OnSearchExhibitions()
        {
            if (string.IsNullOrEmpty(serachInput.text))
            {
                RFrameWork.instance.OpenCommonConfirm("提示", "输入内容不能为空！！！", () => { }, () => { });

            }
            else
            {
                for (int i = 0; i < UserInfoManager.goodsSelectItemList.Count; i++)
                {
                    GoodsSelectItem goodsSelectItem = UserInfoManager.goodsSelectItemList[i];
                    if(goodsSelectItem.isShowing)
                    {
                        if (goodsSelectItem.GoodName.Contains(serachInput.text))
                        {
                            goodsSelectItem.mTrans.gameObject.SetActive(true);
                        }
                        else
                        {
                            goodsSelectItem.mTrans.gameObject.SetActive(false);
                        }
                    }
                    
                        

                }

            }

        }

    
    private void OnShowAllExhibition()
    {
        serachInput.text = "";
        for (int i = 0; i < UserInfoManager.goodsSelectItemList.Count; i++)
        {
            GoodsSelectItem goodsSelectItem = UserInfoManager.goodsSelectItemList[i];
            if (goodsSelectItem.isShowing)
            {
                goodsSelectItem.mTrans.gameObject.SetActive(true);
            }

        }
    }
    private void OnClosePanel()
    {
        OnHideAllItem();
        UIManager.instance.CloseWnd(this);
    }

}
}
