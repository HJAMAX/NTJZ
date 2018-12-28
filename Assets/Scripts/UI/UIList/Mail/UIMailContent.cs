using UnityEngine;
using BestHTTP;

public class UIMailContent : AbstractSendMsgEventHandler
{
    [SerializeField]
    private string webLocation;

    private GameObject senderLabel;

    private GameObject receiverLabel;

    private UILabel senderName;

    private UILabel receiverName;

    private UILabel deliverTime;

    private UILabel mailContent;

    void Awake()
    {
        senderLabel = transform.Find("SenderLabel").gameObject;
        receiverLabel = transform.Find("ReceiverLabel").gameObject;

        senderName = transform.Find("SenderName").GetComponent<UILabel>();
        receiverName = transform.Find("ReceiverName").GetComponent<UILabel>();

        deliverTime = transform.Find("DeliverTime").GetComponent<UILabel>();
        mailContent = transform.Find("Content").GetComponent<UILabel>();
    }

    public void Clear()
    {
        senderLabel.SetActive(false);
        receiverLabel.SetActive(false);
        receiverName.text = "";
        senderName.text = "";
        deliverTime.text = "";
        mailContent.text = "";
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="data"></param>
    public override void HandleEvent(params object[] data)
    {
        if (data.Length > 0 && data[0].GetType() == typeof(uint))
        {
            OnRequestFinishedDelegate callback = OnRequestCallback;
            SendEvent(new PostData[] { new PostData("mail_id", data[0].ToString()) }, webLocation, callback);
        }
    }

    /// <summary>
    /// 显示邮件内容
    /// </summary>
    /// <param name="request"></param>
    /// <param name="response"></param>
    void OnRequestCallback(HTTPRequest request, HTTPResponse response)
    {
        base.OnRequestCallBack(request, response);
        JsonWrapperArray<MailData> wrapper = JsonWrapperArray<MailData>.FromJson<MailData>(response.DataAsText.ToString());

        MailData mailData = wrapper.items[0];
        senderName.text = mailData.sender_nicheng;
        receiverName.text = mailData.receiver_nicheng;

        deliverTime.text = TimeUtil.GetTime(mailData.deliver_time).ToShortDateString();
        mailContent.text = mailData.content;

        senderLabel.SetActive(true);
        receiverLabel.SetActive(true);
    }
}
