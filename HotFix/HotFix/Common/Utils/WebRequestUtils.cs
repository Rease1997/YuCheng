using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotFix.Common.Utils
{
    public class WebRequestUtils
    {
        private static bool hasInit;
        

        public static string myToken = "";

        public static string checkRole = "fish_role/check_role";//是否选择人物 选的谁 1  测试完成
        public static string listFront = "fish_pond_user/list_front";//我的鱼货 1        鱼获列表测试完成
        public static string detailFront = "fish_boat_user/detail_front";//船只详情 1   测试完成 待测试多船只情况
        public static string pageFront = "fish_sail_record/page_front";//捕鱼记录 1     测试完成待测试多船只情况
        public static string recordPageFront = "fish_sail_record/list_front";//捕鱼完成 1      
        public static string enterTheMeta = "fish_user_log/enter_the_meta";//选择人物进入游戏 1    测试完成
        public static string fishPondSale = "fish_pond_sale_record/create";//卖鱼 1                测试并修改完成
        public static string fishPondUser = "fish_pond_user/detail_front";//查询余额 1             测试完成
        public static string getUserPhoto = "cuser/get_user_photo";//通过手机号查询用户名称 1  赠送鱼呗手机是否存在  测试完成
        public static string getAllBoatData = "fish_boat_user/list_front";//所有船只             测试完成需要测试多个船只的情况
        public static string startCatchFishUrl = "fish_sail_record/create";//开始捕鱼           测试完成      

        public static string reCordCreate = "fish_transfer_record/create";//赠送渔贝 1           测试并修改完成
        public static string jourMyPage = "jour/my/page_shell";//账单 入参currency = shell 1   鱼呗明细列表测试完成
        public static string propPageFront = "fish_pray_prop/list_front";//祈福道具详情 1  
        public static string buyOrderCreate = "fish_buy_order/create";//购买祈福道具 1
        public static string prayTreeListUrl = "fish_pray_tree_position/list_front";//祈福树列表 
        public static string prayScrollUrl = "fish_pray_tree_position/scroll_bar";//祈福树滚动条 1
        public static string firstEnterGoodsUrl = "fish_user_exhibits/list_front";//个人馆藏品，第一次进入 1
        public static string addExhibitionUrl = "fish_user_exhibits/create";//个人馆藏品，放置藏品
        public static string removeExhibitionUrl = "fish_user_exhibits/cancel";//个人馆藏品，取下藏品
        public static string exhibitionListUrl = "collection_detail/page_person_front_u3d";//个人馆藏品UI列表//"fish_boat_user/user_list"
        public static string dailyRewardUrl = "shell_task_config/entry_meta_universe";//在线时长
        
        public static void InitUrl(string front,string token) { 
        
            if(hasInit)return;

            checkRole = front + checkRole;
            listFront = front + listFront;
            detailFront = front + detailFront;
            pageFront = front + pageFront;
            recordPageFront = front + recordPageFront;
            enterTheMeta = front + enterTheMeta;
            reCordCreate = front + reCordCreate;
            jourMyPage = front + jourMyPage;
            propPageFront = front + propPageFront;
            buyOrderCreate = front + buyOrderCreate;
            prayTreeListUrl = front + prayTreeListUrl;
            fishPondSale = front + fishPondSale;
            fishPondUser = front + fishPondUser;
            getUserPhoto = front + getUserPhoto;
            getAllBoatData = front + getAllBoatData;
            prayScrollUrl = front + prayScrollUrl;
            startCatchFishUrl= front + startCatchFishUrl;
            firstEnterGoodsUrl= front + firstEnterGoodsUrl;
            addExhibitionUrl= front + addExhibitionUrl;
            removeExhibitionUrl = front + removeExhibitionUrl;
            exhibitionListUrl= front + exhibitionListUrl;
            dailyRewardUrl= front + dailyRewardUrl;
            myToken = token;
            hasInit = true;
        }

    }
}
