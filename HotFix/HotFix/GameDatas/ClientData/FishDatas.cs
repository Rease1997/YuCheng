using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotFix
{
    public class FishData
    {
        public string id;
        public string name;
        public string type; //类型 0近海鱼 1深海鱼
        public string image; //头像
        public string totalQuantity; //总数量
        public string isRepurchase; //是否官方回购
        public string repurchasePrice; //回购价格

        public FishData(string id, string name, string type, string image, string totalQuantity, string isRepurchase, string repurchasePrice)
        {
            this.id = id;
            this.name = name;
            this.type = type;
            this.image = image;
            this.totalQuantity = totalQuantity;
            this.isRepurchase = isRepurchase;
            this.repurchasePrice = repurchasePrice;
        }
    }

    public class FishingData
    {
        public List<FishingFishData> recordDetailList;
        public string sailDatetime;
        public string backDatetime;
        public string status;
        public string id;

        public FishingData(List<FishingFishData> recordDetailList, string sailDatetime, string status, string id, string backDatetime=null)
        {
            this.recordDetailList = recordDetailList;
            this.sailDatetime = sailDatetime;
            this.status = status;
            this.id = id;
            this.backDatetime = backDatetime;
        }
    }

    public class FishingOverData
    {
        public List<FishingFishData> recordDetailList;
        public string boatName;
        public string id;
        public string image;

        public FishingOverData(List<FishingFishData> recordDetailList, string boatName, string id, string image)
        {
            this.recordDetailList = recordDetailList;
            this.boatName = boatName;
            this.id = id;
            this.image = image;
        }
    }

    public class FishingFishData
    {
        public string varietyId; //鱼品种id
        public string fishingQuantity; //鱼捕捞数量
        public string image;

        public FishingFishData(string varietyId, string fishingQuantity, string image)
        {
            this.varietyId = varietyId;
            this.fishingQuantity = fishingQuantity;
            this.image = image;
        }
    }
}
