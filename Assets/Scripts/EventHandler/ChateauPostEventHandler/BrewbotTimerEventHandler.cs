using UnityEngine;
using BestHTTP;
using System.Collections.Generic;
using System;

public class BrewbotTimerEventHandler : AbstractSendMsgEventHandler
{
    [SerializeField]
    private string webLocation;

    [SerializeField]
    private UIProgress progressBar;

    [SerializeField]
    private UILabel timerText;

    private Dictionary<string, string> postDataMap = new Dictionary<string, string>();

    private BrewbotState state = BrewbotState.NOT_AVAILABLE;

    public BrewbotState State { get { return state; } }

    private Dictionary<BrewbotState, string> stateMap;

    public Dictionary<BrewbotState, string> StateMap
    {
        get
        {
            if (stateMap == null)
            {
                stateMap = new Dictionary<BrewbotState, string>();
                stateMap[BrewbotState.DONE] = "点击酒桶可取酒";
                stateMap[BrewbotState.AVAILABLE] = "酿酒器空闲";
                stateMap[BrewbotState.NOT_AVAILABLE] = "不可用";
                stateMap[BrewbotState.STEALABLE] = "正在入库";
            }
            return stateMap;
        }
    }

    public override void HandleEvent(params object[] data)
    {
        string wineStopTime = GameData.playerChateauData.wineStopTime;
        string wineStartTime = GameData.playerChateauData.wineStartTime;
        TimeSpan ts = TimeUtil.GetTimeDiff(wineStartTime);
        int totalSeconds = (int)ts.TotalSeconds;

        //获得当前时间月酿酒开始的时间差
        //大于现在
        if (totalSeconds < 0)
        {
            UpdateState(BrewbotState.WAITING, -totalSeconds);
            return;
        }
        
        //获得当前时间与酿酒结束的时间差
        ts = TimeUtil.GetTimeDiff(wineStopTime);
        totalSeconds = (int)ts.TotalSeconds;

        //大于现在
        if (totalSeconds < 0)
        {
            UpdateState(BrewbotState.PROGRESSING, -totalSeconds);

            int start = int.Parse(GameData.playerChateauData.wineStartTime);
            int stop = int.Parse(wineStopTime);
            progressBar.SetProgressBar(start, stop, stop - start + totalSeconds);
        }
        //小于等于现在
        else
        {
            int wineStored = int.Parse(GameData.playerChateauData.wineStored);

            //在可被偷的时间内
            if (totalSeconds < GameData.secWaitWine)
            {
                state = BrewbotState.STEALABLE;
                GetComponent<UITimeCountDown>().StartCountDownMinute(GameData.secWaitWine - totalSeconds);
            }
            else if (wineStored > 0)
            {
                UpdateState(BrewbotState.DONE);
            }
            else
            {
                UpdateState(BrewbotState.AVAILABLE);
            }
        }
    }

    public void OnClick()
    {
        //若酿酒器酒满了，则取酒
        if (state == BrewbotState.DONE)
        {
            PostData[] postDataList = { new PostData("user_id", GameData.playerData.user_id) };
            SendEvent(postDataList, webLocation, OnRequestCallBack);
        }
    }

    /// <summary>
    /// 更新状态：可取酒，空闲，不可用
    /// </summary>
    /// <param name="brewbotState"></param>
    public void UpdateState(BrewbotState brewbotState)
    {
        state = brewbotState;
        timerText.text = StateMap[state];
    }

    /// <summary>
    /// 更新状态：正在酿酒
    /// 开始倒计时
    /// </summary>
    /// <param name="brewbotState"></param>
    /// <param name="totalSeconds"></param>
    public void UpdateState(BrewbotState brewbotState, int totalSeconds)
    {
        state = brewbotState;
        GetComponent<UITimeCountDown>().StartCountDown(totalSeconds);
    }

    /// <summary>
    /// 更新UI：酿酒器的状态
    /// </summary>
    protected override void OnRequestCallBack(HTTPRequest request, HTTPResponse response)
    {
        base.OnRequestCallBack(request, response);

        FileIOUtil.Write("/Resources/Data/my_info.json", response.DataAsText);
        JsonWrapperArray<ChateauData> wrapper = JsonWrapperArray<ChateauData>.FromJson<ChateauData>(response.DataAsText);
        //发送消息
        ChateauData chateauData = wrapper.items[0];
        int wineIncreased = int.Parse(chateauData.totalWineStored) - int.Parse(GameData.playerChateauData.totalWineStored);
        Game.Instance.EventBus.SendEvents("msg_popup", "取得酒 " + wineIncreased + " ml");
        //更新数据
        GameData.playerChateauData = chateauData;
        Game.Instance.EventBus.SendEvents("brewbot_info");
        Game.Instance.EventBus.SendEvents("wine_maker");
        Game.Instance.EventBus.SendEvents("total_wine_info", chateauData.totalWineStored);
    }

    /// <summary>
    /// 刷新计时器
    /// </summary>
    /// <param name="focus"></param>
    private void OnApplicationPause(bool focus)
    {
        if (!focus)
        {
            HandleEvent();
        }
    }
}

/// <summary>
/// 酿酒器的状态
/// </summary>
public enum BrewbotState
{
    PROGRESSING,        //正在酿酒
    WAITING,            //还没到酿酒时间
    DONE,               //可取酒
    AVAILABLE,          //空闲，可开始酿酒
    NOT_AVAILABLE,      //没有酿酒师，不可用
    STEALABLE           //可被偷 
}