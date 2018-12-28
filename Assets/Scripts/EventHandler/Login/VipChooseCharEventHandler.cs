using BestHTTP;
using UnityEngine;

public class VipChooseCharEventHandler : AbstractSendMsgEventHandler
{
    [SerializeField]
    private string webLocation;

    public void VipChooseCharacter(string price, string level)
    {
        OnRequestFinishedDelegate callback = OnVipChooseCharacterCallback;
        PostData[] postDataList = {new PostData("user_id", GameData.playerData.user_id),
                                   new PostData("gold_coins", price),
                                   new PostData("level", level)};

        SendEvent(postDataList, webLocation, callback);
    }

    void OnVipChooseCharacterCallback(HTTPRequest request, HTTPResponse response)
    {
        base.OnRequestCallBack(request, response);

        JsonWrapperArray<string> wrapper = JsonWrapperArray<string>.FromJson<string>(response.DataAsText);
        GameData.playerData = JsonUtility.FromJson<PlayerData>(wrapper.items[0]);
        GameData.itemsData = JsonUtility.FromJson<ItemsData>(wrapper.items[1]);

        Game.Instance.LoadingScene(GameData.myChateauIndex);
    }
}
