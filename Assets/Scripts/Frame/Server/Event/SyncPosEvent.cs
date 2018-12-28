using System.Collections.Generic;
using ExitGames.Client.Photon;
using UnityEngine;
using Common;

public class SyncPosEvent : BaseEvent
{
    private BattleManager battleManager;

    void Awake()
    {
        battleManager = GetComponent<BattleManager>();
    }

    public override void OnEvent(EventData eventData)
    {
        string playerDataListStr = (string)MapTool.GetValue(eventData.Parameters, (byte)ParameterCode.PlayerDataList);
        List<CombatPlayerData> playerDataList = XmlUtil.Deserialize<List<CombatPlayerData>>(playerDataListStr);

        foreach (CombatPlayerData playerData in playerDataList)
        {
            if(battleManager.EnemyMap.ContainsKey(playerData.id) && 
              !battleManager.EnemyMap[playerData.id].isLocal)
            {
                Vector2 pos = new Vector2(playerData.Pos.x, playerData.Pos.y);
                battleManager.EnemyMap[playerData.id].networkPos = pos;
            }
        }
    }
}
