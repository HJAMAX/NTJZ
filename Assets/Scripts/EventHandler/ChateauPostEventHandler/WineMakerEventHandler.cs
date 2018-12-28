using UnityEngine;
using BestHTTP;
using System.Collections.Generic;

/// <summary>
/// 用于将后端传来的信息显示给用户
/// </summary>
public class WineMakerEventHandler : AbstractSendMsgEventHandler
{
    [SerializeField]
    private UIAmount goldCoins;

    [SerializeField]
    private BrewbotTimerEventHandler brewbotTimer;

    private UIButtonContainer uiContainer;

    private TextEventHandler makeWineInfoPanel;

    /// <summary>
    /// 开始酿酒的时间
    /// </summary>
    private string startTime = "-1";

    void Awake()
    {
        uiContainer = GetComponent<UIButtonContainer>();
        makeWineInfoPanel = transform.Find("Making").GetComponent<TextEventHandler>();
    }

    public override void HandleEvent(params object[] data)
    {
        if(brewbotTimer.State == BrewbotState.PROGRESSING ||
           brewbotTimer.State == BrewbotState.DONE ||
           brewbotTimer.State == BrewbotState.STEALABLE ||
           brewbotTimer.State == BrewbotState.WAITING)
        {
            uiContainer.OnClickOpen("Making");
            uiContainer.OnClickClose("Start");
            makeWineInfoPanel.SetText(GameData.playerChateauData.wineStored + " ml");
        }
        else if(brewbotTimer.State == BrewbotState.AVAILABLE)
        {
            uiContainer.OnClickOpen("Start");
            uiContainer.OnClickClose("Making");
        }
    }

    /// <summary>
    /// 点击发送，开始酿酒
    /// </summary>
    /// <param name="data"></param>
    public void MakeWine()
    {
        int goldAmount = int.Parse(goldCoins.GetAmountStr());
        if(goldAmount < GameData.leastCoinMakeWine)
        {
            Game.Instance.EventBus.SendEvents("msg_popup", "酿酒不得少于" + GameData.leastCoinMakeWine + "金币");
            return;
        }

        uiContainer.OnClickOpen("Making");
        uiContainer.OnClickClose("Start");

        base.HandleEvent();
        PostData[] postDataList = { new PostData("user_id", GameData.playerData.user_id),
                                    new PostData("gold_coins", goldCoins.GetAmountStr()),
                                    new PostData("start_time", startTime)};
        SendEvent(postDataList, "chateau/makeWine", MakeWineCallback);
    }

    /// <summary>
    /// 设置酿酒开始时间
    /// </summary>
    /// <param name="postInfo"></param>
    public void SetStartTime(string s)
    {
        startTime = s;
    }

    private void MakeWineCallback(HTTPRequest request, HTTPResponse response)
    {
        base.OnRequestCallBack(request, response);

        JsonWrapperArray<string> wrapper = JsonWrapperArray<string>.FromJson<string>(response.DataAsText);

        if (wrapper.state == "1")
        {
            string data = wrapper.items[0];

            Dictionary<string, string> dataMap = JsonWrapperArray<string>.FromJsonMap(data);
            GameData.playerChateauData.wineStartTime = dataMap["start_time"];
            GameData.playerChateauData.wineStopTime = dataMap["stop_time"];
            GameData.playerChateauData.wineStored = dataMap["wine_stored"];
            GameData.playerData.goldCoins = int.Parse(dataMap["gold_coins"]);

            Game.Instance.EventBus.SendEvents("brewbot_info");
            Game.Instance.EventBus.SendEvents("gold_coins_info", dataMap["gold_coins"]);
            HandleEvent();

            makeWineInfoPanel.SetText(dataMap["wine_stored"] + " ml");
        }
        else
        {
            Game.Instance.EventBus.SendEvents("msg_popup", wrapper.msg);
        }
    }
}