
public class UIMsgPopup : EventHandler
{
    private UILabel msgLabel;

    public override void HandleEvent(params object[] data)
    {
        if(msgLabel == null)
        {
            msgLabel = transform.Find("MsgLabel").GetComponent<UILabel>();
        }

        gameObject.SetActive(true);
        msgLabel.text = (string)data[0];
        UIRoot.Broadcast("SetEnabled", false);
    }

    public void OnClickClose()
    {
        UIRoot.Broadcast("SetEnabled", true);
        gameObject.SetActive(false);
    }
}
