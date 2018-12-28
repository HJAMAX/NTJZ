using UnityEngine;

public class UITimeUpdate : MonoBehaviour
{
    private UILabel timeLabel;

    private int totalSeconds;

    private float timeAccumulate = 0f;

    void Update()
    {
        timeAccumulate += Time.deltaTime;

        if(timeAccumulate >= 1f && totalSeconds > 0)
        {
            totalSeconds--;
            int curHour = totalSeconds / 3600;
            int curMin = (totalSeconds % 3600) / 60;
            int curSec = totalSeconds % 60;
            timeLabel.text = "还剩 " + curHour + ":" + curMin + ":" + curSec;

            timeAccumulate = 0f;
        }
        else if(totalSeconds <= 0)
        {
            transform.parent.Find("Button").gameObject.SetActive(true);
            timeLabel.text = "未聘请";
            this.enabled = false;
        }
    }

    void OnEnable()
    {
        timeLabel = GetComponent<UILabel>();
    }

    public void StartCountDown(int totalSeconds)
    {
        this.totalSeconds = totalSeconds;
        timeAccumulate = 1f;
    }
}
