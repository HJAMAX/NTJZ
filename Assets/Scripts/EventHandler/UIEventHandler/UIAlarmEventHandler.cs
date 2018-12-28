
public class UIAlarmEventHandler : EventHandler
{
    public override void HandleEvent(params object[] data)
    {
        if (data.Length > 0 && data[0].GetType() == typeof(bool))
        {
            gameObject.SetActive((bool)data[0]);
        }
    }
}
