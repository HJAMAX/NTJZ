using ExitGames.Client.Photon;
using Common;
using System.Collections.Generic;

public class StartGameEvent : BaseEvent
{
    /// <summary>
    /// 初始化所有玩家
    /// </summary>
    /// <param name="eventData"></param>
    public override void OnEvent(EventData eventData)
    {
        GameData.nextBattleStartTime = 0;

        string defenderDataStr = (string)MapTool.GetValue(eventData.Parameters, (byte)ParameterCode.Defender);
        GameData.defenderData = XmlUtil.Deserialize<DefenderData>(defenderDataStr);
        string playerStateDataStr = (string)MapTool.GetValue(eventData.Parameters, (byte)ParameterCode.RoomAction);
        GameData.playerStateDataList = XmlUtil.Deserialize<List<PlayerStateData>> (playerStateDataStr);

        Game.Instance.LoadingScene(GameData.battleIndex);
    }
}
