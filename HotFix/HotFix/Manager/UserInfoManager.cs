using HotFix.Common;
using LitJson;
using StarterAssets;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace HotFix
{
  
    /// <summary>
    /// 用户信息管理类
    /// </summary>
    public static class UserInfoManager
    {
        public static float LoadNum=0;
        public static Vector3 playerScenePos;
        public static Quaternion playerSceneRotation;
        public static Vector3 playerBirthPos;
        public static ThirdPersonController selectPlayer;
        public static Animator fishAnimator;
        public static Transform playerRoot;
        public static string playerType;//选择的角色类型 0:岛民,1:渔民,2:商人
        public static string playerSex;//选择的角色性别
        public  static string palyerStatus;//状态 0:无角色,1:岛民,2:渔民,3:商人,4:全部
        internal static float yuBeiNum;
        internal static float prayCostNum;
        internal static bool refreshBag;
        public static List<BoatData> boatDataList=new List<BoatData>();
        public static bool isFirstInGame=true;//TODO IOS退出重置
        public static List<GoodsItem> goodsItemList = new List<GoodsItem>(8);//3D展台
        public static List<GoodsItem> goods2DItemList = new List<GoodsItem>(6);//2D展台

        public static List<GoodsSelectItem> goodsSelectItemList = new List<GoodsSelectItem>();//3D藏品列表
        public static TranslateType curTrasnlateType=TranslateType.IsLand;
        public static void Init()
        {
        }
    }
}
