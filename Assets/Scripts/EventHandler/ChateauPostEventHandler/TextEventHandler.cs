
public class TextEventHandler : EventHandler
{
    public override void HandleEvent(params object[] data)
    {
        if(data != null && data.Length > 0 && data[0].GetType() == typeof(string))
        {
            transform.Find("Label").GetComponent<UILabel>().text = (string)data[0];
        }
    }

    public void SetText(string text)
    {
        transform.Find("Label").GetComponent<UILabel>().text = text;
    }
}
