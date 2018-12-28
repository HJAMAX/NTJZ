using BestHTTP;

public abstract class AbstractSendMsgEventHandler : EventHandler
{
    public override void HandleEvent(params object[] data)
    {
        UIRoot.Broadcast("SetEnabled", false);
    }

    protected virtual void OnRequestCallBack(HTTPRequest request, HTTPResponse response)
    {
        UIRoot.Broadcast("SetEnabled", true);
        UnityEngine.Debug.Log(response.DataAsText);
    }

    protected void SendEvent(PostData[] postDataList, string webLocation, OnRequestFinishedDelegate callback)
    {
        UIRoot.Broadcast("SetEnabled", false);
        Game.Instance.EventBus.SendEvents("post", webLocation, callback, postDataList);
    }

    protected void SendEvent(string webLocation, OnRequestFinishedDelegate callback)
    {
        UIRoot.Broadcast("SetEnabled", false);
        Game.Instance.EventBus.SendEvents("post", webLocation, callback);
    }

    protected void SendEvent(PostData[] postDataList, PostBinaryData[] postBinaryDataList,
                             string webLocation, OnRequestFinishedDelegate callback)
    {
        UIRoot.Broadcast("SetEnabled", false);
        Game.Instance.EventBus.SendEvents("post", webLocation, callback, postDataList, postBinaryDataList);
    }
}
