using BestHTTP;

public class InitPlayerInfoEventHandler : AbstractSendMsgEventHandler
{
    public void OnClick()
    {
        Game.Instance.EventBus.SendEvents("player_login", 1);
    }

    public override void HandleEvent(params object[] data)
    {
        base.HandleEvent(data);

        OnRequestFinishedDelegate callback = OnRequestCallBack;
        Game.Instance.EventBus.SendEvents("post", "player/guestLogin", callback);
    }

    protected override void OnRequestCallBack(HTTPRequest request, HTTPResponse response)
    {
        base.OnRequestCallBack(request, response);

        JsonWrapperArray<PlayerData> wrapper = JsonWrapperArray<PlayerData>.FromJson<PlayerData>(response.DataAsText);
        GameData.playerData = wrapper.items[0];

        GetComponent<SceneSwitcher>().GoMyChateau();
    }
}
