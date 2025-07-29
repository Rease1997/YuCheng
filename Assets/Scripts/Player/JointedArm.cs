using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

/// <summary>
/// 摇杆
/// </summary>
public class JointedArm : ScrollRect, IPointerDownHandler
{
    public Action<Vector2> onDragCb;
    public Action<Vector2> onDragCb2;
    public Action onStopCb;
    public Vector3 originalPos;
    public bool Run;

    protected float radius = 0f;

    private RectTransform trans;
    private RectTransform bgTrans;
    private Camera uiCam;

    protected override void Awake()
    {
        base.Awake();
        trans = transform.GetComponent<RectTransform>();
        originalPos = trans.anchoredPosition;
        bgTrans = trans.Find("bg") as RectTransform;
        uiCam = RFrameWork.instance.transform.Find("UIRoot/UICamera").GetComponent<Camera>();
    }
    int num = 0;
    Vector2 laseV2;
    bool isPoint = false;
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            //松手时，摇杆复位
            trans.anchoredPosition = originalPos;
            this.content.localPosition = Vector3.zero;
        }
        var contentPostion = this.content.anchoredPosition;
        if (contentPostion.magnitude > radius)
        {
            contentPostion = contentPostion.normalized * radius;
            SetContentAnchoredPosition(contentPostion);
        }
        num = num + 1;
        if (num >= 60)
        {
            num = 0;
            laseV2 = contentPostion;
            
        }
        if (onDragCb2 != null&& isPoint)
        {
            if (laseV2 != contentPostion)
                onDragCb2(contentPostion);
            else
            {
                trans.position = uiCam.ScreenToWorldPoint(Input.mousePosition);
                trans.localPosition = new Vector3(trans.localPosition.x, trans.localPosition.y, 0);
                onDragCb2(Vector2.zero);
            }
        }

    }

    protected override void Start()
    {
        base.Start();
        //计算摇杆块的半径
        radius = bgTrans.sizeDelta.x * 0.5f;
    }

    public override void OnDrag(PointerEventData eventData)
    {
        base.OnDrag(eventData);
        var contentPostion = this.content.anchoredPosition;
        if (Vector3.Distance(bgTrans.localPosition, content.localPosition) < 60)
        {
            Run = false;
        }
        else if (Vector3.Distance(bgTrans.localPosition, content.localPosition) >= 60)
        {
            Run = true;
        }
        if (contentPostion.magnitude > radius)
        {
            contentPostion = contentPostion.normalized * radius;
            SetContentAnchoredPosition(contentPostion);
        }
        // Debug.Log("摇杆滑动，方向：" + contentPostion);

        if (null != onDragCb)
            onDragCb(contentPostion);
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);
        // Debug.Log("摇杆拖动结束");

        if (onDragCb2 != null)
            isPoint = false; 
        if (null != onStopCb)
            onStopCb();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (onDragCb2 != null)
            isPoint = true;
        //点击到摇杆的区域，摇杆移动到点击的位置
        trans.position = uiCam.ScreenToWorldPoint(eventData.position);
        trans.localPosition = new Vector3(trans.localPosition.x, trans.localPosition.y, 0);
    }
}
