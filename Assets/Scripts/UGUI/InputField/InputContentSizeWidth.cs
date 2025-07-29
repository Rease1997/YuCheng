
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(InputField))]
public class InputContentSizeWidth : MonoBehaviour
{
    [SerializeField]
    private bool isNumber;
    [SerializeField]
    private Vector2 originalSize;   //������ʼ��С
    [SerializeField]
    private float maxWeight = 500;//��ʼ�����߶�+ÿ�����ӵĸ߶�*n �����ʼ�����30 ÿ��������16 �������ʾ5���־���30+16*4=94 �������ֵ����94
    private float perWidth = 16;//ÿ����һ��ֵҪ���ӵĿ��
    private float originWidth = 88f;
    public void SetInputFieldValue(int maxWeight,int perWidth,float originWidth,Vector2 originalSize)
    {
        this.maxWeight = maxWeight;
        this.perWidth = perWidth;
        this.originWidth = originWidth;
        this.originalSize = originalSize;
        this.GetComponent<RectTransform>().sizeDelta = originalSize;
    }
    

    [SerializeField]
    private RectTransform m_Rect;
    [SerializeField]
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
        SetInputFieldValue(1000, 16,100,new Vector2(100,90));
    }
    public void OnValueChanged(string v)
    {
      
        int i = (int)((LayoutUtility.GetPreferredWidth(m_Rect)) / perWidth);
        if (LayoutUtility.GetPreferredWidth(m_Rect) >= maxWeight)
        {
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, maxWeight);
        }
        else
        {
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, i <= 1 ? originWidth : originWidth + perWidth * (i - 1));
        }
        
         
    }



}