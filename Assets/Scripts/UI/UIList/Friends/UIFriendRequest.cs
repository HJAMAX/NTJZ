using UnityEngine;
using BestHTTP;

public class UIFriendRequest : AbstractSendMsgEventHandler
{
    [SerializeField]
    private string content = "确定添加该玩家为好友？";

    [SerializeField]
    private string webLocation;

    private UILabel msgLabel;

    private uint friendId;

    private string friendNicheng;

    public void SendFriendRequest()
    {
        OnRequestFinishedDelegate callback = OnRequestCallback;
        SendEvent(new PostData[] { new PostData("friend_id", friendId.ToString()),
                                   new PostData("friend_nicheng", friendNicheng),
                                   new PostData("user_id", GameData.playerData.user_id),
                                   new PostData("nicheng", GameData.playerData.nicheng)}, webLocation, callback);
    }

    /// <summary>
    /// 弹出消息框并复制好友的ID和昵称
    /// </summary>
    /// <param name="data"></param>
    public override void HandleEvent(params object[] data)
    {
        if (msgLabel == null)
        {
            msgLabel = transform.Find("MsgLabel").GetComponent<UILabel>();
        }

        gameObject.SetActive(true);
        msgLabel.text = content;

        friendId = (uint)data[0];
        friendNicheng = (string)data[1];
    }

    void OnRequestCallback(HTTPRequest request, HTTPResponse response)
    {
        base.OnRequestCallBack(request, response);
        JsonWrapperArray<string> wrapper = JsonWrapperArray<string>.FromJson<string>(response.DataAsText.ToString());
        gameObject.SetActive(false);
        Game.Instance.EventBus.SendEvents("msg_popup", wrapper.msg);
    }
}
