using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UI;
using UnityEngine;

namespace HotFix
{
    public enum GoodsType
    {
        Goods2D,
        Goods3D,
    }
    public class GoodsItem
    {

        private Transform mTrans, despawnParent;
        private GameObject exhibition;
        private GameObject ship1, ship2, ship3, ship4, ship5;
        private string curType;
        private bool hasPut;
        private TriggerEvent triggerEvent;
        private GameObject uploadBtn, unloadBtn;
        private MeshRenderer meshRenderer;
        public int pos;
        public string id;
        public string collectId;
        public GoodsType goodsType;
        public Texture blankTexture;

        public void OnInit(Transform itemTrans, Transform btns,int _pos,GoodsType type,Texture texture)
        {
            mTrans = itemTrans;            
            this.pos = _pos;
            this.goodsType = type;
            Debug.Log("GoodItem name:"+this.mTrans.name+"  type:"+this.goodsType);
            if(type == GoodsType.Goods2D)
            {
                meshRenderer = this.mTrans.GetComponent<MeshRenderer>();
                blankTexture = texture;
            }
            Debug.Log("GoodItem init 222");
            uploadBtn = btns.GetChild(0).gameObject;
            unloadBtn = btns.GetChild(1).gameObject;
            Debug.Log("GoodItem init 333");
            GetAllComponents();
            Debug.Log("GoodItem init 444");
            triggerEvent.TriggerEnter = OnTriggerEnter;
            triggerEvent.TriggerExit = OnTriggerExit;
            
        }
        public string Name
        {
            get { return this.mTrans.name; }
        }
        private void GetAllComponents()
        {
            triggerEvent = mTrans.GetComponent<TriggerEvent>();
            despawnParent = mTrans.transform.parent.parent.Find("SpawnParent").transform;
            hasPut = false;
            Transform shipParent = mTrans.transform.parent.parent.Find("ShipGroup");
            ship1 = shipParent.GetChild(0).gameObject;
            ship2 = shipParent.GetChild(1).gameObject;
            ship3 = shipParent.GetChild(2).gameObject;
            ship4 = shipParent.GetChild(3).gameObject;
            ship5 = shipParent.GetChild(4).gameObject;
           
        }
        public void ShowGoods(string type,string _id,string _collectId,string fileUrl=null)
        {        
            curType = type;            
            this.id = _id;
            this.collectId = _collectId;
            hasPut = true;
            Debug.Log("显示3D藏品，藏品类型："+type+"藏品id："+_id);
            if(goodsType==GoodsType.Goods2D)
            {
                meshRenderer.material.mainTexture = null;
                //WebRequestManager.instance.AsyncLoadUnityTexture(fileUrl, (texture) =>
                //{
                //    if (meshRenderer.material.mainTexture != null)
                //    {
                //        if (!meshRenderer.material.mainTexture.name.Equals(blankTexture.name))
                //        {
                //            ToolManager.DestoryDownLoadTexture(meshRenderer.material.mainTexture);

                //        }
                //      }
                //    meshRenderer.material.mainTexture = texture;
                //});
                
                return;
            }
            exhibition = GetShipByType();
            if (exhibition != null)
            {
                exhibition.transform.parent = mTrans;
                exhibition.transform.localPosition = Vector3.zero;
                exhibition.transform.localRotation = Quaternion.identity;
                exhibition.transform.localScale = Vector3.one;
                exhibition.gameObject.SetActive(true);

            }
            else
            {
                RFrameWork.instance.OpenCommonConfirm("提示", "藏品不存在", () => { }, null);
            }

        }
        public void RemoveGoods()
        {
            if (hasPut)
            {
                if(goodsType==GoodsType.Goods3D)
                {
                    if (exhibition != null)
                    {
                        exhibition.transform.parent = despawnParent;
                        exhibition.gameObject.SetActive(false);
                        exhibition = null;
                        
                    }
                }
                else
                {
                    if (meshRenderer.material.mainTexture != null)
                    {
                        Debug.Log("now name:"+ meshRenderer.material.mainTexture+"  blank:"+ blankTexture.name);
                        if(!meshRenderer.material.mainTexture.name.Equals(blankTexture.name))
                        {
                            ToolManager.DestoryDownLoadTexture(meshRenderer.material.mainTexture);

                        }
                    }
                    meshRenderer.material.mainTexture = blankTexture;
                     
                }
                curType = "";
                hasPut = false;
                this.id = "-1";
                this.collectId = "-1";

            }
          
        }
        private GameObject GetShipByType()
        {
            if (despawnParent.childCount > 0)
            {
                int count = despawnParent.childCount;
                for (int i = 0; i < count; i++)
                {
                    GameObject go = despawnParent.GetChild(i).gameObject;
                    string removeName = go.name.Replace("Ship", "");
                    if (removeName.Equals(curType))
                    {
                        return go;
                    }
                }
            }
            if (curType.Equals("1"))
            {
                GameObject shipClone = GameObject.Instantiate(ship1);
                shipClone.name = "Ship1";
                return GameObject.Instantiate(ship1);
            }
            else if (curType.Equals("2"))
            {
                GameObject shipClone = GameObject.Instantiate(ship2);
                shipClone.name = "Ship2";
                return shipClone;
            }
            else if (curType.Equals("3"))
            {
                GameObject shipClone = GameObject.Instantiate(ship3);
                shipClone.name = "Ship3";
                return shipClone;
            }
            else if (curType.Equals("4"))
            {
                GameObject shipClone = GameObject.Instantiate(ship4);
                shipClone.name = "Ship4";
                return shipClone;
            }
            else if (curType.Equals("5"))
            {
                GameObject shipClone = GameObject.Instantiate(ship5);
                shipClone.name = "Ship5";
                return shipClone;
                
            }
            else
            {
                return null;
            }


        }
        public bool HasPut
        {
            get
            {
                return hasPut;
            }
            set { hasPut = value; }

        }
        private void OnTriggerEnter(Collider col)
        {
            if (col.CompareTag("Player"))
            {
                if (hasPut)
                {
                    unloadBtn.gameObject.SetActive(true);
                    uploadBtn.gameObject.SetActive(false);
                }
                else
                {
                    uploadBtn.gameObject.SetActive(true);
                    unloadBtn.gameObject.SetActive(false);
                }
                JoyStickWindow.ExhibitionAction(this);
            }
        }
        private void OnTriggerExit(Collider col)
        {
            if (col.CompareTag("Player"))
            {
                uploadBtn.gameObject.SetActive(false);
                unloadBtn.gameObject.SetActive(false);

            }
        }

    }
}
