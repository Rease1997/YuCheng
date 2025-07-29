using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureController : MonoBehaviour
{
    [SerializeField] private Renderer render;


    [SerializeField] private Vector2 textureSpeed;



    //----------------------------
    /// <summary>
    /// ��˸��ɫ
    /// </summary>
    public Color color = new Color(1, 0, 1, 1);
    /// <summary>
    /// ��ͷ������ȣ�ȡֵ��Χ[0,1]����С����߷������ȡ�
    /// </summary>
    [Range(0.0f, 1.0f)]
    public float minBrightness = 0.0f;
    /// <summary>
    /// ��߷������ȣ�ȡֵ��Χ[0,1]���������ͷ������ȡ�
    /// </summary>
    [Range(0.0f, 5)]
    public float maxBrightness = 1f;
    /// <summary>
    /// ��˸Ƶ�ʣ�ȡֵ��Χ[0.2,30.0]��
    /// </summary>
    [Range(0.2f, 30.0f)]
    public float rate = 1;

    [Tooltip("��ѡ����������ʱ�Զ���ʼ��˸")]
    [SerializeField]
    private bool _autoStart = false;

    private float _h, _s, _v;           // ɫ�������Ͷȣ�����
    private float _deltaBrightness;     // ���������Ȳ�
    [SerializeField] private Renderer _renderer;
    [SerializeField] private Material _material;
    private readonly string _keyword = "_EMISSION";
    private readonly string _colorName = "_EmissionColor";

    private Coroutine _glinting;
    //----------------------------------------------------------------




    private void Awake()
    {
        //render.GetComponent<Renderer>();
    }

    private void Start()
    {
        _renderer = gameObject.GetComponent<Renderer>();
        _material = _renderer.material;

        if (_autoStart)
        {
            StartGlinting();
        }
    }

    /// <summary>
    /// У�����ݣ�����֤����ʱ���޸��ܹ��õ�Ӧ�á�
    /// �÷���ֻ�ڱ༭��ģʽ����Ч������
    /// </summary>
    private void OnValidate()
    {
        // �������ȷ�Χ
        if (minBrightness < 0 || minBrightness > 1)
        {
            minBrightness = 0.0f;
            Debug.LogError("������ȳ���ȡֵ��Χ[0, 1]��������Ϊ0��");
        }
        if (maxBrightness < 0 || maxBrightness > 5)
        {
            maxBrightness = 1.0f;
            Debug.LogError("������ȳ���ȡֵ��Χ[0, 5]��������Ϊ1��");
        }
        if (minBrightness >= maxBrightness)
        {
            minBrightness = 0.0f;
            maxBrightness = 1.0f;
            Debug.LogError("�������[MinBrightness]��������������[MaxBrightness]���ѷֱ�����Ϊ0/1��");
        }

        // ������˸Ƶ��
        if (rate < 0.2f || rate > 30.0f)
        {
            rate = 1;
            Debug.LogError("��˸Ƶ�ʳ���ȡֵ��Χ[0.2, 30.0]��������Ϊ1.0��");
        }

        // �������Ȳ�
        _deltaBrightness = maxBrightness - minBrightness;

        // ������ɫ
        // ע�ⲻ��ʹ�� _v ������������ʱ�޸Ĳ����ᵼ������ͻ��
        float tempV = 0;
        Color.RGBToHSV(color, out _h, out _s, out tempV);
    }

    /// <summary>
    /// ��ʼ��˸��
    /// </summary>
    public void StartGlinting()
    {
        _material.EnableKeyword(_keyword);

        if (_glinting != null)
        {
            StopCoroutine(_glinting);
        }
        _glinting = StartCoroutine(IEGlinting());
    }

    /// <summary>
    /// ֹͣ��˸��
    /// </summary>
    public void StopGlinting()
    {
        _material.DisableKeyword(_keyword);

        if (_glinting != null)
        {
            StopCoroutine(_glinting);
        }
    }

    /// <summary>
    /// �����Է���ǿ�ȡ�
    /// </summary>
    /// <returns></returns>
    private IEnumerator IEGlinting()
    {
        Color.RGBToHSV(color, out _h, out _s, out _v);
        _v = minBrightness;
        _deltaBrightness = maxBrightness - minBrightness;

        bool increase = true;
        while (true)
        {
            if (increase)
            {
                _v += _deltaBrightness * Time.deltaTime * rate;
                increase = _v <= maxBrightness;
            }
            else
            {
                _v -= _deltaBrightness * Time.deltaTime * rate;
                increase = _v <= minBrightness;
            }
            _material.SetColor(_colorName, Color.HSVToRGB(_h, _s, _v));
            //_renderer.UpdateGIMaterials();
            yield return null;
        }
    }

    private void Update()
    {
        if (render != null)
        {
            render.material.mainTextureOffset = Time.time * textureSpeed;
        }

    }

}
