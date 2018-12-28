using BestHTTP;
using UnityEngine;

public class MyChateauInitEventHandler : AbstractSendMsgEventHandler
{
    /// <summary>
    /// 发送请求，接收被选中酒庄的信息
    /// </summary>
    /// <param name="data"></param>
    public override void HandleEvent(params object[] data)
    {
        PostData[] postDataList = { new PostData("user_id", GameData.playerData.user_id) };
        SendEvent(postDataList, "chateau/getChateauInfo", OnRequestCallBack);

        Game.Instance.GetComponent<ServerEngine>().Connect();
    }

    protected override void OnRequestCallBack(HTTPRequest request, HTTPResponse response)
    {
        base.OnRequestCallBack(request, response);
        JsonWrapperArray<string> wrapper = JsonWrapperArray<string>.FromJson<string>(response.DataAsText);

        if (wrapper.state == "1")
        {
            GameData.playerChateauData = JsonUtility.FromJson<ChateauData>(wrapper.items[0]);
            GameData.shopData = JsonUtility.FromJson<ShopData>(wrapper.items[1]);
            GameData.itemsData = JsonUtility.FromJson<ItemsData>(wrapper.items[2]);
            GameData.playerData.wine_coins = float.Parse(wrapper.items[3]);  

            //更新UI
            Game.Instance.EventBus.SendEvents("brewbot_info");
            Game.Instance.EventBus.SendEvents("wine_maker");
            Game.Instance.EventBus.SendEvents("wine_coins_info", GameData.playerData.wine_coins.ToString());
            Game.Instance.EventBus.SendEvents("yuanbao_info", GameData.playerData.yuanbao.ToString());
            Game.Instance.EventBus.SendEvents("gold_coins_info", GameData.playerData.goldCoins.ToString());
            Game.Instance.EventBus.SendEvents("name_info", GameData.playerData.nicheng);

            Game.Instance.EventBus.SendEvents("total_wine_stored_info", GameData.playerChateauData.totalWineStored);
            float totalWineStole = float.Parse(GameData.playerChateauData.total_wine_stealed);
            Game.Instance.EventBus.SendEvents("total_wine_stealed_info", totalWineStole.ToString());
            Game.Instance.EventBus.SendEvents("total_wine_lost_info", GameData.playerChateauData.total_wine_lost);

            Game.Instance.EventBus.SendEvents("gold_coins_up_limit");

            Game.Instance.EventBus.SendEvents("ui_battle_ready");
        }
        else
        {
            Game.Instance.EventBus.SendEvents("msg_popup", wrapper.msg);
        }
    }
}