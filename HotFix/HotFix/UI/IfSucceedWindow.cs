using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UI;
using UnityEngine;

namespace HotFix
{
    
    internal class IfSucceedWindow:Window
    {
        private Transform succeedImg;
        private Transform unSucceedImg;
        private Text title;
        private Text des;
        private Button sureBtn;
        private bool showLuckyBag;
        private int prayType;
        private int prayPos;
        public override void Awake(object param1 = null, object param2 = null, object param3 = null)
        {
            GetAllComponent();
            AddAllBtnListener();
            showLuckyBag = false;
        }

        public override void OnShow(object param1 = null, object param2 = null, object param3 = null)
        {
            object[] objs = param1 as object[];
            if(param2!=null)
            {
                showLuckyBag = (bool)(param2);
            }
            if(showLuckyBag)
            {
                prayType = int.Parse(objs[3].ToString());
                prayPos=(int)objs[4];

            }
            UpdateUI(objs[0].ToString(), objs[1].ToString(), (bool)objs[2]);
        }

        private void UpdateUI(string titleStr,string desStr,bool ifSucceed)
        {
            succeedImg.gameObject.SetActive(ifSucceed);
            unSucceedImg.gameObject.SetActive(!ifSucceed);
            title.text = titleStr;
            des.text = desStr;
        }

        private void GetAllComponent()
        {
            succeedImg = m_Transform.Find("BackImg/SucceedImg");
            unSucceedImg = m_Transform.Find("BackImg/UnSucceedImg");
            title = m_Transform.Find("BackImg/TitleText").GetComponent<Text>();
            des = m_Transform.Find("BackImg/DesText").GetComponent<Text>();
            sureBtn = m_Transform.Find("BackImg/SureBtn").GetComponent<Button>();
        }

        private void AddAllBtnListener()
        {
            AddButtonClickListener(sureBtn, SureFunc);
        }

        private void SureFunc()
        {
            UIManager.instance.CloseWnd(this);
            if (showLuckyBag)
            {
                MainWindow.StartPrayAction(prayType, prayPos-1);
            }
        }
    }
}
