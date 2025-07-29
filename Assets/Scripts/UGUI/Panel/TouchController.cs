using StarterAssets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    RectTransform center;

    Vector2 centerPos;

    GameObject myPlayer;

    bool isMove;

    public Action<PointerEventData> PointerDownAct;
    public Action<PointerEventData> PointerUpAct;
    public Action<PointerEventData> DragAct;

    public void Init(GameObject myplayer)
    {
        Center = transform.GetChild(0).transform as RectTransform;
        CenterPos = Center.position;
        MyPlayer = myplayer;
        IsMove = false;
    }

    Vector2 moveVec;

    public RectTransform Center { get => center; set => center = value; }
    public Vector2 CenterPos { get => centerPos; set => centerPos = value; }
    public GameObject MyPlayer { get => myPlayer; set => myPlayer = value; }
    public bool IsMove { get => isMove; set => isMove = value; }
    public Vector2 MoveVec { get => moveVec; set => moveVec = value; }

   
    private void FixedUpdate()
    {
        if (IsMove)
        {
            MyPlayer.GetComponent<PlayerInputController_CC>().MoveInput(MoveVec);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        DragAct(eventData);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        PointerDownAct(eventData);

    }

    public void OnPointerUp(PointerEventData eventData)
    {
        PointerUpAct(eventData);

    }
}
