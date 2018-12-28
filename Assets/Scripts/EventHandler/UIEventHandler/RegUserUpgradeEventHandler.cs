using BestHTTP;
using UnityEngine;

public class RegUserUpgradeEventHandler : AbstractSendMsgEventHandler
{
    /// <summary>
    /// 用户要提升至的等级
    /// </summary>
    [SerializeField]
    private string nextLevel;

    [SerializeField]
    private string webLocation;

    [SerializeField]
    private string callbackUIName;

    public void Upgrade()
    {
        OnRequestFinishedDelegate callback = OnRequestCallBack;
        PostData[] postDataList = { new PostData("user_id", GameData.playerData.user_id),
                                    new PostData("next_level", nextLevel ) };
        Game.Instance.EventBus.SendEvents("post", webLocation, callback, postDataList);
    }

    protected override void OnRequestCallBack(HTTPRequest request, HTTPResponse response)
    {
        base.OnRequestCallBack(request, response);
        JsonWrapperArray<string> wrapper = JsonWrapperArray<string>.FromJson<string>(response.DataAsText);
        GameData.playerData.game_leaguer_rank = int.Parse(wrapper.items[0]);

        transform.Find(callbackUIName).gameObject.SetActive(true);
    }
}
