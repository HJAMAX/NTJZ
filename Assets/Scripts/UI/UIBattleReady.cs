using System.Collections;
using UnityEngine;

public class UIBattleReady : EventHandler
{
    private GameObject joinBtn;

    private GameObject leaveBtn;

    private UILabel numberLabel;

    private UILabel timerLabel;
    
    private int curMin;

    private int curSec;

    void Awake()
    {
        Transform trans = transform.Find("Join");
        joinBtn = trans == null ? null : trans.gameObject;

        leaveBtn = transform.Find("Leave").gameObject;
        numberLabel = transform.Find("Number").GetComponent<UILabel>();
        timerLabel = transform.Find("Timer").GetComponent<UILabel>();
    }

    public override void HandleEvent(params object[] data)
    {
        gameObject.SetActive(true);

        if(joinBtn == null)
        {
            leaveBtn.SetActive(true);
        }
        else
        {
            joinBtn.SetActive(!GameData.isDefender);
            leaveBtn.SetActive(GameData.isDefender);
        }

        int restTime = (int)(GameData.nextBattleStartTime - TimeUtil.GetCurrentTimeStampLong());

        if(restTime < 0)
        {
            numberLabel.text = "0";
        }
        else if (restTime <= 180)
        {
            if (GameData.playerCount > 0)
            {
                //开始倒计时
                timerLabel.gameObject.SetActive(true);
                StartCountDown(restTime);
                //显示人数
                numberLabel.text = GameData.playerCount.ToString();
                //报警
                Game.Instance.EventBus.SendEvents("alarm", true);
            }
            else
            {
                StopAllCoroutines();
                timerLabel.gameObject.SetActive(false);
                numberLabel.text = "0";

                Game.Instance.EventBus.SendEvents("alarm", false);
            }
        }
    }

    public void DisablePanel()
    {
        gameObject.SetActive(false);
        //GetComponent<UIPanel>().enabled = false;
    }

    public void StartCountDown(int restSeconds)
    {
        curMin = restSeconds / 60;
        curSec = restSeconds % 60;

        StopAllCoroutines();
        IEnumerator coroutine = CountDown();
        StartCoroutine(coroutine);
    }

    private IEnumerator CountDown()
    {
        //string text = isHost ? " 可以点击开始" : " 请耐心等待";
        while (curMin >= 0 && curSec >= 0)
        {
            timerLabel.text = "偷酒倒计时 " + curMin + ":" + curSec;
            curSec--;

            if (curSec < 0)
            {
                curMin--;

                curSec = 59;
            }

            yield return new WaitForSeconds(1.0f);
        }

        timerLabel.gameObject.SetActive(false);
        //startBtn.SetActive(isHost);
    }
}
