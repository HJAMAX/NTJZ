using UnityEngine;
using BestHTTP;
using System.Collections.Generic;

public class UIMailDataManager : AbstractSendMsgEventHandler
{
    [SerializeField]
    private string webLocation;

    /// <summary>
    /// 邮件asset名
    /// </summary>
    [SerializeField]
    private string spawnName;

    /// <summary>
    /// 当前被选中的邮件，用于回复
    /// </summary>
    public MailData curChosenMail;

    private List<MailData> sysMailList = new List<MailData>();

    private List<MailData> playerMailList = new List<MailData>();

    private List<MailData> sentMailList = new List<MailData>();

    /// <summary>
    /// 滚动显示，在此应用用于复位
    /// </summary>
    private UIScrollView scrollView;

    void Awake()
    {
        scrollView = transform.parent.GetComponent<UIScrollView>();
    }

    public void Reply()
    {
        if(curChosenMail.mail_type == 0)
        {
            Game.Instance.EventBus.SendEvents("msg_popup", "不能回复系统邮件");
            return;
        }
        else if(curChosenMail.sender_id == int.Parse(GameData.playerData.user_id))
        {
            Game.Instance.EventBus.SendEvents("msg_popup", "不能回复已发送邮件");
            return;
        }
        else if(curChosenMail.mail_type == 2)
        {
            Game.Instance.EventBus.SendEvents("friend_request", curChosenMail.sender_id, curChosenMail.sender_nicheng);
            return;
        }

        Game.Instance.EventBus.SendEvents("open_ui", "SendMailPanel");
        Game.Instance.EventBus.SendEvents("send_mail", 
                                           curChosenMail.sender_id.ToString(),
                                           curChosenMail.sender_nicheng);
    }

    public void GetMailList()
    {
        OnRequestFinishedDelegate callback = OnRequestCallback;
        SendEvent(new PostData[] { new PostData("user_id", GameData.playerData.user_id)}, webLocation, callback);
    }

    /// <summary>
    /// 去掉未读标记
    /// </summary>
    /// <param name="data"></param>
    public void MarkIsChecked(string typeStr, uint mailId)
    {
        List<MailData> list = GetTypeList(typeStr);
        for(int i = 0; i < list.Count; i++)
        {
             if(list[i].mail_id == mailId)
             {
                MailData mailData = list[i];
                mailData.is_checked = true;
                list[i] = mailData;
                return;
             }
        }
    }

    /// <summary>
    /// UI显示对应的邮件种类
    /// </summary>
    /// <param name="type"></param>
    public void ShowMails(string typeStr)
    {
        //滚轴复位
        scrollView.ResetPosition();

        List<MailData> list = GetTypeList(typeStr);
        Game.Instance.ObjectPool.UnspawnAll();
        for(int i = 0; i < list.Count; i++)
        {
            GameObject mailItem = Game.Instance.ObjectPool.Spawn(spawnName);
            mailItem.transform.parent = transform;
            mailItem.transform.localScale = Vector3.one;
            mailItem.GetComponent<UIMailItem>().SetMailUI(list[i]);
        }
        GetComponent<UIGrid>().enabled = true;
    }

    /// <summary>
    /// 把邮件放入不同的列表里
    /// </summary>
    /// <param name="request"></param>
    /// <param name="response"></param>
    void OnRequestCallback(HTTPRequest request, HTTPResponse response)
    {
        base.OnRequestCallBack(request, response);
        JsonWrapperArray<MailData> wrapper = JsonWrapperArray<MailData>.FromJson<MailData>(response.DataAsText.ToString());
        
        if(wrapper.state == "1")
        {
            sysMailList.Clear();
            playerMailList.Clear();
            sentMailList.Clear();

            int userId = int.Parse(GameData.playerData.user_id);
            //生成邮件列表UI
            MailData[] items = wrapper.items;
            for(int i = 0; i < items.Length; i++)
            {
                if (items[i].sender_id == userId)
                    sentMailList.Add(items[i]);
                else if (items[i].mail_type == 0)
                    sysMailList.Add(items[i]);
                else
                    playerMailList.Add(items[i]);
            }
            ShowMails("sys");
        }
    }

    private List<MailData> GetTypeList(string typeStr)
    {
        if (typeStr == "sys")
            return sysMailList;
        else if (typeStr == "player")
            return playerMailList;
        else
            return sentMailList;
    }
}
