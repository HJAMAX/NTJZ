using System.Collections.Generic;
using ExitGames.Client.Photon;
using Common;

public class GameOverEvent : BaseEvent
{
    public override void OnEvent(EventData eventData)
    {
        string stateDataStr = (string)MapTool.GetValue(eventData.Parameters, (byte)ParameterCode.GameOver);
        PlayerStateData playerStateData = XmlUtil.Deserialize<PlayerStateData>(stateDataStr);

        //主动或被动离开房间的玩家都会被删除
        EnemyController enemy = null;
        BattleManager battleManager = GetComponent<BattleManager>();
        battleManager.EnemyMap.TryGetValue(playerStateData.id, out enemy);

        if (enemy != null)
        {
            Game.Instance.ObjectPool.Unspawn(enemy.gameObject);
            GetComponent<BattleManager>().EnemyMap.Remove(playerStateData.id);
            Game.Instance.EventBus.SendEvents("open_ui", "GameOverPopup", false);
        }

        if (playerStateData.id == GameData.playerStateData.id)
        {
            if (playerStateData.isGone)
            {
                Game.Instance.LoadingScene(GameData.myChateauIndex);
            }
            else if (playerStateData.isAttackSucceed)
            {
                JsonWrapperArray<string> wrapper = JsonWrapperArray<string>.FromJson<string>(playerStateData.responseContext);
                Dictionary<string, string> dataMap = JsonWrapperArray<string>.FromJsonMap(wrapper.items[0]);
                Game.Instance.EventBus.SendEvents("battle_end", "attack_succeeded", dataMap["wine_get"], dataMap["wine_coins"]);
            }
            else
            {
                Game.Instance.EventBus.SendEvents("battle_end", "attack_failed");
            }
        }
        else if (battleManager.ActiveDefender.isHuman)
        {
            if (playerStateData.isAttackSucceed)
            {
                Game.Instance.EventBus.SendEvents("battle_end", "defend_failed");
            }
            else if(battleManager.EnemyMap.Count == 0)
            {
                Game.Instance.EventBus.SendEvents("battle_end", "defend_succeeded");
            }
        }
        else if (playerStateData.isAttackSucceed)
        {
            Game.Instance.EventBus.SendEvents("battle_end", "attack_failed");
        }
    }
}
