using ExitGames.Client.Photon;
using UnityEngine;
using Common;

public class MoveHostEvent : BaseEvent
{
    public override void OnEvent(EventData eventData)
    {
        string defenderDataStr = (string)MapTool.GetValue(eventData.Parameters, (byte)ParameterCode.MoveHost);
        DefenderData defenderData = XmlUtil.Deserialize<DefenderData>(defenderDataStr);

        DefenderController activeDefender = GetComponent<BattleManager>().ActiveDefender;
        activeDefender.isAI = true;
        activeDefender.StartBattle();
    }
}
