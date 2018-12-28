using UnityEngine;
using BestHTTP;
using System.Collections.Generic;

public class ExchangeWineCoinsEventHandler : AbstractSendMsgEventHandler
{
    [SerializeField]
    private string webLocation;

    public override void HandleEvent(params object[] data)
    {
        base.HandleEvent(data);
        PostData[] postDataList = { new PostData("user_id", GameData.playerData.user_id)};
        SendEvent(postDataList, webLocation, OnRequestCallBack);
    }

    protected override void OnRequestCallBack(HTTPRequest request, HTTPResponse response)
    {
        base.OnRequestCallBack(request, response);

        JsonWrapperArray<string> wrapper = JsonWrapperArray<string>.FromJson<string>(response.DataAsText);

        if (wrapper.state == "1")
        {
            string data = wrapper.items[0];
            Dictionary<string, string> dataMap = JsonWrapperArray<string>.FromJsonMap(data);
            GameData.playerData.wine_coins = float.Parse(dataMap["wine_coins"]);
            //GameData.playerChateauData.totalWineStored = dataMap["total_wine_stored"];

            Game.Instance.EventBus.SendEvents("wine_coins_info", GameData.playerData.wine_coins.ToString());
            Game.Instance.EventBus.SendEvents("wine_maker");
            //Game.Instance.EventBus.SendEvents("total_wine_info", dataMap["total_wine_stored"]);
        }
        else
        {
            Game.Instance.EventBus.SendEvents("msg_popup", wrapper.msg);
        }
    }
}