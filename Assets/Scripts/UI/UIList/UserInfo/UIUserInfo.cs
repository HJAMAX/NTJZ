using UnityEngine;

/// <summary>
/// 更新用户信息
/// </summary>
public class UIUserInfoEventHandler : EventHandler
{
    [SerializeField]
    private Transform[] userInfoUIList;

    public override void HandleEvent(params object[] data)
    {
        foreach(Transform userInfoUI in userInfoUIList)
        {
            userInfoUI.Find("Label").GetComponent<UILabel>().text = GameData.GetField(userInfoUI.name).ToString();
        }
    }
}
