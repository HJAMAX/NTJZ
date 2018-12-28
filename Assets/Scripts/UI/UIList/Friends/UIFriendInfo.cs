using UnityEngine;
using BestHTTP;

public class UIFriendInfo : AbstractSendMsgEventHandler
{
    private PlayerData playerData;

    [SerializeField]
    private UILabel nameLabel;

    /// <summary>
    /// 不同性别显示
    /// </summary>
    [SerializeField]
    private GameObject[] sexIcon;

    /// <summary>
    /// 不同功能显示，0为添加，1为赠与
    /// </summary>
    [SerializeField]
    private GameObject[] funcIcon;

    [SerializeField]
    private string webLocation;

    void OnDisable()
    {
        sexIcon[0].SetActive(false);
        sexIcon[1].SetActive(false);
        funcIcon[0].SetActive(false);
        funcIcon[1].SetActive(false);
    }

    /// <summary>
    /// 拼装好友或玩家信息UI
    /// </summary>
    /// <param name="data"></param>
    public void SetPlayerData(PlayerData data, int funcIndex)
    {
        playerData = data;

        nameLabel.text = playerData.nicheng;
        int sexIndex = int.Parse(playerData.sex);
        sexIcon[sexIndex].SetActive(true);
        funcIcon[funcIndex].SetActive(true);
    }

    /// <summary>
    /// 给该组件包含的玩家发送邮件
    /// </summary>
    public void SendMail()
    {
        Game.Instance.EventBus.SendEvents("open_ui", "SendMailPanel");
        Game.Instance.EventBus.SendEvents("send_mail", 
                                           playerData.user_id.ToString(),
                                           playerData.nicheng);
    }

    /// <summary>
    /// 发送好友请求
    /// </summary>
    public void SendFriendRequest()
    {
        OnRequestFinishedDelegate callback = OnRequestCallback;
        SendEvent(new PostData[] { new PostData("sender_id", GameData.playerData.user_id),
                                   new PostData("sender_nicheng", GameData.playerData.nicheng),
                                   new PostData("receiver_id", playerData.user_id),
                                   new PostData("receiver_nicheng", playerData.nicheng)}, webLocation, callback);
    }

    void OnRequestCallback(HTTPRequest request, HTTPResponse response)
    {
        base.OnRequestCallBack(request, response);
        JsonWrapperArray<string> wrapper = JsonWrapperArray<string>.FromJson<string>(response.DataAsText.ToString());

        Game.Instance.EventBus.SendEvents("msg_popup", wrapper.msg);
    }
}