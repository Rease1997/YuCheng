using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchScreenCamera : MonoBehaviour
{
    [Header("将观察的目标位置放进来")]
    public  Transform target;
    [Header("第一人称距离")]
    public float firstPerDis;
    [Header("第一人称视野")]
    public float firstPerFov = 40;
    [Header("第三人称距离")]
    public float thirdPerDis = 3;
    [Header("第三人称视野")]
    public float thirdPerFov = 50;

    [Header("是否按了移动键")]
    public bool isMovingButtonPressed = false;

    [Header("观察距离")]
    //这个值必须一开始就设定好
    public float distance = 3f;

    [Header("这个调整的是旋转速度")]
    public float xSpeed = 10.0f;
    public float ySpeed = 8.0f;

    [Header("这个是调整可观测角度的")]
    public float yMinLimit = -30f;
    public float yMaxLimit = 360f;

    [Header("这个是碰撞到障碍物后向目标缩进到什么范围")]
    public float zoomSpeed = 0.5f;      // 比如我这边一开始设置的和目标相距 8 个单位，碰到障碍物后缩进到 0.5 个单位，没有障碍物就恢复正常

    [Header("相机旋转角度Y")]
    public float x=0;
    [Header("相机旋转角度X")]
    public float y=90;

    private float fx = 0f;
    private float fy = 0f;
    private float fDistance = 0;


    int m_fingerId = -1; //  当摇杆移动，控制镜头的手指
    bool m_isClickUi = false;
    Dictionary<int, bool> m_dicTouch = new Dictionary<int, bool>(); //key 为fingerid ，value为是否接触到UI
    float t = 0.2f;
    public float m_minDis = 1.0f;
    public bool m_isNear = false;

    public string ignoreTag;
    public string ignoreTag01;


    // Use this for initialization
    void Start()
    {
        //target = GameObject.FindWithTag("PlayerTarget").transform
;        Vector3 angles = transform.eulerAngles;
        x += angles.y;
        y = angles.x;
        fx = x;
        fy = y;
        UpdateRotaAndPos();
        fDistance = distance;
    }
    void Update()
    {
        if (Input.touchCount == 0)//当前没有手指,全部初始化
        {
            m_fingerId = -1;
            m_dicTouch.Clear();
        }

        if (Input.touchCount == 1)
        {            
            // 不然按 移动的时候 也会转向
            if (isMovingButtonPressed == true)
            {
                Debug.Log("Archer: i am moving");
                return;
            }

            List<int> deleteFinger = new List<int>();
            foreach (var item in m_dicTouch)
            {
                if (item.Key != Input.GetTouch(0).fingerId)
                {
                    deleteFinger.Add(item.Key);
                }
            }
             
            for (int i = 0; i < deleteFinger.Count; i++)
            {
                
                m_dicTouch.Remove(deleteFinger[i]);
            }
            switch (Input.GetTouch(0).phase)
            {
                case TouchPhase.Began:
                    if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                    {
                        
                        m_dicTouch[Input.GetTouch(0).fingerId] = true; //如果按在ui上为true
                    }
                    else
                    {
                         
                        m_dicTouch[Input.GetTouch(0).fingerId] = false;
                    }
                    break;
                case TouchPhase.Moved:

                    break;
                case TouchPhase.Stationary:
                    break;
                case TouchPhase.Ended:
                    break;
                case TouchPhase.Canceled:
                    break;
                default:
                    break;
            }
          
        }
        if (Input.touchCount == 2)
        {
            List<int> deleteFinger = new List<int>();
            foreach (var item in m_dicTouch)
            {
                bool isDelete = true;
                for (int i = 0; i < Input.touchCount; i++)
                {
                    if (item.Key == Input.touches[i].fingerId)
                    {
                        isDelete = false;
                        break;
                    }
                }
                if (isDelete == true)
                {
                    deleteFinger.Add(item.Key);
                }
            }
            //删除m_dicTouch里面非现在按下的两个手指--》得到的结果：m_dicTouch里有一个现在按下手指中的0-2个
            for (int i = 0; i < deleteFinger.Count; i++)
            {
                //DebugManager.Log("双指：删除：" + deleteFinger[i]);
                m_dicTouch.Remove(deleteFinger[i]);
            }
            for (int i = 0; i < Input.touchCount; i++)
            {
                switch (Input.touches[i].phase)
                {
                    case TouchPhase.Began:
                        if (EventSystem.current.IsPointerOverGameObject(Input.touches[i].fingerId))
                        {
                            //DebugManager.Log("双指：增加true：" + Input.touches[i].fingerId);
                            m_dicTouch[Input.touches[i].fingerId] = true;
                        }
                        else
                        {
                            //DebugManager.Log("双指：增加false：" + Input.touches[i].fingerId);
                            m_dicTouch[Input.touches[i].fingerId] = false;
                        }
                        break;
                    case TouchPhase.Moved:
                        break;
                    case TouchPhase.Stationary:
                        break;
                    case TouchPhase.Ended:
                        break;
                    case TouchPhase.Canceled:
                        break;
                    default:
                        break;
                }
            }
        }
        //distance = Mathf.Lerp(distance, fDistance, 0.25f);
    }
    void LateUpdate()
    {
        if (Application.isMobilePlatform)
        {
            if (Input.GetMouseButton(0) && IsCanRotate())
            {
                Touch input = new Touch();
                for (int i = 0; i < Input.touchCount; i++)
                {
                    if (Input.touches[i].fingerId == m_fingerId)
                    {
                        input = Input.touches[i];//找到控制镜头移动的手指
                    }
                }
                if (target)
                {
                    float dx = Input.GetAxis("Mouse X");
                    float dy = Input.GetAxis("Mouse Y");
                    if (Input.touchCount > 0)
                    {
                        dx = input.deltaPosition.x;
                        dy = input.deltaPosition.y;
                    }

                    x += dx * xSpeed * Time.deltaTime;//*distance
                    y -= (dy) * ySpeed * Time.deltaTime;
                    y = ClampAngle(y, yMinLimit, yMaxLimit);
                  
                }
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (EventSystem.current.IsPointerOverGameObject())
                {
                    m_isClickUi = true;
                }
            }
            if (Input.GetMouseButtonUp(0))
            {
                m_isClickUi = false;
            }
            if (m_isClickUi == false && Input.GetMouseButton(0))
            {
                if (target)
                {
                    float dx = Input.GetAxis("Mouse X");
                    float dy = Input.GetAxis("Mouse Y");


                    x += dx * xSpeed * 20 * Time.deltaTime;//*distance
                    y -= dy * ySpeed * 20 * Time.deltaTime;

                    y = ClampAngle(y, yMinLimit, yMaxLimit);
                   
                }
            }
        }
        fx = Mathf.Lerp(fx, x, 0.2f);
        fy = Mathf.Lerp(fy, y, 0.2f);

        UpdateRotaAndPos();



        //Debug.Log("isMovingButtonPressed: " + isMovingButtonPressed);
    }
    bool m_isLastHit = false;
    void UpdateRotaAndPos()
    {
        if (target)
        {           
            bool isCurHit = false;
            Quaternion rotation = Quaternion.Euler(fy, fx, 0);
            Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);
            Vector3 position = rotation * negDistance + target.position; //摄像头的位置为 移动后角度*距离 + 目标位置
            Vector3 cameraCurrrentPosition = position;
            RaycastHit hit;
            if (Physics.Linecast(target.position, position, out hit))
            {
                

                //当碰撞的不是摄像机也不是地形 那么直接移动摄像机的坐标
                 if (hit.collider.CompareTag(ignoreTag) == false && hit.collider.CompareTag(ignoreTag01) == false)
                {
                    Vector3 posHit = hit.point;
                    Vector3 dir = posHit - target.position;
                    Vector3 dis = hit.point + this.transform.forward * 0.3f;
                    cameraCurrrentPosition = Vector3.Lerp(hit.point, dis, Time.deltaTime * 3);
                    //canSetPos = posHit - dir * 0.5f;
                    isCurHit = true;
                }


            }
            ////摄像机与地面的检测，不要穿过地
            if (cameraCurrrentPosition.y < target.parent.position.y)
            {
                cameraCurrrentPosition.y = target.parent.position.y;
                Vector3 relativePos = target.position - cameraCurrrentPosition;
                rotation = Quaternion.LookRotation(relativePos);
                isCurHit = true;
            }
            this.transform.position = cameraCurrrentPosition;
            this.transform.rotation = rotation;

            m_isLastHit = isCurHit;
        }
    }
    float movementLerpSpeed = 0.1f;
    float currentDeltaTime;
    public float getCurrentDeltaTime()
    {
        currentDeltaTime = Time.deltaTime;
        return currentDeltaTime;
    }
    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360F)
            angle += 360F;
        if (angle > 360F)
            angle -= 360F;
        return Mathf.Clamp(angle, min, max);
    }
    /// <summary>
    /// 是否可以选择
    /// 
    /// </summary>
    /// <returns></returns>
    bool IsCanRotate() //这里做是否可以移动摄像机的更改，在 有UI 界面打开的时候 3.2版本
    {
        bool ret = false;
        if (m_dicTouch.Count == 1) //只有一个手指按下，并且没按在UI上  ???? 这里没有进行 没按在UI上的 计算啊？？
        {
            foreach (var item in m_dicTouch)
            {
                if (item.Value == false)
                {
                    ret = true;
                    m_fingerId = item.Key;
                }
            }
        }
        else
        {
            //当有两个手指按下，一个手指在UI（包含在UI摇杆）上，一个手指没在，可以移动镜头 
            int inUI = 0;
            int outUI = 0;
            foreach (var item in m_dicTouch)
            {
                if (item.Value == true)
                {
                    inUI++;
                }
                else
                {
                    outUI++;
                    m_fingerId = item.Key;
                }
            }
            if (inUI == 1 && outUI == 1)
            {
                ret = true;
            }
        }
        return ret;
    }
    //何时才能双指控制缩放-->两个手指都没按在UI上
    bool IsCanScale()
    {
        bool ret = true;
        if (m_dicTouch.Count == 2)
        {
            foreach (var item in m_dicTouch)
            {
                if (item.Value == true)
                {
                    ret = false;
                    break;
                }
            }
        }
        return ret;
    }

    public void SetMoveState(bool state)
    {
        isMovingButtonPressed = state;
    }
}