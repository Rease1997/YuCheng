using System.Collections.Generic;
using UnityEngine;
/*
* Author:W
* Excel表转换生成
* AllPrefabsData
*/
namespace HotFix
{
    [System.Serializable]
    public class BuildingDataParSer
    {
        public List<BuildingData> data = new List<BuildingData>();
        public List<BuildingData> Data
        {
            get
            {
                return data;
            }
        }
    }
    [System.Serializable]
    public class BuildingData
    {
        /// <summary>
        /// 区域内唯一ID
        /// <summary>
        public int ID;
        /// <summary>
        /// 预制体ID
        /// <summary>
        public int PrefabID;
        /// <summary>
        /// 预制体名称
        /// <summary>
        public string Name;
        /// <summary>
        /// 父节点ID（如果为空为-1）
        /// <summary>
        public int Parent;
        /// <summary>
        /// 坐标(x|y|z)
        /// <summary>
        public float[] Position;
        /// <summary>
        /// 旋转(x|y|z)
        /// <summary>
        public float[] Rotation;
        /// <summary>
        /// 大小(x|y|z)
        /// <summary>
        public float[] Scale;

        public GameObject m_Object;

        public BuildingData(int iD, int prefabID, string name, int parent, string position, string rotation, string scale)
        {
            ID = iD;
            PrefabID = prefabID;
            Name = name;
            Parent = parent;
            string[] the_Position = position.Split('|');
            string[] the_Rotation = rotation.Split('|');
            string[] the_Scale = scale.Split('|');
            Position = new float[the_Position.Length];
            Rotation = new float[the_Rotation.Length];
            Scale = new float[the_Scale.Length];
            for (int i = 0; i < the_Position.Length; i++)
            {
                Position[i] = float.Parse(the_Position[i]);
            }
            for (int i = 0; i < the_Rotation.Length; i++)
            {
                Rotation[i] = float.Parse(the_Rotation[i]);
            }
            for (int i = 0; i < the_Position.Length; i++)
            {
                Scale[i] = float.Parse(the_Scale[i]);
            }
        }
    }
    public class AllPrefabsDataParSer
    {
        public List<AllPrefabsData> data = new List<AllPrefabsData>();
        public List<AllPrefabsData> Data
        {
            get
            {
                return data;
            }
        }
    }
    public class AllPrefabsData
    {
        /// <summary>
        /// 预制体id
        /// <summary>
        public int ID;
        /// <summary>
        /// 预制体名称
        /// <summary>
        public string Name;
        /// <summary>
        /// 预制体路径
        /// <summary>
        public string Path;

        public AllPrefabsData(int iD, string name, string path)
        {
            ID = iD;
            Name = name;
            Path = path;
        }
    }
}