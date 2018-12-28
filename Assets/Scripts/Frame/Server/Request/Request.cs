using Common;
using ExitGames.Client.Photon;
using System.Collections.Generic;

public abstract class Request : EventHandler
{
    /// <summary>
    /// 要发送的数据
    /// </summary>
    protected Dictionary<byte, object> data = new Dictionary<byte, object>();

    public OperationCode opCode;
    public abstract void DefaultRequest();
    public abstract void OnOperationResponse(OperationResponse opResponse);

    public virtual void Start()
    {
        Game.Instance.serverEngine.AddRequest(this);
    }

    public override void HandleEvent(params object[] data)
    {
        
    }

    public void OnDestroy()
    {
        Game.Instance.serverEngine.RemoveRequest(opCode);
    }
}
