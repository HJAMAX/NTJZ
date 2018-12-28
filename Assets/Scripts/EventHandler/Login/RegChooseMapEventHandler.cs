using BestHTTP;
using UnityEngine;

public class RegChooseMapEventHandler : AbstractSendMsgEventHandler
{
    [SerializeField]
    private string webLocation;

    [SerializeField]
    private string callbackOpenPanel;

    public void UpdateMapId()
    {
        OnRequestFinishedDelegate callback = OnRequestCallBack;
        PostData[] postDataList = {new PostData("user_id", GameData.playerData.user_id),
                                   new PostData("map_id", GameData.chosenMapId.ToString())};

        SendEvent(postDataList, webLocation, callback);
    }

    protected override void OnRequestCallBack(HTTPRequest request, HTTPResponse response)
    {
        base.OnRequestCallBack(request, response);

        JsonWrapperArray<string> wrapper = JsonWrapperArray<string>.FromJson<string>(response.DataAsText);
        if(wrapper.state == "1")
        {
            Game.Instance.EventBus.SendEvents("open_ui", callbackOpenPanel);
            gameObject.SetActive(false);
        }
    }
}
