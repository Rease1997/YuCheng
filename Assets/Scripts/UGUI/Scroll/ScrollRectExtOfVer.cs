using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum ScrollDirOfVer
{
    Top,        //向上滑动
    Bottom,     //向下滑动
    Stoped      //停止
}
public class ScrollRectExtOfVer : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [Header("必须在Editor中初始化的变量", order = 1)]
    [Header("只支持垂直滑动，代码会强制设置", order = 2)]
    public ScrollRect scrollRect = null;
    [Header("滚动列表Item", order = 3)]
    public RectTransform scrollItemTemplate = null;
    [Header("Item列数，必须大于0", order = 5)]
    public int row = 1;
    [Header("行列间距", order = 6)]
    public Vector2 spacing = Vector2.zero;
    [Header("        ", order = 7)]
    [Header("        ", order = 8)]
    [Header("******************************************************", order = 9)]
    [Header("运行时计算中间变量", order = 10)]
    [Header("每页显示Item个数", order = 11)]
    public int itemCountPerPage = 0;
    [Header("每个Item大小", order = 16)]
    Vector2 cellSize = Vector2.one * 100;
    [Header("自动滑动时，停止滑动灵敏度", order = 12)]
    public float stopSpeedPerFre = 0;
    [Header("是否有点击", order = 13)]
    public bool isClickedDown = false;
    [Header("数据总条数", order = 14)]
    public int maxDataCount = 50;
    [Header("滑动方向", order = 15)]
    public ScrollDirOfVer scrollDirection = ScrollDirOfVer.Stoped;
    //更新Item   Transform:待更新的Item    int：更新Item对应的数据索引，从0开始
    public UnityAction<GameObject, int> onUpdateItemAction = null;
    //获取下一页数据 int：下一页数据页码，从0开始
    public UnityAction<int> onGetNextPageDataAction = null;
    //获取上一页数据 int：上一页数据页码，从0开始
    public UnityAction<int> onGetPrePageDataAction = null;
    RectTransform viewRect = null;
    Vector3[] viewRectCorners = new Vector3[4];
    public List<GameObject> itemList = new List<GameObject>();
    RectTransform content = null;

    private void Awake()
    {
        this.scrollItemTemplate.gameObject.SetActive(false);
        if (row <= 0)
        {
            row = 1;
        }

        this.viewRect = this.scrollRect.viewport.GetComponent<RectTransform>();
        this.content = this.scrollRect.content.GetComponent<RectTransform>();
        this.cellSize = new Vector2(this.scrollItemTemplate.rect.width, this.scrollItemTemplate.rect.height);
        //总条数计算
        this.itemCountPerPage = ((int)(this.viewRect.rect.height / (this.cellSize.y + this.spacing.y)) + 3) * row;
        Debug.Log("每页数量："+itemCountPerPage);        
        


    }
    /// <summary>
    /// clone每页的item数量，只能调用一次
    /// </summary>
    public void InitCountPerPage()
    {      
        this.scrollRect.elasticity = 0.05f;
        this.scrollRect.horizontal = false;
        this.scrollRect.vertical = true;
        this.scrollRect.movementType = ScrollRect.MovementType.Clamped;

        RectTransform goodsRect = null;
        Vector2 pivot = new Vector2(0, 1);
        Vector2 anchorMax = new Vector2(0, 1);
        Vector2 anchorMin = new Vector2(0, 1);
       for (int i = 0; i < itemCountPerPage; i++)
        {
            goodsRect = GameObject.Instantiate(this.scrollItemTemplate.gameObject, this.content).GetComponent<RectTransform>();
            goodsRect.gameObject.SetActive(false);
            goodsRect.pivot = pivot;
            goodsRect.anchorMax = anchorMax;
            goodsRect.anchorMin = anchorMin;
            itemList.Add(goodsRect.gameObject);
        }
        this.scrollDirection = ScrollDirOfVer.Stoped;
    }
    /// <summary>
    /// 页面关闭重置数据
    /// </summary>
    public void Init()
    {
        this.scrollRect.StopMovement();
        this.InitItems();
        this.SetMaxDataCount(0);
    }

    public void InitItems()
    {
        for (int i = 0; i < this.itemCountPerPage; i++)
        {
            var item = this.scrollRect.content.GetChild(i).GetComponent<RectTransform>();
            Debug.Log("Scroll InitItems");
            this.UpdateItem(item, i, false);
            var pos = this.scrollRect.content.anchoredPosition;
            pos = new Vector2(pos.x, 0);
            this.scrollRect.content.anchoredPosition = pos;
            
        }
    }

    Vector2 tempV2 = Vector2.zero;
    int UpdateItem(RectTransform item, int idx = -1, bool isSetSibling = true)
    {
        if (idx == -1)
        {
            if (this.scrollDirection == ScrollDirOfVer.Top)
            {
                
                idx = int.Parse(this.scrollRect.content.GetChild(this.itemCountPerPage - 1).gameObject.name) + 1;
               
            }
            else if (this.scrollDirection == ScrollDirOfVer.Bottom)
            {
                idx = int.Parse(this.scrollRect.content.GetChild(0).gameObject.name) - 1;
                
            }
        }      
         if (idx >= 0)
        {
            if (isSetSibling)
            {
                if (this.scrollDirection == ScrollDirOfVer.Top)
                {
                    item.SetAsLastSibling();
                }
                else if (this.scrollDirection == ScrollDirOfVer.Bottom)
                {
                    item.SetAsFirstSibling();
                }
            }
            item.gameObject.name = idx.ToString();
            var x = idx % row;
            var y = idx / row * -1;
            tempV2.x = (this.cellSize.x + this.spacing.x) * x;
            tempV2.y = (this.cellSize.y + this.spacing.y) * y;
            item.anchoredPosition = tempV2;
            Debug.Log("更新Item："+idx+"  最大数量："+maxDataCount);
            if (idx >= 0 && idx < this.maxDataCount)
            {
                item.gameObject.SetActive(true);
                if (this.onUpdateItemAction != null)
                {
                    this.onUpdateItemAction(item.gameObject, idx);
                }
                return idx;
            }
            else
            {
                item.gameObject.SetActive(false);
            }
        }
        return -1;
    }

    /// <summary>
    /// 更新Item所有数据
    /// </summary>
    public void UpdateAllItems()
    {
        var idx = 0;
        foreach (var item in itemList)
        {
            if (int.TryParse(item.gameObject.name, out idx))
            {
                this.UpdateItem(item.GetComponent<RectTransform>(), idx, false);
                
            }
        }
    }

    void OnGetNextPageData(int page)
    {
        if (onGetNextPageDataAction != null)
        {
            onGetNextPageDataAction(page);
        }
    }

    void OnGetLastPageData(int page)
    {
        if (onGetPrePageDataAction != null)
        {
            onGetPrePageDataAction(page);
        }
    }

    Vector3[] itemCorners = new Vector3[4];
    bool IsItemInViewRect(RectTransform item)
    {
        this.viewRect.GetWorldCorners(this.viewRectCorners);
        item.GetWorldCorners(itemCorners);
        for (int i = 0; i < 4; i++)
        {
            if (this.IsViewRectContainPoint(itemCorners[i]))
            {
                return true;
            }
        }
        return false;
    }
     bool IsViewRectContainPoint(Vector3 v3)
    {
        bool isContain = false;
        if (v3.y >= this.viewRectCorners[0].y && v3.y <= this.viewRectCorners[2].y)
        {
            isContain = true;
        }
        else
        {
            isContain = false;
        }
        return isContain;
    }
    /// <summary>
    /// 列表最大数量
    /// </summary>
    /// <param name="count"></param>
    public void SetMaxDataCount(int count)
    {
        Debug.Log("设置总数据条数：" + name + "   " + count);
        this.maxDataCount = count;
        var line = Mathf.CeilToInt(count * 1.0f / this.row);
        this.scrollRect.content.sizeDelta = new Vector2(this.content.rect.width, line * (this.cellSize.y + this.spacing.y));
        
    }

    public float lastY = -99999999;
    float minus = 0;
    RectTransform tempItem = null;
    void Update()
    {
        if (this.scrollRect == null) return;
        var v2 = this.scrollRect.content.anchoredPosition;
        if (lastY < -1000000)
        {
            lastY = v2.y;
            this.scrollDirection = ScrollDirOfVer.Stoped;
            return;
        }

        if (isClickedDown == false && Mathf.Abs(lastY - v2.y) < stopSpeedPerFre)
        {
            this.scrollRect.StopMovement();
            return;
        }
        if (lastY > -1000000)
        {
            if (lastY < v2.y)
            {
                this.scrollDirection = ScrollDirOfVer.Top;
                if (Mathf.Abs(lastY - v2.y) > 0.005)
                {
                    this.OnMoveToTop();
                }
            }
            else
            {
                this.scrollDirection = ScrollDirOfVer.Bottom;
                if (Mathf.Abs(lastY - v2.y) > 0.0001)
                {
                    this.OnMoveToBottom();
                }
            }
            lastY = v2.y;
        }
        else
        {

        }
    }
   List<RectTransform> updateItems = new List<RectTransform>();
    void OnMoveToTop()
    {
        updateItems.Clear();
        for (int i = 0; i < this.itemCountPerPage; i++)
        {
            tempItem = this.scrollRect.content.GetChild(i).GetComponent<RectTransform>();
            if (!this.IsItemInViewRect(tempItem))
            {
                updateItems.Add(tempItem);
            }
            else
            {
                break;
            }
        }

        var updateIdx = -1;
        for (int i = 0; i < updateItems.Count; i++)
        {
            tempItem = updateItems[i];
            updateIdx = this.UpdateItem(tempItem);
            if (updateIdx >= 0)
            {
                int idx = 0;
                for (int j = 0; j < 1000; j++)
                {
                    idx = this.itemCountPerPage * j;
                    if (idx > this.maxDataCount)
                    {
                        break;
                    }
                    if (updateIdx == idx)
                    {
                        this.OnGetNextPageData(updateIdx / this.itemCountPerPage);
                        break;
                    }
                }
            }
        }
    }

    void OnMoveToBottom()
    {
        updateItems.Clear();
        for (int i = this.itemCountPerPage - 1; i >= 0; i--)
        {
            tempItem = this.scrollRect.content.GetChild(i).GetComponent<RectTransform>();
            if (!this.IsItemInViewRect(tempItem))
            {
                //先缓存再更新：更新里面有设置tempItem的sibling值，这会导致上面GetChild不准确
                updateItems.Add(tempItem);
            }
            else
            {
                break;
            }
        }

        var updateIdx = -1;
        for (int i = 0; i < updateItems.Count; i++)
        {
            tempItem = updateItems[i];
             updateIdx = this.UpdateItem(tempItem);
            if (updateIdx >= 0)
            {
                int idx = 0;
                for (int j = 0; j < 1000; j++)
                {
                    idx = j * this.itemCountPerPage;
                    if (idx > this.maxDataCount)
                    {
                        break;
                    }
                    if (updateIdx == idx)
                    {
                        //DebugManager.Log("获取上一页数据：" + updateIdx / this.itemCount + "  updateIdx" + updateIdx);
                        this.OnGetLastPageData(updateIdx / this.itemCountPerPage);
                        break;
                    }
                }
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        lastY = -99999999;
        this.isClickedDown = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        lastY = -99999999;
        this.scrollDirection = ScrollDirOfVer.Stoped;
        this.isClickedDown = false;
    }
}
