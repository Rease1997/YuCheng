
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(InputField))]
public class InputContentSizeHeight : MonoBehaviour
{
    [SerializeField]
    private bool isNumber;
    [SerializeField]
    private Vector2 originalSize;   //������ʼ��С
    [SerializeField]
    private float maxHeight = 500;//��ʼ�����߶�+ÿ�����ӵĸ߶�*n �����ʼ�����30 ÿ��������16 �������ʾ5���־���30+16*4=94 �������ֵ����94
    private float perHeight = 16;//ÿ����һ��ֵҪ���ӵĿ��
    private float originHeight = 88f;
    public void SetInputValue(int maxH,int perH,float originH,Vector2 originalSize)
    {
        this.maxHeight = maxH;
        this.perHeight = perH;
        this.originHeight = originH;
        this.originalSize = originalSize;
        this.GetComponent<RectTransform>().sizeDelta = originalSize;
         
    }

    [SerializeField]
    private RectTransform m_Rect;
    
    private RectTransform rectTransform
    {
        get
        {
            if (m_Rect == null)
                m_Rect = GetComponent<RectTransform>();
            return m_Rect;
        }
    }

    private InputField _inputField;
    public InputField inputField
    {
        get
        {
            return _inputField ?? (_inputField = this.GetComponent<InputField>());
        }
    }


    protected void Awake()
    {
        this.originalSize = this.GetComponent<RectTransform>().sizeDelta;
         

    }
    private void Start()
    {
        this.inputField.onValueChanged.AddListener(OnValueChanged);
    }
     
    void Test()
    {
        SetInputValue(1000, 16,100,new Vector2(100,100));
    }
    public void OnValueChanged(string v)
    {
      
        int i = (int)((LayoutUtility.GetPreferredHeight(m_Rect)) / perHeight);
        if (LayoutUtility.GetPreferredHeight(m_Rect) >= maxHeight)
        {
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, maxHeight);
        }
        else
        {
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, i <= 1 ? originHeight : originHeight + perHeight * (i - 1));
        }
        
         
    }



}