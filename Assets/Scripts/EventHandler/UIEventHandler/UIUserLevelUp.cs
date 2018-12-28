using UnityEngine;

/// <summary>
/// 用于用户升级的回调
/// </summary>
public class UIUserLevelUp : AbstractSendMsgEventHandler
{
    /// <summary>
    /// 申请审核的地址
    /// </summary>
    public string applyLocation;

    /// <summary>
    /// 申请直接升级的地址
    /// </summary>
    public string payLocation;

    [SerializeField]
    private string callbackName;

    /// <summary>
    /// 每个等级的名字
    /// </summary>
    [SerializeField]
    private string[] levelNames;

    private UIButtonContainer uiContainer;

    private PostEvent postEventHandler;

    void Start()
    {
        uiContainer = transform.parent.GetComponent<UIButtonContainer>();
        postEventHandler = transform.parent.GetComponent<PostEvent>();
    }

    void OnEnable()
    {
        HandleEvent(null);
    }

    public override void HandleEvent(params object[] data)
    {
        if (data == null || data.Length == 0)
        {
            string text = levelNames[GameData.playerData.game_leaguer_rank];
            if (GameData.upgradeData.application_rank > 1)
            {
                string tempText = levelNames[GameData.playerData.game_leaguer_rank] + "(" + levelNames[GameData.upgradeData.application_rank];
                switch (GameData.upgradeData.check_type)
                {
                    case 0 : text =  tempText + " 正在申请中)";break;
                    case 1 : text = GameData.upgradeData.is_pay == 1 ? text = tempText + " 已支付，请耐心等待终审)"  : text = tempText + " 已通过初审，可支付)";
                             break;
                    case 2: text = tempText + " 条件不符合，已拒绝)"; break;
                    case 3: text = tempText + " 已通过终审, 下个月自动升级)"; break;
                }
            }
            else if(GameData.upgradeData.application_rank == 1)
            {
                GameData.playerData.game_leaguer_rank = GameData.upgradeData.application_rank;
                GameData.upgradeData.application_rank = 0;
                text = levelNames[GameData.playerData.game_leaguer_rank];
            }

            GetComponent<UILabel>().text = text;
        }
        else if (data.Length > 0 && data[0].GetType() == typeof(string))
        {
            GameData.playerData.game_leaguer_rank = int.Parse((string)data[0]);
            GetComponent<UILabel>().text = levelNames[GameData.playerData.game_leaguer_rank];
        }
    }

    /// <summary>
    /// 是否可以升级
    /// </summary>
    public void CheckCanUpgrade(string dataName, string levelStr, string openUIName, string webLocation)
    {
        int nextLevel = int.Parse(levelStr);
        //支付升级
        if ((GameData.upgradeData.check_type == 1 && GameData.upgradeData.application_rank == nextLevel) ||
            (GameData.playerData.game_leaguer_rank == 0 && nextLevel == 1))
        {
            postEventHandler.ResetPostMap();
            postEventHandler.AddPostData("type", "upgrade");
            postEventHandler.UpdateWebInfo("Wechatpay/wechatPay", "wx_pay");
            if (nextLevel == 1)
            {
                postEventHandler.AddPostData("order_name", "vip");
                return;
            }
            else if (nextLevel == 2)
                postEventHandler.AddPostData("order_name", "ds");
            else
                postEventHandler.AddPostData("order_name", "dc");

            
            uiContainer.OnClickOpen("PayPanel");
            return;
        }
        //申请升级
        if((GameData.playerData.game_leaguer_rank >= nextLevel) ||
           (GameData.playerData.game_leaguer_rank == 0 && nextLevel > 1) ||
           (GameData.upgradeData.application_rank > 0))
        {
            return;
        }

        uiContainer.OnClickOpen(openUIName);
        postEventHandler.AddPostData(dataName, levelStr);
        postEventHandler.UpdateWebInfo(webLocation, callbackName);
    }

    public void CheckCanUpgradeVIP(string levelStr)
    {
        int nextLevel = int.Parse(levelStr);

        if (GameData.playerData.game_leaguer_rank == 0 && nextLevel == 1)
        {
            uiContainer.OnClickOpen("ToVIP");
        }
    }
}
