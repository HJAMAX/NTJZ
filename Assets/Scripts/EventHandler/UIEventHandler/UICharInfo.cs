using UnityEngine;

public class UICharInfo : EventHandler
{
    [SerializeField]
    private string[] amountList;

    [SerializeField]
    private string[] charInfoList;

    /// <summary>
    /// 价格UI
    /// </summary>
    private UILabel amountLabel;

    /// <summary>
    /// 角色信息UI
    /// </summary>
    private UILabel charInfoLabel;

    /// <summary>
    /// 当前被选中的角色序号
    /// </summary>
    public string curCharIndex;

    /// <summary>
    /// 当前角色价格
    /// </summary>
    public string curCharPrice;

    void Start()
    {
        amountLabel = transform.Find("Amount").GetComponent<UILabel>();
        charInfoLabel = transform.Find("CharInfo").GetComponent<UILabel>();

        Game.Instance.EventBus.RegisterController(GetComponent<EventController>());
    }

    public override void HandleEvent(params object[] data)
    {
        if(data.Length > 0)
        {
            int index = (int)data[0];
            curCharIndex = index.ToString();
            curCharPrice = amountList[index];

            amountLabel.text = amountList[index];
            charInfoLabel.text = charInfoList[index];
        }
    }
}
