using LitJson;
using RPG.Sockets;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
public delegate void EventListener(string err);
public delegate void MsgListener(MsgBase msg);

public class NetManager : Singleton<NetManager>
{
    //是否启用心跳
    public bool isUsePing = true;
    //心跳间隔时间
    public int pingInterval = 5;
    //上一次发送Ping时间
    public float lastPingTime = 0;
    //上一次接收Pong时间
    public float lastPongTime = 0;
    //消息监听字典
    private Dictionary<string, MsgListener> MsgListenerDic = new Dictionary<string, MsgListener>();
    //事件监听字典
    private Dictionary<NetEvent, EventListener> EventListenerDic = new Dictionary<NetEvent, EventListener>();
    //是否正在断开连接
    private bool isClosing;
    //是否正在连接
    private bool isConnecting;
    private Socket socket;
    //发送消息缓存队列
    private Queue<byte[]> writeQueue;
    //接收消息缓存列表
    private List<MsgBase> msgList;
    private Queue<NetEvent> eventList;
    //接收消息缓存列表数量
    private int msgCount;
    //每一次Update处理的消息量
    readonly int MAX_MESSAGE_FIRE = 10;
    private Message message = new Message();
    private string ip;
    //private bool connectedFailed = false;
    //初始化
    private void InitState()
    {
        //上次发送Ping时间
        lastPingTime = Time.time;
        //上次收到Pong时间
        lastPongTime = Time.time;
        isClosing = false;
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        writeQueue = new Queue<byte[]>();
        msgList = new List<MsgBase>();
        eventList = new Queue<NetEvent>();
        msgCount = 0;
        if (!MsgListenerDic.ContainsKey("MsgPong"))
        {
            MsgListenerDic.Add("MsgPong", OnMsgPong);
        }
    }
    /// <summary>
    /// 发送Ping协议
    /// </summary>
    private void PingUpdate()
    {
        if (!isUsePing)
        {
            return;
        }
        if (socket == null || !socket.Connected)
        {
            /*if(!isStartReconnect)
            {
                OnStartReconnect();
            }*/

            return;
        }
        //发送Ping
        if (Time.time - lastPingTime > pingInterval)
        {
            //Debug.Log("发送心跳包");
            JsonData data = new JsonData();
            MsgBase ping = new MsgBase("MsgPing", data.ToJson());
            Send(ping);
            lastPingTime = Time.time;
        }
        //Debug.LogError("PingUpdate 主动断开连接 socket close");
        if (Time.time - lastPongTime > pingInterval * 4)
        {
            Debug.LogError("PingUpdate 主动断开连接 socket close");
            socket.Close();
            socket.Dispose();
            OnStartReconnect();

        }

    }
    private void OnMsgPong(MsgBase msg)
    {
        lastPongTime = Time.time;
    }
    /// <summary>
    /// 连接服务器
    /// </summary>
    /// <param name="ip">服务器Ip</param>
    /// <param name="port">端口号</param>
    public void Connect(string ip, int port)
    {
        this.ip = ip;
        if (socket != null && socket.Connected)
        {
            Debug.Log("已连接!!!");
            return;
        }
        isConnecting = true;
        if (isConnecting)
        {
            Debug.Log("正在连接.....");
        }
        InitState();

        socket.BeginConnect(ip, port, ConnectCallBack, socket);
    }
    public bool HasConnected()
    {
        if (socket != null && socket.Connected)
        {
            return true;
        }
        return false;
    }
    /// <summary>
    /// 断线重连服务器
    /// </summary>
    /// <param name="ip">服务器Ip</param>
    /// <param name="port">端口号</param>
    public void ReConnect()
    {

        if (socket != null && socket.Connected)
        {
            return;
        }
        isConnecting = true;
        if (isConnecting)
        {
            Debug.Log("断线重连中");
            RFrameWork.instance.OpenCommonConfirm("网络异常", "断线重连，连接中。。。", () =>
            {

                ReConnect();

            }, null);

        }
        InitState();
        socket.BeginConnect(this.ip, 8107, ConnectCallBack, socket);
    }
    public void OnStartReconnect()
    {
        //connectedFailed = false;
        Debug.Log("OnStartReconnect 连接失败重新链接");
        if (!RFrameWork.instance.checkReconnect)
        {
            return;
        }
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            RFrameWork.instance.OpenCommonConfirm("网络连接失败", "网络连接失败，请检查网络连接是否正常？", () =>
            {

                OnStartReconnect();


            }, null);

        }
        else
        {
            WaitOpen();
        }



    }
    private void WaitOpen()
    {
        Debug.Log("连接失败重新打开页面:");
        RFrameWork.instance.OpenCommonConfirm("网络异常", "网络异常，请重新连接网络！！！", () =>
        {

            ReConnect();

        }, null);
    }
    /// <summary>
    /// 断开连接
    /// </summary>
    public void Close()
    {
        Debug.LogError("Close 主动断开连接 socket close");
        socket.Close();
        socket.Dispose();
        eventList.Enqueue(NetEvent.Close);
        writeQueue = new Queue<byte[]>();
        msgList = new List<MsgBase>();
        eventList = new Queue<NetEvent>();
        if (MsgListenerDic.Count > 0 && MsgListenerDic != null)
            MsgListenerDic.Clear();
        MsgListenerDic = new Dictionary<string, MsgListener>();
        if (EventListenerDic.Count > 0 && EventListenerDic != null)
            EventListenerDic.Clear();
        EventListenerDic = new Dictionary<NetEvent, EventListener>();
        //if (socket == null || !socket.Connected)
        //{
        //    Debug.Log("未能断开连接，原因：未连接服务器");
        //    return;
        //}
        //if (isConnecting)
        //{
        //    Debug.Log("未能断开连接，原因：未连接服务器");
        //    return;
        //}
        //if (writeQueue.Count > 0)
        //{
        //    isClosing = true;
        //}
        //else
        //{
        //    Debug.LogError("Close 主动断开连接 socket close");
        //    socket.Close();
        //    socket.Dispose();
        //    eventList.Enqueue(NetEvent.Close);
        //    //FireEvent(NetEvent.Close, "");
        //}
    }
    public void TestSocketConnected()
    {
        Debug.Log("网络连接状态" + socket.Connected);
    }
    /// <summary>
    /// 发送消息
    /// </summary>
    public void Send(MsgBase msg)
    {
        if (socket == null || !socket.Connected)
        {
            Debug.Log("发送失败，未连接服务器！");
            return;
        }
        if (isConnecting)
        {
            Debug.Log("发送失败,正在连接服务器。。。。");
            return;
        }
        if (isClosing)
        {
            Debug.Log("发送失败，正在断开连接。。。。");
            return;
        }
        Debug.Log("给服务器发送消息code：" + msg.requestCode + "  data:" + msg.data);
        //数据编码
        byte[] bodyBytes = MsgBase.Encode(msg);
        int count = 0;
        lock (writeQueue)
        {
            writeQueue.Enqueue(bodyBytes);
            count = writeQueue.Count;
        }
        if (count > 0)
        {
            socket.BeginSend(bodyBytes, 0, bodyBytes.Length, 0, SendCallBack, socket);
        }
    }
    /// <summary>
    /// 更新消息
    /// </summary>
    public void MsgUpdate()
    {
        if (msgCount == 0)
        {
            return;
        }
        for (int i = 0; i < MAX_MESSAGE_FIRE; i++)
        {
            //获取第一条消息
            MsgBase msg = null;
            lock (msgList)
            {
                if (msgList.Count > 0)
                {
                    msg = msgList[0];
                    msgList.RemoveAt(0);
                    msgCount--;
                }
            }
            if (msg != null)
            {
                FireMsg(msg.requestCode, msg);
            }
            else
            {
                break;
            }
        }
    }
    /// 更新消息
    /// </summary>
    public void EventUpdate()
    {
        if (EventListenerDic.Count == 0 || eventList.Count <= 0)
        {
            return;
        }
        //获取第一条消息

        lock (EventListenerDic)
        {

            NetEvent netEvent = eventList.Dequeue();
            if (EventListenerDic.Count > 0)
            {
                Debug.Log("开始分发消息：" + netEvent);
                if (EventListenerDic.ContainsKey(netEvent))
                {
                    FireEvent(netEvent, "");
                }

            }
        }
    }
    /// <summary>
    /// 发送消息回调
    /// </summary>
    /// <param name="asyncResult"></param>
    private void SendCallBack(IAsyncResult asyncResult)
    {
        Socket socket = asyncResult.AsyncState as Socket;
        if (socket == null || !socket.Connected)
        {
            return;
        }
        //EndSend
        int count = socket.EndSend(asyncResult);
        //取出队列第一个消息
        byte[] ba;
        lock (writeQueue)
        {
            ba = writeQueue.Peek();
        }
        int idx = count;
        if (ba.Length == count)
        {
            idx = 0;
            lock (writeQueue)
            {
                writeQueue.Dequeue();
                ba = writeQueue.Count > 0 ? writeQueue.Peek() : null;
            }
        }
        if (ba != null)
        {
            socket.BeginSend(ba, idx, ba.Length, 0, SendCallBack, socket);
        }
        else if (isClosing)
        {
            Debug.LogError("SendCallBack 主动断开连接 socket close");
            //socket.Close();
        }
    }
    //连接回调
    private void ConnectCallBack(IAsyncResult asyncResult)
    {
        try
        {
            Socket socket = asyncResult.AsyncState as Socket;
            socket.EndConnect(asyncResult);
            isConnecting = false;
            socket.BeginReceive(message.ReadBytes, message.StartIndex, message.RemainCount(), SocketFlags.None, ReciveCallBack, null);
            Debug.Log("连接服务器连接成功 ip:" + this.ip);
            eventList.Enqueue(NetEvent.ConnectSucc);
            //connectedFailed = false;


        }
        catch (SocketException ex)
        {
            isConnecting = false;
            //connectedFailed = true;
            Debug.Log("连接失败：" + ex.ToString());
            eventList.Enqueue(NetEvent.ConnectFail);




        }
    }
    private void ReciveCallBack(IAsyncResult asyncResult)
    {
        if (socket == null || (!socket.Connected))
        {
            return;
        }

        int count = socket.EndReceive(asyncResult);

        if (count > 0)
        {
            Debug.Log("接收Socket数据长度：" + count);
            message.ReadMsg(count, OnProcessDataCallBack);
        }
        //继续接收数据
        socket.BeginReceive(message.ReadBytes, message.StartIndex, message.RemainCount(), SocketFlags.None, ReciveCallBack, null);

    }
    /// <summary>
    /// 处理缓冲区消息（解决粘包问题）
    /// </summary>
    private void OnProcessDataCallBack(string msg)
    {
        Debug.Log("接收到服务器发送消息：" + msg);
        JsonData data = JsonMapper.ToObject(msg);
        MsgBase mBase = new MsgBase(data["requestCode"].ToString(), JsonMapper.ToJson(data["data"]));
        //添加消息
        lock (msgList)
        {
            msgList.Add(mBase);
        }
        msgCount++;
        //Array.Copy(readBuff, start, readBuff, 0, count);
        //buffcount -= start;
        //OnReciveData(count);
    }

    //分发消息
    private void FireEvent(NetEvent netEvent, string err)
    {
        if (EventListenerDic.ContainsKey(netEvent) && EventListenerDic[netEvent] != null)
        {
            EventListenerDic[netEvent](err);
        }
    }
    /// <summary>
    /// 添加监听事件
    /// </summary>
    /// <param name="netEvent"></param>
    /// <param name="listener"></param>
    public void AddListener(NetEvent netEvent, EventListener listener)
    {
        if (EventListenerDic.ContainsKey(netEvent))
        {
            EventListenerDic[netEvent] += listener;
        }
        else
        {
            EventListenerDic.Add(netEvent, listener);
        }
    }
    /// <summary>
    /// 移除监听事件
    /// </summary>
    /// <param name="netEvent"></param>
    /// <param name="listener"></param>
    public void RemoveListener(NetEvent netEvent, EventListener listener)
    {
        if (EventListenerDic.ContainsKey(netEvent))
        {
            if (EventListenerDic[netEvent] != null)
            {
                EventListenerDic[netEvent] -= listener;
            }
            else
            {
                EventListenerDic.Remove(netEvent);
            }
        }
    }
    /// <summary>
    /// 添加消息监听
    /// </summary>
    /// <param name="msgName">消息名称</param>
    /// <param name="msgListener">回调函数</param>
    public void AddMsgListener(string msgName, MsgListener msgListener)
    {
        if (MsgListenerDic.ContainsKey(msgName))
        {
            MsgListenerDic[msgName] += msgListener;
        }
        else
        {
            MsgListenerDic[msgName] = msgListener;
        }
    }
    /// <summary>
    /// 移除消息监听
    /// </summary>
    /// <param name="msgName">消息类型名称</param>
    /// <param name="msgListener">回调函数</param>
    public void RemoveMsgListener(string msgName, MsgListener msgListener)
    {
        if (MsgListenerDic.ContainsKey(msgName))
        {
            if (MsgListenerDic[msgName] != null)
            {
                MsgListenerDic[msgName] -= msgListener;
            }
            else
            {
                MsgListenerDic.Remove(msgName);
            }
        }
    }
    public static void TesSocketConnect()
    {

    }
    /// <summary>
    /// 分发消息
    /// </summary>
    /// <param name="msgName">消息类型</param>
    /// <param name="msg">消息类</param>
    private void FireMsg(string msgName, MsgBase msg)
    {
        if (MsgListenerDic.ContainsKey(msgName) && MsgListenerDic[msgName] != null)
        {
            MsgListenerDic[msgName](msg);
        }
    }

    public void Update()
    {
        MsgUpdate();
        PingUpdate();
        EventUpdate();


    }
}
public enum NetEvent
{
    ConnectSucc = 1,
    ConnectFail = 2,
    Close = 3,
}
