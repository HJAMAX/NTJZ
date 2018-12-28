using BestHTTP;

public class UpgradeEventHandler : AbstractSendMsgEventHandler
{
    public override void HandleEvent(params object[] data)
    {
        base.HandleEvent(data);
    }

    protected override void OnRequestCallBack(HTTPRequest request, HTTPResponse response)
    {
        base.OnRequestCallBack(request, response);
    }
}
