using UnityEngine;
using BestHTTP;

public class UISendMailContent : AbstractSendMsgEventHandler
{
    [SerializeField]
    private string webLocation;

    /// <summary>
    /// 收件人昵称
    /// </summary>
    private UILabel receiverName;

    /// <summary>
    /// 信件内容
    /// </summary>
    private UILabel content;

    /// <summary>
    /// 邮件标题
    /// </summary>
    private UILabel title;

    #region 发送邮件相关

    private string receiverId;

    private string receiverNicheng;
    
    #endregion

    void Awake()
    {
        receiverName = transform.Find("ReceiverName").GetComponent<UILabel>();
        content = transform.Find("Content").Find("Label").GetComponent<UILabel>();
        title = transform.Find("TitleName").Find("Label").GetComponent<UILabel>();
    }

    /// <summary>
    /// 初始化邮件
    /// </summary>
    /// <param name="data"></param>
    public override void HandleEvent(params object[] data)
    {
        if (data.Length > 0)
        {
            receiverId = (string)data[0];
            receiverNicheng = receiverName.text = (string)data[1];
        }
    }

    public void SendMail()
    {
        if(receiverName.text == "" || receiverName.text == null ||
           content.text == "" || content.text == null ||
           title.text == "" || title.text == null)
        {
            Game.Instance.EventBus.SendEvents("MsgPopup", "收件人，内容，标题不能为空");
            return;
        }

        OnRequestFinishedDelegate callback = OnRequestCallback;
        SendEvent(new PostData[] { new PostData("sender_id", GameData.playerData.user_id),
                                   new PostData("sender_nicheng", GameData.playerData.nicheng),
                                   new PostData("receiver_id", receiverId),
                                   new PostData("receiver_nicheng", receiverNicheng),
                                   new PostData("content", content.text),
                                   new PostData("title", title.text)}, webLocation, callback);
    }

    void OnRequestCallback(HTTPRequest request, HTTPResponse response)
    {
        base.OnRequestCallBack(request, response);
        JsonWrapperArray<string> wrapper = JsonWrapperArray<string>.FromJson<string>(response.DataAsText.ToString());
        Game.Instance.EventBus.SendEvents("msg_popup", wrapper.msg);
        gameObject.SetActive(false);
    }
}
