using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEngine.UI.ScrollRect;

public class ScrollRectRef : MonoBehaviour,IBeginDragHandler, IEndDragHandler,IDragHandler
{
    //�Ƿ�ˢ��
    bool isRef = false;
    //�Ƿ����϶�
    bool isDrag = false;
    //�������ˢ������ ִ�еķ���
    public Action top;
    public Action bottom;
    public Scrollbar scrollbar = null;
    public float len = 0;
    void Awake()
    {
    }

    public void RefreshLen()
    {
        len = scrollbar.size;
    }

    void Start()
    {

    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        isDrag = true;
        RefreshLen();
    }

    public void OnDrag(PointerEventData eventData)
    {
        //������϶� ��Ȼ��ִ��֮�µĴ���
        if (!isDrag)
            return;
        //�������Content
        isRef = true;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if(scrollbar.size <= len /5*4 && isRef)
        {
            if (scrollbar.value > 0)
            {
                if (top != null)
                    top.Invoke();
            }
            else if(scrollbar.value<=0)
            {
                if (bottom != null)
                    bottom.Invoke();
            }
        }
        isRef = false;
        isDrag = false;
    }
}