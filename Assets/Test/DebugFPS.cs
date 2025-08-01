﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Reflection.Obfuscation(Exclude = true)]
public class DebugFPS : MonoBehaviour
{
    // Use this for initialization  
    void Start()
    {

    }

    // Update is called once per frame  
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        UpdateTick();
    }

    void OnGUI()
    {
        DrawFps();
    }

    private void DrawFps()
    {
        if (mLastFps > 50)
        {
            GUI.color = new Color(0, 1, 0);
        }
        else if (mLastFps > 40)
        {
            GUI.color = new Color(1, 1, 0);
        }
        else
        {
            GUI.color = new Color(1.0f, 0, 0);
          
        }
        GUIStyle fontStyle = GUI.skin.button;
        
        
        fontStyle.fontSize = 40;       //字体大小  
        GUI.Label(new Rect(50, 32, 364, 84), "fps: " + mLastFps,fontStyle);

    }

    private long mFrameCount = 0;
    private long mLastFrameTime = 0;
    static long mLastFps = 0;
    private void UpdateTick()
    {
        if (true)
        {
            mFrameCount++;
            long nCurTime = TickToMilliSec(System.DateTime.Now.Ticks);
            if (mLastFrameTime == 0)
            {
                mLastFrameTime = TickToMilliSec(System.DateTime.Now.Ticks);
            }

            if ((nCurTime - mLastFrameTime) >= 1000)
            {
                long fps = (long)(mFrameCount * 1.0f / ((nCurTime - mLastFrameTime) / 1000.0f));

                mLastFps = fps;

                mFrameCount = 0;

                mLastFrameTime = nCurTime;
            }
        }
    }
    public static long TickToMilliSec(long tick)
    {
        return tick / (10 * 1000);
    }

}
