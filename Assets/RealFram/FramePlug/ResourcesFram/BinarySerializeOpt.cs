﻿using System;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using UnityEngine;

public class BinarySerializeOpt
{
    /// <summary>
    /// 类序列化成Xml
    /// </summary>
    /// <param name="path"></param>
    /// <param name="obj"></param>
    /// <returns></returns>
   public static bool Xmlserialize(string path,object obj)
    {
        try
        {
            using(FileStream fs=new FileStream(path, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                using(StreamWriter sw=new StreamWriter(fs, System.Text.Encoding.UTF8))
                {
                    XmlSerializer xs = new XmlSerializer(obj.GetType());
                    xs.Serialize(sw, obj);
                }
            }
            return true;
        }
        catch (Exception e)
        {
            Debug.LogError("此类无法转换成xml " + obj.GetType() + "," + e);
        }
        return false;
    }
    /// <summary>
    /// 编辑器时读取xml
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="path"></param>
    /// <returns></returns>
    public static T XmlDeserialize<T> (string path)where T : class
    {
        T t = default;
        try
        {
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                XmlSerializer xs = new XmlSerializer(typeof(T));
                t = (T)xs.Deserialize(fs);
            }
        }
        catch (Exception e)
        {
            Debug.LogError("此xml无法转换成二进制 " + path + "," + e);
        }
        return t;
    }
    /// <summary>
    /// XML的反序列化
    /// </summary>
    /// <param name="path"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    public static object XmlDeserialize(string path,Type type)
    {
        object obj = null;
        try
        {
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                XmlSerializer xs = new XmlSerializer(type);
                obj=xs.Deserialize(fs);
            }
        }
        catch (Exception e)
        {
            Debug.LogError("此xml无法转换成二进制 " + path + "," + e);
        }
        return obj;
    }
    /// <summary>
    /// 运行时使读取xml
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="path"></param>
    /// <returns></returns>
    public static T XmlDeserializeRun<T>(string path) where T : class
    {
        T t = default;
        TextAsset textAsset = ResourceManager.instance.LoadResources<TextAsset>(path);
        if (textAsset == null)
        {
            UnityEngine.Debug.LogError("cant load TextAsset: " + path);
            return null;
        }
        try
        {
            using(MemoryStream stream=new MemoryStream(textAsset.bytes))
            {
                XmlSerializer xs = new XmlSerializer(typeof(T));
                t = (T)xs.Deserialize(stream);
            }
            ResourceManager.instance.RealaseResource(path, true);
        }
        catch (Exception e)
        {

            Debug.LogError("load TextAsset exception:" + path + "," + e);
        }
        return t;
    }
    /// <summary>
    /// 类转换成二进制
    /// </summary>
    /// <param name="path"></param>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static bool BinarySerlize(string path,object obj)
    {
        try
        {
            using(FileStream fs=new FileStream(path, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(fs, obj);
            }
            return true;
        }
        catch (Exception e)
        {
            Debug.LogError("此类无法转换成二进制 " + obj.GetType() + "," + e);
        }
        return false;
    }

    /// <summary>
    /// 读取二进制
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="path"></param>
    /// <returns></returns>
    public static T BinaryDeserialize<T>(string path) where T : class
    {
        T t = default;
        TextAsset textAsset = ResourceManager.instance.LoadResources<TextAsset>(path);
        if (textAsset == null)
        {
            UnityEngine.Debug.LogError("cant load TextAsset: " + path);
            return null;
        }
        try
        {
            using (MemoryStream stream = new MemoryStream(textAsset.bytes))
            {
                BinaryFormatter bf = new BinaryFormatter();
                t = (T)bf.Deserialize(stream);
            }
            ResourceManager.instance.RealaseResource(path, true);
        }
        catch (Exception e)
        {

            Debug.LogError("load TextAsset exception:" + path + "," + e);
        }
        return t;
    }
}
