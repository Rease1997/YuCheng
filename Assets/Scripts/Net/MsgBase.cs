using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[System.Serializable]
public class MsgBase
{
    //协议名
    public string requestCode = "";
    public string data = "";
    public MsgBase(string protoName, string m_data)
    {
        this.requestCode = protoName;
        this.data = m_data;
    }
    public static byte[] Encode(MsgBase msgBase)
    {
        string json = JsonUtility.ToJson(msgBase);
        //Debug.Log(json + "|");
        return Encoding.UTF8.GetBytes(json+"|");
    }
    public static MsgBase Decode(string protoName, byte[] bytes, int offset, int count)
    {
        string json = Encoding.UTF8.GetString(bytes, offset, count);
        string typeName = "Assets.Scripts.Net.Proto." + protoName;
        MsgBase msg = (MsgBase)JsonUtility.FromJson(json, Type.GetType(typeName));
        return msg;
    }
    /// <summary>
    /// 编码协议名
    /// </summary>
    /// <param name="msg">协议</param>
    /// <returns></returns>
    public static byte[] EncodeProtoName(MsgBase msg)
    {
        byte[] nameBytes = Encoding.UTF8.GetBytes(msg.requestCode);
        int nameLen = nameBytes.Length;
        byte[] ret = new byte[nameLen + 2];
        ret[0] = (byte)(nameLen % 256);
        ret[1] = (byte)(nameLen / 256);
        Array.Copy(nameBytes, 0, ret, 2, nameLen);
        return ret;
    }
    /// <summary>
    /// 解码协议名
    /// </summary>
    /// <param name="bytes"></param>
    /// <param name="offset"></param>
    /// <param name="count"></param>
    /// <returns></returns>
    public static string DecodeProtoName(byte[] bytes, int offset, out int count)
    {
        count = 0;
        if (offset + 2 > bytes.Length)
        {
            return "";
        }
        //读取长度
        Int16 len = (Int16)(bytes[offset + 1] << 8 | bytes[offset]);
        if (len <= 0)
        {
            count = 2;
            return "";
        }
        if (offset + 2 + len > bytes.Length)
        {
            return "";
        }
        count = 2 + len;
        string name = Encoding.UTF8.GetString(bytes, offset + 2, len);
        return name;
    }
}
