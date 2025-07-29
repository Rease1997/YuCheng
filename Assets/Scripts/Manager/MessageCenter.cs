using System;
using System.Collections.Generic;

/// <summary>
/// ��Ϣ����
/// </summary>
public class MessageCenter : Singleton<MessageCenter>
{
    Dictionary<int, Action<Notification>> dic = new Dictionary<int, Action<Notification>>();

    /// <summary>
    /// ����������ķ�����
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
    /// �Ƴ��¼���
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
    /// �ɷ��¼���
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
    /// ֪ͨ������ݡ�
    /// </summary>
    public object[] content;

    public Notification(params object[] content)
    {
        this.content = content;
    }
}
