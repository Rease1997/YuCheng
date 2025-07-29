using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace HotFix
{
    public static class HouseManager
    {
        internal static void LoadMainScene(Action obj)
        {
            Dictionary<int, AllPrefabsData> prefabDatas = JsonConfigManager.GetPrefabsData();
            int allCount = 0;
            foreach (var tableName in JsonConfigManager.buildingDataList)
            {
                Dictionary<int, BuildingData> centerAreeDatas = JsonConfigManager.GetBuildingData(tableName);
                float speed = 90 / JsonConfigManager.buildingDataList.Count / centerAreeDatas.Count;
                float proportion = 0;
                int count = 0;
                foreach (var item in centerAreeDatas)
                {
                    ObjectManager.instance.InstantiateObjectAsync(prefabDatas[item.Value.PrefabID].Path, (path, go/*GameObject*/, param1, param2, param3) =>
                    {
                        count++;
                        UserInfoManager.LoadNum = UserInfoManager.LoadNum + (1.0f / centerAreeDatas.Count) * 100;
                        GameObject instantiateGo = go as GameObject;
                        instantiateGo.SetActive(true);
                        Debug.Log("Before: " + instantiateGo.transform.localPosition + "   " + instantiateGo.transform.localRotation + "   " + instantiateGo.transform.localScale);
                        item.Value.m_Object = instantiateGo;
                        if (count >= centerAreeDatas.Count)
                        {
                            foreach (var data in centerAreeDatas)
                            {
                                if (data.Value.Parent > 0)
                                {
                                    data.Value.m_Object.transform.SetParent(centerAreeDatas[data.Value.Parent].m_Object.transform);
                                }
                                data.Value.m_Object.transform.localPosition = new Vector3(data.Value.Position[0], data.Value.Position[1], data.Value.Position[2]);
                                data.Value.m_Object.transform.localRotation = Quaternion.Euler(new Vector3(data.Value.Rotation[0],  data.Value.Rotation[1], data.Value.Rotation[2]));
                                data.Value.m_Object.transform.localScale = new Vector3(data.Value.Scale[0], data.Value.Scale[1], data.Value.Scale[2]);
                                data.Value.m_Object.name = data.Value.Name;
                                SetPrefabDatas(data);
                                Debug.Log("After: " + data.Value.m_Object.transform.localPosition + "   " + data.Value.m_Object.transform.localRotation + "   " + data.Value.m_Object.transform.localScale);
                            }
                            allCount++;
                            if (allCount >= JsonConfigManager.buildingDataList.Count)
                            {
                                obj.Invoke();
                                UserInfoManager.LoadNum = 0;
                            }
                        }
                    }, LoadResPriority.RES_HIGHT, false, null, null, null, true);
                }
            }           
        }

        private static void SetPrefabDatas(KeyValuePair<int, BuildingData> data)
        {
            Debug.Log("生成了" + data.Value.Name);
            switch (data.Value.Name)
            {
                default:
                    break;
            }
        }
        //static Transform ledObject;
        //static Transform doorParentObject;
        //private static void SetFZCLed(KeyValuePair<int, BuildingData> data)
        //{
        //    Transform led = data.Value.m_Object.transform.Find("billboard").transform;
        //    Transform doorParent = data.Value.m_Object.transform.Find("FanZhiChang/fzc_room/fzc_room_door").transform;
        //    ledObject = led;
        //    doorParentObject = doorParent;
        //    SetFZCLed3dData(led,doorParent);
        //}

        //private static void SetFZCLed3dData(Transform led, Transform doorParent)
        //{
        //    if (UserInfoManager.MyBreedSiteData == null)
        //        return;
        //    led.Find("group2/booking_room/QueueNumText").GetComponent<TextMeshPro>().text = ":"+UserInfoManager.MyBreedSiteData.queueNumber;
        //    Transform breedRoom = led.Find("BreedingRoomLED");
        //    int index = 0;
        //    for (int i = 0; i < breedRoom.childCount; i++)
        //    {
        //        breedRoom.GetChild(i).gameObject.SetActive(false);
        //    }
        //    for (int i = 0; i < doorParent.childCount; i++)
        //    {
        //        doorParent.GetChild(i).GetComponent<BoxCollider>().enabled = false;
        //        doorParent.GetChild(i).Find("Room_tips").gameObject.SetActive(false);
        //    }
        //    foreach (var item in UserInfoManager.MyBreedSiteData.list)
        //    {
        //        Debug.Log("排序后的：" + item.roomNumber);
        //        Transform room = breedRoom.GetChild(index);
        //        Transform tips = doorParent.GetChild(index).Find("Room_tips");
        //        Transform breeding = room.Find("State/Breeding");
        //        breeding.GetComponent<Renderer>().material.SetColor("_BaseColor", Color.red); 
        //        Transform waiting = room.Find("State/Waiting");
        //        waiting.GetComponent<Renderer>().material.SetColor("_BaseColor", Color.green);
        //        Transform free = room.Find("State/Free");
        //        free.GetComponent<Renderer>().material.SetColor("_BaseColor", Color.yellow);
        //        Transform breedingTips = tips.Find("red");
        //        Transform waitingTips = tips.Find("green");
        //        Transform freeTips = tips.Find("Yellow");
        //        bool isfree = item.useStatus.Equals("0")&&!item.status.Equals("0") ? true : false;
        //        bool isWait = item.useStatus.Equals("1") && !item.status.Equals("0") ? true : false;
        //        bool isBreed = item.useStatus.Equals("2") && !item.status.Equals("0") ? true : false;
        //        tips.gameObject.SetActive(isfree|| isWait|| isBreed);
        //        room.gameObject.SetActive(isfree || isWait || isBreed);
        //        free.gameObject.SetActive(isfree);
        //        waiting.gameObject.SetActive(isWait);
        //        breeding.gameObject.SetActive(isBreed);
        //        freeTips.gameObject.SetActive(isfree);
        //        waitingTips.gameObject.SetActive(isWait);
        //        breedingTips.gameObject.SetActive(isBreed);
        //        doorParent.GetChild(index).GetComponent<BoxCollider>().enabled = (!isBreed&&isWait);
        //        doorParent.GetChild(index).GetChild(0).Find("HighLight").gameObject.SetActive(!isBreed && isWait);
        //        index++;
        //    }
        //    CreateBreedHorse(doorParent);
        //}

        //private static void CreateBreedHorse(Transform doorParent)
        //{
        //    UserInfoManager.doorParent = doorParent;
        //    int index = 0;
        //    foreach (var item in UserInfoManager.MyBreedSiteData.list)
        //    {
        //        if (item.horseFatherData != null)
        //        {
        //            if (doorParent.GetChild(index).Find("Father").childCount > 0)
        //            {
        //                var horse = doorParent.GetChild(index).Find("Father").GetChild(0);
        //                horse.name = item.horseFatherData.id;
        //                int price = item.price != null ? (int)float.Parse(item.price) : 0;
        //                HorseBreedObject obj = ObjectsManager.instance.AddObject(horse.gameObject, "HorseFather", "HorseBreedObject", item.horseFatherData, price, item.id) as HorseBreedObject;
        //                item.horseFatherData.horse = horse.gameObject;
        //                item.horseFatherData.horseBreedCtrl = obj;
        //            }
        //            else
        //            {
        //                var horse = ObjectManager.instance.InstantiateObject("Assets/GameData/Prefabs/Animals/Horse.prefab", true, true);
        //                horse.name = item.horseFatherData.id;
        //                horse.transform.SetParent(doorParent.GetChild(index).Find("Father"));
        //                horse.transform.ResetLocal();
        //                int price = item.price != null ? (int)float.Parse(item.price) : 0;
        //                HorseBreedObject obj = ObjectsManager.instance.AddObject(horse, "HorseFather", "HorseBreedObject", item.horseFatherData, price, item.id) as HorseBreedObject;
        //                item.horseFatherData.horse = horse;
        //                item.horseFatherData.horseBreedCtrl = obj;
        //            }
        //        }
        //        else
        //        {
        //            if (doorParent.GetChild(index).Find("Father").childCount > 0)
        //            {
        //                doorParent.GetChild(index).Find("Father").GetChild(0).gameObject.SetActive(false);
        //            }
        //        }
        //        if (item.horseMotherData != null)
        //        {
        //            if (doorParent.GetChild(index).Find("Mother").childCount > 0)
        //            {
        //                var horse = doorParent.GetChild(index).Find("Mother").GetChild(0);
        //                horse.name = item.horseMotherData.id;
        //                int price = item.price != null ? (int)float.Parse(item.price) : 0;
        //                HorseBreedObject obj = ObjectsManager.instance.AddObject(horse.gameObject, "HorseMother", "HorseBreedObject", item.horseMotherData, price, item.id) as HorseBreedObject;
        //                item.horseMotherData.horse = horse.gameObject;
        //                item.horseMotherData.horseBreedCtrl = obj;
        //            }
        //            else
        //            {
        //                var horse = ObjectManager.instance.InstantiateObject("Assets/GameData/Prefabs/Animals/Horse.prefab", true, true);
        //                horse.name = item.horseMotherData.id;
        //                horse.transform.SetParent(doorParent.GetChild(index).Find("Mother"));
        //                horse.transform.ResetLocal();
        //                int price = item.price != null ? (int)float.Parse(item.price) : 0;
        //                HorseBreedObject obj = ObjectsManager.instance.AddObject(horse, "HorseMother", "HorseBreedObject", item.horseMotherData, price, item.id) as HorseBreedObject;
        //                item.horseMotherData.horse = horse;
        //                item.horseMotherData.horseBreedCtrl = obj;
        //            }
        //        }
        //        else
        //        {
        //            if (doorParent.GetChild(index).Find("Mother").childCount > 0)
        //            {
        //                doorParent.GetChild(index).Find("Mother").GetChild(0).gameObject.SetActive(false);
        //            }
        //        }
        //        index++;
        //    }
        //}

        //private static void CreateFanZhiChang(KeyValuePair<int, BuildingData> data)
        //{
        //}

        //internal static void RefreshBreedData()
        //{
        //    SetFZCLed3dData(ledObject, doorParentObject);
        //}

        //private static void GetComponent(KeyValuePair<int, BuildingData> data)
        //{
        //    UserInfoManager.horseCantPutDown = data.Value.m_Object.transform.Find("GameObject/HorseCantDown").gameObject;
        //}
        //static Transform maCaos;
        //private static void CreateHorse(KeyValuePair<int, BuildingData> data)
        //{
        //    int index = 0;
        //    maCaos = data.Value.m_Object.transform.Find("Horse_farms_room/Collider/MaCaos").transform;
        //    foreach (var item in UserInfoManager.MyHorseList)
        //    {
        //        var horse = ObjectManager.instance.InstantiateObject("Assets/GameData/Prefabs/Animals/Horse.prefab", true, true);
        //        horse.transform.SetParent(maCaos.GetChild(index).Find("HorsePos"));
        //        Transform foodParent = maCaos.GetChild(index).Find("fodders");
        //        for (int i = 0; i < foodParent.childCount; i++)
        //        {
        //            foodParent.GetChild(i).gameObject.SetActive(i < item.Value.feedNember);
        //        }
        //        horse.transform.ResetLocal();
        //        HorseObject obj = ObjectsManager.instance.AddObject(horse, "Horse" + item.Key, "HorseObject", item.Value) as HorseObject;
        //        item.Value.horse = horse;
        //        item.Value.horseCtrl = obj;
        //        index++;
        //    }
        //    UserInfoManager.NowHorseList.Clear();
        //    UserInfoManager.NowHorseList = new Dictionary<int, HorseData>(UserInfoManager.MyHorseList);
        //    if (UserInfoManager.detailHorse == null)
        //    {
        //        var horse = ObjectManager.instance.InstantiateObject("Assets/GameData/Prefabs/Animals/Horse.prefab", true, true);
        //        horse.transform.SetParent(UserInfoManager.horsePos.transform);
        //        horse.transform.ResetLocal();
        //        HorseData horsedata = null;
        //        if (UserInfoManager.NowHorseList.Count > 0)
        //            horsedata = UserInfoManager.NowHorseList[1];

        //        HorseObject obj = ObjectsManager.instance.AddObject(horse, "Horse" + 1, "HorseObject", horsedata) as HorseObject;
        //        horse.SetActive(false);
        //        UserInfoManager.detailHorse = horse;
        //        UserInfoManager.horseDetailObject = obj;
        //    }
        //}

        //internal static void SetFeedNum(HorseData[] list)
        //{
        //    int index = 0;
        //    foreach (var item in list)
        //    {
        //        Transform foodParent = maCaos.GetChild(index).Find("fodders");
        //        for (int i = 0; i < foodParent.childCount; i++)
        //        {
        //            //Debug.Log((index + 1) + "马槽里面 第" + i+"个马粟"+( i < item.feedNember));
        //            foodParent.GetChild(i).gameObject.SetActive(i < item.feedNember);
        //        }
        //        index++;
        //    }
        //}
    }
}
