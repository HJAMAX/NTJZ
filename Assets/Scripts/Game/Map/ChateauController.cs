using UnityEngine;
using System;

/// <summary>
/// 点击酒庄图标进入战斗场景
/// </summary>
public class ChateauController : MonoBehaviour
{
    public string id;

    public string nicheng;

    /// <summary>
    /// 显示昵称
    /// </summary>
    private TextMesh textObj;

    /// <summary>
    /// 显示倒计时
    /// </summary>
    private TextMesh timerText;

    /// <summary>
    /// 显示进度条
    /// </summary>
    private Transform progressBar;

    private int curHour;

    private int curMin;

    private int curSec;

    /// <summary>
    /// 离开始酿酒的时间
    /// </summary>
    private int secondsFromStart;

    /// <summary>
    /// 离酿酒结束的时间
    /// </summary>
    private int totalSeconds;

    public int TotalSeconds { get{ return totalSeconds; } }

    /// <summary>
    /// 剩余酒量百分比
    /// </summary>
    private string wineLeftPer;

    public int WineLeftPer { get { return int.Parse(wineLeftPer); } }

    /// <summary>
    /// 储存的酒量
    /// </summary>
    private string wineStored;

    public int WineStored { get { return int.Parse(wineStored); } }

    void Awake()
    {
        textObj = transform.Find("Text").GetComponent<TextMesh>();
        timerText = transform.Find("Timer").GetComponent<TextMesh>();
        progressBar = timerText.transform.Find("Mask");
    }

    void OnDisable()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
        CancelInvoke();
    }

    public void SetFlag(string flagStr, bool isActive)
    {
        transform.Find(flagStr).gameObject.SetActive(isActive);
    }

    /// <summary>
    /// 是否战斗过
    /// </summary>
    /// <returns></returns>
    public bool HasFlag()
    {
        return transform.Find("win_flag").gameObject.activeInHierarchy ||
               transform.Find("fail_flag").gameObject.activeInHierarchy;
    }

    /// <summary>
    /// 初始化并开始显示倒计时
    /// </summary>
    /// <param name="i"></param>
    /// <param name="n"></param>
    /// <param name="stopTime"></param>
    public void Init(string i, string n, string startTime, string stopTime, 
                     string ws, string wlp)
    {
        id = i;
        textObj.text = nicheng = n;
        textObj.gameObject.SetActive(true);
        wineStored = ws;
        wineLeftPer = wlp;

        secondsFromStart = (int)TimeUtil.GetTimeFromNow(startTime).TotalSeconds;
        totalSeconds = (int)TimeUtil.GetTimeFromNow(stopTime).TotalSeconds;

        if (secondsFromStart < 0 && totalSeconds > 0)
        {
            timerText.gameObject.SetActive(true);
            UpdateProgressBar();

            curHour = totalSeconds / 3600;
            curMin = totalSeconds % 3600 / 60;
            curSec = totalSeconds % 60;
            InvokeRepeating("CountDown", 0.0f, 1.0f);
        }
    }

    public bool IsProgressing()
    {
        return totalSeconds > 0 && secondsFromStart < 0;
    }

    /// <summary>
    /// 是否长时间没酿酒
    /// </summary>
    /// <returns></returns>
    public bool IsLongTimeNoWine()
    {
        TimeSpan ts = TimeUtil.GetTimeDiff(GameData.playerChateauData.wineStopTime);
        return (int)ts.TotalSeconds > 86400;
    }

    /// <summary>
    /// 开始倒计时
    /// </summary>
    private void CountDown()
    {
        if(curHour >= 0 && curMin >= 0 && curSec >= 0)
        {
            totalSeconds--;
            timerText.text = curHour + ":" + curMin + ":" + curSec;
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
                UpdateProgressBar();
            }
        }
        else
        {
            CancelInvoke();
        }
    }

    private void UpdateProgressBar()
    {
        progressBar.localScale = new Vector3((float)totalSeconds / GameData.timeMakeWine * 2, 2, 1);
    }
}
