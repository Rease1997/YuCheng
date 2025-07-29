using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotFix
{
    public class BoatData
    {
        
        public string id;
        public string name;
        public string type; //类型  {0:小渔船,1:钓鱼船,2:远洋舰}
        public string regionType; // 捕鱼区域 {0:不可捕鱼,1:近海捕鱼,2:远海捕鱼}
        public string speed;
        public string enduranceMileage; //续航里程
        public string getTime; //获得时间
        public string remainFishingNum; //剩余捕鱼次数
        public string deadweight; //载重量
        public string sailNumMonth; //月度出海次数
        public string status; //状态 {0空闲，1捕鱼中}
        public BoatData(string id, string name, string type, string regionType, string speed, string enduranceMileage, string getTime, string remainFishingNum, string deadweight, string sailNumMonth, string status=null)
        {
            this.id = id;
            this.name = name;
            this.type = type;
            this.regionType = regionType;
            this.speed = speed;
            this.enduranceMileage = enduranceMileage;
            this.getTime = getTime;
            this.remainFishingNum = remainFishingNum;
            this.deadweight = deadweight;
            this.sailNumMonth = sailNumMonth;
            this.status = status;
        }
    }
}
