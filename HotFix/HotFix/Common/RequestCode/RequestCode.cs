using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotFix.Common 
{
   
    public enum RequestCode
    {
        /// <summary>
        /// //"进入大厅"
        /// </summary>
        JoinHall,
        /// <summary>
        /// "新增大厅房间"
        /// </summary>
        CreateRoomInHall, 
        /// <summary>
        /// "更新大厅房间"
        /// </summary>
        UpdateRoomInHall,
        /// <summary>
        /// "更新房间信息"
        /// </summary>
        UpdateRoom,
        /// <summary>
        /// "进入房间"
        /// </summary>
        JoinRoom,
        /// <summary>
        /// 准备
        /// </summary>
        UserReady,
        /// <summary>
        /// 取消准备
        /// </summary>
        CancelReady,
        /// <summary>
        /// 提醒用户准备
        /// </summary>
        RemindReady,
        /// <summary>
        /// "离开房间"
        /// </summary>
        LeaveRoom,
        /// <summary>
        /// 解散房间
        /// </summary>
        CloseRoom,
        /// <summary>
        /// 房间开始
        /// </summary>
        RoomStart,
        /// <summary>
        /// 加载完成
        /// </summary>
        LoadFinish,
        /// <summary>
        /// 比赛开始
        /// </summary>
        GameStart,
        /// <summary>
        /// 倒计时
        /// </summary>
        GameCountdown,
        /// <summary>
        /// 马跑到终点
        /// </summary>
        MatchEnd,
        /// <summary>
        /// 游戏结束
        /// </summary>
        GameEnd,
        /// <summary>
        /// 玩家被踢出房间
        /// </summary>
        MandatoryExit,
        /// <summary>
        /// 加入房间
        /// </summary>
        JoinScene,
        QueueUpdate,//繁育场衍生


    }
   
    

     
}
