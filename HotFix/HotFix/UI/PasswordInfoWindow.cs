using HotFix.Common;
using HotFix.Common.Utils;
using LitJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace HotFix
{
    public class PasswordInfoWindow : Window
    {

        public Button closeBtn;
        public Button sureBtn;
        public Button[] inputBtns;
        public Text[] showTxts;
        public Button deleteBtn;
        string password = "";
        string reconFirm = "";
        public override void Awake(object param1 = null, object param2 = null, object param3 = null)
        {
            GetAllComponents();
            AddBtnClickListener();
        }
        private void GetAllComponents()
        {
            closeBtn = m_Transform.Find("CloseButton").GetComponent<Button>();
            sureBtn = m_Transform.Find("Bg/SureButton").GetComponent<Button>();
            deleteBtn = m_Transform.Find("Bg/DeleteBtn").GetComponent<Button>();
            inputBtns = m_Transform.Find("Bg/InputNumber").GetComponentsInChildren<Button>();
            showTxts = m_Transform.Find("Bg/ShowNumber").GetComponentsInChildren<Text>();
        }

        private void AddBtnClickListener()
        {
            ClearValue();
            AddButtonClickListener(closeBtn, OnClosePanel);
            AddButtonClickListener(deleteBtn, DeleteTxtValue);
            AddButtonClickListener(sureBtn, ConfirmBtnClicked);
            AddButtonClickListener(inputBtns[0],()=> {
                NumBtnClicked(1);
            } );
            AddButtonClickListener(inputBtns[1], () => {
                NumBtnClicked(2);
            });
            AddButtonClickListener(inputBtns[2], () => {
                NumBtnClicked(3);
            });
            AddButtonClickListener(inputBtns[3], () => {
                NumBtnClicked(4);
            });
            AddButtonClickListener(inputBtns[4], () => {
                NumBtnClicked(5);
            });
            AddButtonClickListener(inputBtns[5], () => {
                NumBtnClicked(6);
            });
            AddButtonClickListener(inputBtns[6], () => {
                NumBtnClicked(7);
            });
            AddButtonClickListener(inputBtns[7], () => {
                NumBtnClicked(8);
            });
            AddButtonClickListener(inputBtns[8], () => {
                NumBtnClicked(9);
            });
            AddButtonClickListener(inputBtns[9], () => {
                NumBtnClicked(0);
            });


        }




        private void NumBtnClicked(int num)
        {
            ClearValue();
             
            password += num;
            SetTxtValue();
            Debug.Log("NumBtnClicked num:"+ num+" password:" + password);
            
        }
        private void OnClosePanel()
        {
            UIManager.instance.CloseWnd(this);
        }

        Action<string> act;
        public override void OnShow(object param1 = null, object param2 = null, object param3 = null)
        {
            ClearValue();
            password = "";
            reconFirm = "";
            act = param1 as Action<string>;
        }

        private void SetTxtValue()
        {
            if (password.Length <= 6)
            {
                reconFirm = password;
                Debug.Log("SetTxtValue ReconFirm:" + reconFirm);
            }
            if (password != null && password.Length > 0)
            {
                for (int i = 0; i < reconFirm.Length; i++)
                {
                    showTxts[i].text = "";
                    showTxts[i].text = password[i].ToString();
                    showTxts[i].text = "*";
                }
            }
        }
        private void MinusPassword()
        {
            password = reconFirm.Substring(0, reconFirm.Length - 1);
            Debug.Log(password);
        }
        private void ClearValue()
        {
            for (int i = 0; i < showTxts.Length; i++)
            {
                showTxts[i].text = "";
            }
        }
        private void DeleteTxtValue()
        {
            MinusPassword(); 
            ClearValue();
            Debug.Log("PasswordInfoPanel" + password);
            SetTxtValue();

        }
        private void ConfirmBtnClicked()
        {
            Debug.Log("ConfirmBtnClicked password:" + reconFirm);
            if (reconFirm.Length == 6 && reconFirm != null)
            {
                act(reconFirm);
            }
            else
            {
                RFrameWork.instance.OpenCommonConfirm("提示", "请输入正确的密码", () => { }, null);


            }
        }
    }
}
