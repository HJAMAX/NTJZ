
/// <summary>
/// 用于显示断线重连的信息
/// </summary>
public class UIRonnMsg : EventHandler
{
    /// <summary>
    /// 用于显示重连次数
    /// </summary>
    private UILabel countLabel;

    void Awake()
    {
        countLabel = transform.Find("Count").GetComponent<UILabel>();
    }

    /// <summary>
    /// 弹出重连消息框并更新重连次数
    /// </summary>
    /// <param name="data"></param>
    public override void HandleEvent(params object[] data)
    {
        if(data.Length > 0 && data[0].GetType() == typeof(bool))
        {
            gameObject.SetActive((bool)data[0]);
        }
        if(data.Length > 1)
        {
            countLabel.text = data[1].ToString();
        }
    }
}
