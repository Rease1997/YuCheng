using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 触发系统脚本
/// </summary>
public class TriggerEvent : MonoBehaviour, IPointerClickHandler,IBeginDragHandler,IDragHandler,IEndDragHandler
{
    public Action<object[]> ClickAct;
    public Action<object[]> BeginDrag;
    public Action<object[]> Drag;
    public Action<object[]> EndDrag;
    public Action<Collision> CollisionEnter;
    public Action<Collision> CollisionStay;
    public Action<Collision> CollisionExit;
    public Action<Collider> TriggerEnter;
    public Action<Collider> TriggerStay;
    public Action<Collider> TriggerExit;

    public void OnBeginDrag(PointerEventData eventData)
    {
        if(BeginDrag!=null)
        {
            BeginDrag(new object[] { eventData, eventData.selectedObject });

        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(Drag!=null)
        {
            Drag(new object[] { eventData, eventData.selectedObject });
        }
        
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if(EndDrag!=null)
        {
            EndDrag(new object[] { eventData, eventData.selectedObject });

        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (ClickAct != null)
        {
            ClickAct(new object[] { eventData.selectedObject, eventData });
        }
        
        
        }

    public void OnMouseDown()
    {
        if(ClickAct!=null)
        {
            ClickAct(new object[] { gameObject });
        }
       
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (CollisionEnter != null)
        {
            CollisionEnter(collision);
        }
        
    }

    public void OnCollisionStay(Collision collision)
    {
        if(CollisionStay!=null)
        {
            CollisionStay(collision);
        }
        
    }

    public void OnCollisionExit(Collision collision)
    {
        if(CollisionExit!=null)
        {
            CollisionExit(collision);
        }
        
    }

    public void OnTriggerEnter(Collider other)
    {
      if(TriggerEnter != null)
        {
            TriggerEnter(other);
        }
           
    }

    public void OnTriggerStay(Collider other)
    {
        if(TriggerStay!=null)
        {
            TriggerStay(other);
        }
            
    }

    public void OnTriggerExit(Collider other)
    {
        if(TriggerExit!=null)
        {
            TriggerExit(other);
        }
        
            
    }
}
