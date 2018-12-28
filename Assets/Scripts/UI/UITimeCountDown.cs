using System.Collections;
using UnityEngine;
using System;

public class UITimeCountDown : MonoBehaviour
{
    private UILabel timerText;

    private int curHour;

    private int curMin;

    private int curSec;

    /// <summary>
    /// 显示酿酒的状态
    /// </summary>
    private string labelText;
    
    void OnEnable()
    {
        timerText = GetComponent<UILabel>();
    }

    void OnDisable()
    {
        StopAllCoroutines();
    }

    public bool isAvailable()
    {
        //return curHour == 0 && curMin < 3 && curMin >= 0 && curSec > 0;
        return curHour <= 0 && curMin <= 0  && curSec <= 0;
    }

    public void StartCountDown(int h, int m, int s)
    {
        curHour = h;
        curMin = m;
        curSec = s;
        //timerText.text = h + ":" + m + ":" + s;
        IEnumerator coroutine = CountDown();
        StartCoroutine(coroutine);
    }

    public void StartCountDown(int restSeconds)
    {
        switch (GetComponent<BrewbotTimerEventHandler>().State)
        {
            case BrewbotState.PROGRESSING: labelText = "酿酒中："; break;
            case BrewbotState.WAITING: labelText = "离酿酒开始还有："; break;
        }

        StopAllCoroutines();
        curHour = restSeconds / 3600;
        curMin = (restSeconds % 3600) / 60;
        curSec = restSeconds % 60;

        IEnumerator coroutine = CountDown();
        StartCoroutine(coroutine);
    }

    public void StartCountDownMinute(int restSeconds)
    {
        labelText = "入库中：";
        StopAllCoroutines();
        curMin = (restSeconds % 3600) / 60;
        curSec = restSeconds % 60;

        IEnumerator coroutine = CountDownMinute();
        StartCoroutine(coroutine);
    }

    private IEnumerator CountDown()
    {
        while (curHour >= 0 && curMin >= 0 && curSec >= 0)
        {
            timerText.text = labelText + curHour + ":" + curMin + ":" + curSec;
            curSec--;

            if (curSec < 0)
            {
                if (curMin == 0)
                {
                    curHour--;
                    curMin = 59;
                }
                else
                {
                    curMin--;
                }

                curSec = 59;
            }

            yield return new WaitForSeconds(1.0f);
        }

        curHour = 0;
        switch (GetComponent<BrewbotTimerEventHandler>().State)
        {
            case BrewbotState.PROGRESSING:
                labelText = "入库中：";
                StartCountDownMinute(GameData.secWaitWine);
                break;
            case BrewbotState.WAITING:
                GetComponent<BrewbotTimerEventHandler>().UpdateState(BrewbotState.PROGRESSING);
                TimeSpan ts = TimeUtil.GetTimeDiff(GameData.playerChateauData.wineStopTime);
                StartCountDown((int)ts.TotalSeconds);
                break;
        }
    }

    private IEnumerator CountDownMinute()
    {
        GetComponent<BrewbotTimerEventHandler>().UpdateState(BrewbotState.STEALABLE);
        timerText.color = Color.red;

        while(curMin >= 0 && curSec >= 0)
        {
            timerText.text = labelText + 0 + ":" + curMin + ":" + curSec;
            curSec--;

            if (curSec < 0)
            {
                curMin--;
                curSec = 59;
            }

            yield return new WaitForSeconds(1.0f);
        }

        timerText.color = Color.black;
        GetComponent<BrewbotTimerEventHandler>().UpdateState(BrewbotState.AVAILABLE);
        Game.Instance.EventBus.SendEvents("exchange_wine_coins");
    }
}
