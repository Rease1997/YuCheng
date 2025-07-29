using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HorseAnimSpeed : MonoBehaviour
{
    [Range(1, 1.4f)]
    public float speed = 1;
    public Animator anim;
    private AnimatorStateInfo animStateInfo;
    private bool isStart;
    private string stateName;
    private int layer = 0;
   
    // Start is called before the first frame update
    void Awake()
    {
        anim = this.GetComponent<Animator>();
       
        
    }
    public void SetAniSpeed(string stateName, int layer, float aniStateSpeed)
    {
        this.stateName = stateName;
        this.layer = layer;
        this.speed = aniStateSpeed;
        isStart = true;
    }

    public void TestSetSpeed1()
    {
        SetAniSpeed("H_Gallop_IP", 0, 1.4f);
    }
    public void TestSetSpeed2()
    {
        SetAniSpeed("H_Gallop_IP", 0, 0.2f);
    }
    private void Update()
    {
        if(isStart)
        {
            animStateInfo = anim.GetCurrentAnimatorStateInfo(0);
            if (animStateInfo.IsName("H_Gallop_IP"))
            {

                anim.speed = speed;
            }
           
        }
        
    }
    
    public void ResetSpeed()
    {
        isStart = false;
        anim.speed = 1f;
        
    }


}
