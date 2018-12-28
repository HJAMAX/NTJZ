using ExitGames.Client.Photon;
using Common;

public class LeaveRoomEvent : BaseEvent
{
    public override void OnEvent(EventData eventData)
    {
        GameData.playerCount = (int)MapTool.GetValue(eventData.Parameters, (byte)ParameterCode.RoomAction);
        int playerId = (int)MapTool.GetValue(eventData.Parameters, (byte)ParameterCode.PlayerId);

        if(playerId != int.Parse(GameData.playerData.user_id))
        {
            Game.Instance.EventBus.SendEvents("ui_join_ready");
            Game.Instance.EventBus.SendEvents("ui_battle_ready");
        }
    }
}
