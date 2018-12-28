using ExitGames.Client.Photon;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using Common;

public class ServerEngine : MonoBehaviour, IPhotonPeerListener
{
    [SerializeField]
    private bool connectStart;

    [SerializeField]
    private string ip;

    [SerializeField]
    private string port;

    [SerializeField]
    private string serverName;

    private PhotonPeer peer;

    public PhotonPeer Peer { get { return peer; } }
    
    /// <summary>
    /// 请求
    /// </summary>
    private Dictionary<OperationCode, Request> requestMap = new Dictionary<OperationCode, Request>();

    /// <summary>
    /// 事件
    /// </summary>
    private Dictionary<EventCode, BaseEvent> eventMap = new Dictionary<EventCode, BaseEvent>();

    /// <summary>
    /// 重连次数
    /// </summary>
    private int reConnCount = 0;

    /// <summary>
    /// 最大重连次数
    /// </summary>
    private int maxConnCount = 45;

    /// <summary>
    /// 是否后台运行
    /// </summary>
    private bool isRunningBackground = false;

    /// <summary>
    /// 是否连接状态
    /// </summary>
    private bool isConnected = false;

    /// <summary>
    /// 是否主动登出
    /// </summary>
    private bool isLogout = false;

    private int lastNetworkDataReceivedTime;

    public int TimeSinceLastUpdate
    {
        get
        {
            int timeSinceLastUpdate = peer.ServerTimeInMilliSeconds - lastNetworkDataReceivedTime;
            lastNetworkDataReceivedTime = peer.ServerTimeInMilliSeconds;
            return timeSinceLastUpdate;
        }
    }

    void Start()
    {
        peer = new PhotonPeer(this, ConnectionProtocol.Udp);

        if(connectStart)
        {
            Connect();
        }
    }

    void Update()
    {
        peer.Service();
    }

    void OnDestroy()
    {
        if(peer != null && peer.PeerState == PeerStateValue.Connected)
        {
            peer.Disconnect();
        }
    }

    public void DebugReturn(DebugLevel level, string message)
    {

    }

    public void OnEvent(EventData eventData)
    {
        EventCode code = (EventCode)eventData.Code;
        BaseEvent e = null;

        if (eventMap.TryGetValue(code, out e))
        {
            e.OnEvent(eventData);
        }
        else
        {
            Debug.LogError("没找到Event响应对象" + code);
        }
    }

    public void AddRequest(Request request)
    {
        requestMap[request.opCode] = request;
    }

    public void RemoveRequest(OperationCode opCode)
    {
        requestMap.Remove(opCode);
    }

    public void AddEvent(BaseEvent e)
    {
        eventMap.Add(e.eventCode, e);
    }

    public void RemoveEvent(EventCode eventCode)
    {
        eventMap.Remove(eventCode);
    }

    public void OnOperationResponse(OperationResponse operationResponse)
    {
        OperationCode opCode = (OperationCode)operationResponse.OperationCode;
        Request request = null;
        bool isExist = requestMap.TryGetValue(opCode, out request);

        if(isExist)
        {
            request.OnOperationResponse(operationResponse);
        }
        else
        {
            Debug.LogError("没找到Request响应对象" + opCode);
        }
    }

    public void Connect()
    {
        if (!isConnected)
        {
            isLogout = false;
            lastNetworkDataReceivedTime = peer.ServerTimeInMilliSeconds;
            peer.Connect(ip + ":" + port, serverName);
        }
    }

    public void DisConnect()
    {
        isLogout = true;
        peer.Disconnect();
    }

    public void OnStatusChanged(StatusCode statusCode)
    {
        if (statusCode == StatusCode.Connect)
        {
            Game.Instance.EventBus.SendEvents("reconn_popup", false);
            isConnected = true;
            CancelInvoke();
            reConnCount = 0;

            if (!connectStart)
            {
                GetComponent<LoginRequest>().DefaultRequest();
            }
            return;
        }

        if(SceneManager.GetActiveScene().buildIndex == GameData.battleIndex)
        {
            Game.Instance.ReturnStartScene(1);
            return;
        }

        if(statusCode == StatusCode.Disconnect)
        {
            isConnected = false;

            if (!isRunningBackground && !isLogout)
            {
                InvokeRepeating("Reconnect", 1, 1);
            }
        }
        
        else if(statusCode == StatusCode.DisconnectByServerLogic ||
                statusCode == StatusCode.DisconnectByServerUserLimit ||
                statusCode == StatusCode.TimeoutDisconnect ||
                statusCode == StatusCode.DisconnectByServer)
        {
            isLogout = true;
            isConnected = false;
            //直接跳到登录页
            Game.Instance.EventBus.SendEvents("msg_popup", statusCode.ToString());
            Game.Instance.ReturnStartScene(2);
        }
    }

    /// <summary>
    /// 判断是否断线重连
    /// </summary>
    /// <param name="focus"></param>
    private void OnApplicationPause(bool focus)
    {
        //断线后进入前台
        if (!focus && !isConnected && 
            SceneManager.GetActiveScene().buildIndex > GameData.startIndex)
        {
            InvokeRepeating("Reconnect", 1, 1);
        }
    }

    /// <summary>
    /// 断线重连
    /// </summary>
    private void Reconnect()
    {
        if(reConnCount >= maxConnCount)
        {
            reConnCount = 0;
            Game.Instance.LoadingScene(GameData.startIndex);
            return;
        }

        Game.Instance.EventBus.SendEvents("reconn_popup", true, maxConnCount - reConnCount);
        reConnCount++;
        Connect();
    }
}
