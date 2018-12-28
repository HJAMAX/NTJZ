using ExitGames.Client.Photon;
using Common;

public class DefenderResponseEvent : BaseEvent
{
    public override void OnEvent(EventData eventData)
    {
        //int roomId = (int)MapTool.GetValue(eventData.Parameters, (byte)ParameterCode.RoomId);
        GameData.playerCount = (int)MapTool.GetValue(eventData.Parameters, (byte)ParameterCode.RoomAction);
        GameData.nextBattleStartTime = (long)MapTool.GetValue(eventData.Parameters, (byte)ParameterCode.StartTime);
        //GetComponent<DefenderJoinRoomRequest>().SetRoomId(roomId);
        GameData.chosenRoomId = int.Parse(GameData.playerData.user_id);
        //Game.Instance.EventBus.SendEvents("open_ui", "DefenderJoin");
        Game.Instance.EventBus.SendEvents("ui_battle_ready");
    }
}
