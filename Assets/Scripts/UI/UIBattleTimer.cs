using UnityEngine;
using System.Collections;

public class UIBattleTimer : MonoBehaviour
{
    [SerializeField]
    private int totalSeconds;

    private int curMin;

    private int curSec;

    private UILabel timerText;

    void Awake()
    {
        timerText = GetComponent<UILabel>();
    }

    //开始计时
    void Start()
    {
        curMin = totalSeconds / 60;
        curSec = totalSeconds % 60;

        IEnumerator coroutine = CountDown();
        StartCoroutine(coroutine);
    }

    private IEnumerator CountDown()
    {
        while (curMin >= 0 && curSec >= 0)
        {
            timerText.text = curMin + ":" + curSec;
            curSec--;

            if(curSec < 0)
            {
                curMin--;
                curSec = 59;
            }

            yield return new WaitForSeconds(1.0f);
        }

        Game.Instance.EventBus.SendEvents("battle_end", "defend_succeeded");
    }
}
