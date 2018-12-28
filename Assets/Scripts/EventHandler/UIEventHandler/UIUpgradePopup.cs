using UnityEngine;

public class UIUpgradePopup : EventHandler
{
    [SerializeField]
    private bool isActive;

    [SerializeField]
    private string nextEventName;

    [SerializeField]
    private string fieldName;

    [SerializeField]
    private UILabel msgLabel;

    public override void HandleEvent(params object[] data)
    {
        gameObject.SetActive(isActive);
        JsonWrapperArray<string> wrapper = JsonWrapperArray<string>.FromJson<string>((string)data[0]);

        if (wrapper.state == "1")
        {
            GameData.upgradeData.application_rank = int.Parse(wrapper.items[0]);
            Game.Instance.EventBus.SendEvents(nextEventName);
        }

        msgLabel.text = wrapper.msg;
    }
}