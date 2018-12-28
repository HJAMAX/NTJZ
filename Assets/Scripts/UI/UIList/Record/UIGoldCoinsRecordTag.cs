using UnityEngine;
using System;

public class UIGoldCoinsRecordTag : UIListElement
{
    public override void SetData(params object[] data)
    {
        try
        {
            GoldDetailRecord record = (GoldDetailRecord)data[0];
            string timeStr = TimeUtil.GetFormatedTime("yyyy-MM-dd H:mm", record.exchange_time);

            transform.Find("ExchangeTime").GetComponent<UILabel>().text = timeStr;
            transform.Find("CauseOfChange").GetComponent<UILabel>().text = record.cause_of_change;
            transform.Find("AddNum").GetComponent<UILabel>().text = record.add_num;
            transform.Find("Payment").GetComponent<UILabel>().text = record.reduce_num;
            transform.Find("RemainingSum").GetComponent<UILabel>().text = record.remaining_sum;
        }
        catch (Exception e)
        {
            Debug.LogError(e.StackTrace);
        }
    }
}
