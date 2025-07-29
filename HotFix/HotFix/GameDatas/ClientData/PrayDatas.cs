using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotFix
{
    public class PrayData
    {
        public string id; //ID
        public string image; //图片
        public string name; //名称 
        public string status; //状态 {0:待上架,1:已上架,2:已下架}
        public string type;
        public string price;
        public PrayData(string image, string name, string status, string id = null, string type = null, string price = null)
        {
            this.image = image;
            this.name = name;
            this.status = status;
            this.id = id;
            this.type = type;
            this.price = price;
        }
    }

    public class PrayInfoData
    {
        public string nickName; //用户昵称
        public string propName; //道具名称 

        public PrayInfoData(string nickName, string propName)
        {
            this.nickName = nickName;
            this.propName = propName;
        }
    }
}
