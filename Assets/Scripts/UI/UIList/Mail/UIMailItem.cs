using UnityEngine;

public class UIMailItem : MonoBehaviour
{
    public uint mailId;

    /// <summary>
    /// 邮件种类：系统/玩家
    /// </summary>
    [SerializeField]
    private string[] mailTypeArray;

    /// <summary>
    /// 是否被点中
    /// </summary>
    [SerializeField]
    private GameObject isPickedSprite;
    
    /// <summary>
    /// 邮件种类UI标签
    /// </summary>
    [SerializeField]
    private UILabel typeLabel;

    /// <summary>
    /// 邮件标题UI标签
    /// </summary>
    [SerializeField]
    private UILabel titleLabel;

    /// <summary>
    /// 是否已读标签
    /// </summary>
    [SerializeField]
    private GameObject isReadIcon;

    private UIMailDataManager mailDataMgr;

    private MailData mailData;

    /// <summary>
    /// 设置是否已读，邮件种类，标题
    /// </summary>
    /// <param name="mailData"></param>
    public void SetMailUI(MailData data)
    {
        mailData = data;

        mailId = mailData.mail_id;
        isReadIcon.SetActive(!mailData.is_checked);
        typeLabel.text = mailTypeArray[mailData.mail_type];
        titleLabel.text = mailData.title;

        //默认为未被选中
        isPickedSprite.SetActive(false);
        mailDataMgr = transform.parent.GetComponent<UIMailDataManager>();
    }

    public void ShowContent()
    {
        //已经发送过请求了
        if(isPickedSprite.activeInHierarchy)
        {
            return;
        }

        isReadIcon.SetActive(false);
        //高亮被选中的邮件
        transform.parent.GetComponent<UITagContainer>().LightUp(isPickedSprite);
        //赋值被选中的邮件
        mailDataMgr.curChosenMail = mailData;
        Game.Instance.EventBus.SendEvents("mail_content", mailId);

        string typeStr = "sys";
        int userId = int.Parse(GameData.playerData.user_id);
        if (mailData.receiver_id == userId)
            typeStr = "player";
        else if (mailData.sender_id == userId)
            typeStr = "send";
        mailDataMgr.MarkIsChecked(typeStr, mailData.mail_id);
    }
}
