using UnityEngine.UI;
using LitJson;
using UnityEngine;
using System;
using System.Collections.Generic;
using HotFix.Common.Utils;
using StarterAssets;
using System.Collections;

namespace HotFix
{
   public enum TranslateType
    {
        None,
        IsLand,
        Lighting,
        Gulf,
        OldTown,
        PersonHorse,
    }
    class MapWindow : Window
    {

        private Transform[] transPositions = new Transform[5];
        private Button oldTownBtn, gulfBtn, lightBtn, islandBtn,returnBtn,personalBtn;
        public override void Awake(object param1 = null, object param2 = null, object param3 = null)
        {           
            Debug.Log("MapWindow Awake:" + m_GameObject.name);
            GetAllComponents();
            AddAllButtonListeners();          
            transPositions[0] = GameObject.Find("TeleportMainLand").transform;
            transPositions[1] = GameObject.Find("TeleportPort").transform;
            transPositions[2] = GameObject.Find("TeleportGuangjing").transform;
            transPositions[3] = GameObject.Find("TeleportEastGateIsland").transform;
            transPositions[4] = GameObject.Find("TeleportPersonalHouse").transform;


        }
        public override void OnShow(object param1 = null, object param2 = null, object param3 = null)
        {
            m_GameObject.transform.SetAsLastSibling();

        }
        private void GetAllComponents()
        {
            oldTownBtn = m_GameObject.transform.Find("Bg/OldTownBtn").GetComponent<Button>();
            gulfBtn = m_GameObject.transform.Find("Bg/GulfBtn").GetComponent<Button>();
            lightBtn = m_GameObject.transform.Find("Bg/LightBtn").GetComponent<Button>();
            islandBtn = m_GameObject.transform.Find("Bg/IslandBtn").GetComponent<Button>();
            personalBtn = m_GameObject.transform.Find("Bg/PersonalBtn").GetComponent<Button>();
            returnBtn = m_GameObject.transform.Find("ReturnBtn").GetComponent<Button>();
            



        }
        private void AddAllButtonListeners()
        {
            AddButtonClickListener(oldTownBtn, () =>
            {
                OnTranslatePos(TranslateType.OldTown);
            });
            AddButtonClickListener(personalBtn, () =>
            {
                OnTranslatePos(TranslateType.PersonHorse);
            });
            
            AddButtonClickListener(gulfBtn, () =>
            {
                OnTranslatePos(TranslateType.Gulf);
            });
            AddButtonClickListener(lightBtn, () =>
            {
                OnTranslatePos(TranslateType.Lighting);
            });
            AddButtonClickListener(islandBtn, () =>
            {
                OnTranslatePos(TranslateType.IsLand);
            });
            AddButtonClickListener(returnBtn, () =>
            {
                UIManager.instance.CloseWnd(this);
            });

        }
        private void OnTranslatePos(TranslateType type)
        {
            UserInfoManager.curTrasnlateType = type;
            switch (type)
            {
                case TranslateType.OldTown:
                    UserInfoManager.fishAnimator.speed = 1;
                    IEnumeratorTool.instance.StartCoroutineNew(ToolManager.WaitSecondsToTranslate(transPositions[0],FilesName.MAPPANEL,TranslateType.OldTown));
                    Debug.Log("传送到古镇："+ transPositions[0].position);
                    break;
                    case TranslateType.Gulf:
                    UserInfoManager.fishAnimator.speed = 1;
                    IEnumeratorTool.instance.StartCoroutineNew(ToolManager.WaitSecondsToTranslate(transPositions[1], FilesName.MAPPANEL,TranslateType.Gulf));
                    Debug.Log("传送到海峡：" + transPositions[1].position);
                    break ;
                    case TranslateType.Lighting:
                    UserInfoManager.fishAnimator.speed = 0;
                    IEnumeratorTool.instance.StartCoroutineNew(ToolManager.WaitSecondsToTranslate(transPositions[2], FilesName.MAPPANEL,TranslateType.Lighting));
                    Debug.Log("传送到光镜：" + transPositions[2].position);                   
                    break;
                case TranslateType.IsLand:
                    UserInfoManager.fishAnimator.speed = 1;                    
                    IEnumeratorTool.instance.StartCoroutineNew(ToolManager.WaitSecondsToTranslate(transPositions[3], FilesName.MAPPANEL,TranslateType.IsLand));
                    Debug.Log("传送到东门岛：" + transPositions[3].position);
                    break;
                case TranslateType.PersonHorse:
                    UserInfoManager.fishAnimator.speed = 1;
                    IEnumeratorTool.instance.StartCoroutineNew(ToolManager.WaitSecondsToTranslate(transPositions[4], FilesName.MAPPANEL, TranslateType.PersonHorse));
                    Debug.Log("传送到个人馆：" + transPositions[4].position);
                    break;
                default:
                    Debug.Log("传送位置错误");
                   
                    break;
            }
        }
       



    }
}
