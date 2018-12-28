using UnityEngine;
using System;

public class UIWineCoinsRecordTag : UIListElement
{
    public override void SetData(params object[] data)
    {
        try
        {
            string timeStr = TimeUtil.GetFormatedTime("yyyy-MM-dd H:mm", ((StealWineRecord)data[0]).exchange_time);

            transform.Find("Time").GetComponent<UILabel>().text = timeStr;
            transform.Find("CoinsGet").GetComponent<UILabel>().text = ((StealWineRecord)data[0]).coins_get;
            transform.Find("Payment").GetComponent<UILabel>().text = ((StealWineRecord)data[0]).payment;
            transform.Find("CoinsLeft").GetComponent<UILabel>().text = ((StealWineRecord)data[0]).coins_left;
            transform.Find("CauseOfChange").GetComponent<UILabel>().text = ((StealWineRecord)data[0]).comment;
        }
        catch (Exception e)
        {
            Debug.LogError(e.StackTrace);
        }
    }
}
