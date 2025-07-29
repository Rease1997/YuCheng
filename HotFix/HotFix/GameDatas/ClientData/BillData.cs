using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotFix
{

    public class BillData
    {
        public string bizCategory;
        public string bizCategoryNote; //名字1
        public string bizNote;  //名字2
        public string bizType;
        public string createDatetimeStr;
        public string id;
        public string transAmount; //变动金额
        public string remark;

        public BillData(string bizCategory, string bizCategoryNote, string bizNote, string bizType, string createDatetimeStr, string id, string transAmount, string remark=null)
        {
            this.bizCategory = bizCategory;
            this.bizCategoryNote = bizCategoryNote;
            this.bizNote = bizNote;
            this.bizType = bizType;
            this.createDatetimeStr = createDatetimeStr;
            this.id = id;
            this.transAmount = transAmount;
            this.remark = remark;   
        }
    }

    public class GoodsSelectData
    {
        public string id;
        public string boatType;
        public string collectionId;
        public string u3dFlag;//是否元宇宙展示3d 1是 0否  
        public string name;
        public string coverFileUrl;
        public string fileType;
        public string showFlag;    
        public string tokenId;
        public GoodsSelectData(string id, string tokenId, string boatType = null, string collectionId = null, string name = null, string showFlag = null, string coverFileUrl = null, string fileType = null, string u3dFlag = null)
        {
            this.id = id;
            this.tokenId = tokenId;
            this.boatType = boatType;
            this.collectionId = collectionId;
            this.name = name;
            this.showFlag = showFlag;
            this.coverFileUrl = coverFileUrl;
            this.fileType = fileType;
            this.u3dFlag = u3dFlag;
        }
    }
    public class GoodsData
    {
        public string collectionDetailId;
        public string boatType;
        public string position;
        public string collectionId;
        public string u3dFlag;
        public string coverFileUrl;


        public GoodsData(string collectionDetailId, string position = null, string boatType = null,string collectionId = null, string u3dFlag = null, string coverFileUrl = null)
        {
            this.collectionDetailId = collectionDetailId;
            this.boatType = boatType;
            this.position = position;
            this.collectionId = collectionId;
            this.u3dFlag = u3dFlag;
            this.coverFileUrl = coverFileUrl;
        }
    }
}
