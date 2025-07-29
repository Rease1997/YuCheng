using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;
using UnityEngine.UI;

public class GameManager_COS : MonoBehaviour
{
    public Button switchBtn;
    public Camera mainCamera;
    public UICanvasControllerInput joyCanvas;
    public TouchScreenCamera touchCamera;
    public float firstPerDis;
    public float firstPerFov=40;
    public float thirdPerDis=3;
    public float thirdPerFov=50;



    bool isFirst;
    private void Awake()
    {
        
        mainCamera = Camera.main;
        touchCamera=mainCamera.GetComponent<TouchScreenCamera>();
        Debug.Log("GameManager_COS Awake");
    }

    private void Start()
    {
        SwitchPersonView(isFirst);
        switchBtn.onClick.AddListener(() =>
        {
            isFirst = !isFirst;
            SwitchPersonView(isFirst);

        });
        SwitchPlayer(GameObject.FindObjectOfType<ThirdPersonController>());
    }
    public void SwitchPlayer(ThirdPersonController player)
    {
        joyCanvas.starterAssetsInputs = player.assetsInputs; //joy中文
        touchCamera.target = player.target.transform;// 
    }
    public void SwitchPersonView(bool isFirst)
    {
         
        if (isFirst)
        {
            touchCamera.distance = firstPerDis;
            mainCamera.fieldOfView = firstPerFov;
            GetComponent<Camera>().cullingMask &= ~(1 << 6); //tree   
        }
        else
        {
            touchCamera.distance = thirdPerDis;
            mainCamera.fieldOfView = thirdPerFov;
        }
        

    }

     




}
