using UnityEngine;

public class UpdatePlayerInfoEventHandler : EventHandler
{
    public override void HandleEvent(params object[] data)
    {
        if (data != null && data.Length > 0 && data[0].GetType() == typeof(string))
        {
            JsonWrapperArray<string> wrapper = JsonWrapperArray<string>.FromJson<string>((string)data[0]);

            if (wrapper.items != null && wrapper.items[0] != "")
            {
                GameData.playerData = JsonUtility.FromJson<PlayerData>(wrapper.items[0]);
                GameData.itemsData = JsonUtility.FromJson<ItemsData>(wrapper.items[1]);

                Game.Instance.EventBus.SendEvents("bullets_info");
                Game.Instance.EventBus.SendEvents("mask_info");
                Game.Instance.EventBus.SendEvents("weapon_info");
                Game.Instance.EventBus.SendEvents("gold_coins_info", GameData.playerData.goldCoins.ToString());
                Game.Instance.EventBus.SendEvents("gold_coins_up_limit");
            }

            Game.Instance.EventBus.SendEvents("msg_popup", wrapper.msg);
        }
    }
}
