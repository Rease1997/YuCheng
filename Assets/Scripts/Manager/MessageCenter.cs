using System;
using System.Collections.Generic;

/// <summary>
/// 消息中心
/// </summary>
public class MessageCenter : Singleton<MessageCenter>
{
    Dictionary<int, Action<Notification>> dic = new Dictionary<int, Action<Notification>>();

    /// <summary>
    /// 添加侦听器的方法。
    /// </summary>
    /// <param name="msgID"></param>
    /// <param name="action"></param>
    public void AddListener(int msgID, Action<Notification> action)
    {
        if (dic.ContainsKey(msgID))
            dic[msgID] += action;
        else
            dic.Add(msgID, action);
    }

    /// <summary>
    /// 移除事件。
    /// </summary>
    /// <param name="msgID"></param>
    /// <param name="action"></param>
    public void RemoveListener(int msgID, Action<Notification> action)
    {
        dic[msgID] -= action;
        if (dic[msgID] == null)
            dic.Remove(msgID);
    }


    /// <summary>
    /// 派发事件。
    /// </summary>
    /// <param name="msgID"></param>
    /// <param name="notification"></param>
    public void Dispatch(int msgID, Notification notification = null)
    {
        if (dic.ContainsKey(msgID))
            dic[msgID](notification);
    }

    internal void ClearAllListener()
    {
        dic.Clear();
    }
}

public class Notification
{
    /// <summary>
    /// 通知里的内容。
    /// </summary>
    public object[] content;

    public Notification(params object[] content)
    {
        this.content = content;
    }
}
