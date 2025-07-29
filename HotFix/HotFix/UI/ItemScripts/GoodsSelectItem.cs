using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UI;
using UnityEngine;

namespace HotFix
{
    public class GoodsSelectItem
    {

        public Transform mTrans;
        private RawImage iconImg;
        private Text nameTxt,idTxt, btnTxt;
        private Button putBtn;
        private Image btnImg;
        private GameObject type2D, type3D ;
        private string goodsName;
        private string goodsId;
        private string collectId;
        private string orderNum;
        private string url;
        private string status;
        private string file2d;//0是2d 1是3d
        private string boatType;
        private Color grayColor=new Color(1,1,1,0.6f);
        private Color originColor = new Color(0.13f,0.267f,0.259f,1f);
        private Color textGrayColor = new Color(0.13f, 0.267f, 0.259f, 0.6f);
        public bool isShowing;

        public void OnInit(Transform itemTrans, GoodsSelectData goodsData)
        {
            mTrans = itemTrans;
            GetAllComponents();
            UpdateUI(goodsData);
            putBtn.onClick.RemoveAllListeners();
            putBtn.onClick.AddListener(OnShowExhibition);
        }
        private void GetAllComponents()
        {
            iconImg = this.mTrans.Find("Icon").GetComponent<RawImage>();
            nameTxt = this.mTrans.Find("Name").GetComponent<Text>();
            idTxt = this.mTrans.Find("ID").GetComponent<Text>();
            putBtn  = this.mTrans.Find("ShowBtn").GetComponent<Button>();
            btnImg = this.mTrans.Find("ShowBtn").GetComponent<Image>();
            btnTxt= this.mTrans.Find("ShowBtn/Text").GetComponent<Text>();
            type2D = this.mTrans.Find("Type2D").gameObject;
            type3D = this.mTrans.Find("Type3D").gameObject;
        }
        public string GoodName
        {
            get { return this.goodsName; }
        }
        public void UpdateUI(GoodsSelectData goodsData)
        {
            this.mTrans.gameObject.SetActive(true);
            this.mTrans.localScale = Vector3.one;
            this.isShowing = true;
            this.goodsId = goodsData.id;
            Debug.Log("goodsId:"+goodsData.id);
            this.collectId = goodsData.collectionId;
            this.goodsName = goodsData.name;
            this.url = goodsData.coverFileUrl;
            this.status = goodsData.showFlag;
            this.file2d = goodsData.u3dFlag;
            this.orderNum = goodsData.tokenId;
            if(string.IsNullOrEmpty(goodsData.boatType))
            {
                this.boatType = "-1";
            }
            else
            {
                this.boatType = (int.Parse(goodsData.boatType) + 1).ToString();
            }
            
            this.nameTxt.text = this.goodsName;
            this.idTxt.text="#"+ orderNum;
            TextExtensionUtils.SetTextWithEllipsis(this.idTxt, this.idTxt.text, 17);
            //TextExtensionUtils.SetTextWithEllipsis(this.nameTxt, this.nameTxt.text, 6);
            iconImg.texture = null;
            //WebRequestManager.instance.AsyncLoadUnityTexture(this.url, (tx) =>
            //{
            //    if (iconImg.texture != null)
            //    {
            //        ToolManager.DestoryDownLoadTexture(iconImg.texture);
            //    }
            //    iconImg.texture = tx;
            //});
            type2D.gameObject.SetActive(file2d.Equals("0"));
            type3D.gameObject.SetActive(file2d.Equals("1"));
            if (status.Equals("1"))
            {
                this.btnImg.GetComponent<Image>().color = grayColor;
                this.btnTxt.color = textGrayColor;
                this.putBtn.enabled = false;
                this.btnTxt.text = "展示中";
            }
            else
            {
                
                this.putBtn.enabled = true;
                this.btnImg.GetComponent<Image>().color = Color.white;
                this.btnTxt.color = originColor;
                this.btnTxt.text = "展示";
            }

        }
        private void OnShowExhibition()
        {
            if(JoyStickWindow.ShowExhibition3DAction!=null)
            {
                if(file2d.Equals("1"))
                {
                    JoyStickWindow.ShowExhibition3DAction(this.boatType, this.goodsId, this.collectId, null);

                }
                else
                {
                    JoyStickWindow.ShowExhibition3DAction(this.boatType, this.goodsId, this.collectId, this.url);

                }

            }
            else
            {
                Debug.LogError("3D藏品不能展示委托为空");
            }
        }
        public void OnHideUI()
        {
            this.isShowing=false;
            this.mTrans.gameObject.SetActive(false);
             
        }

    }
}
