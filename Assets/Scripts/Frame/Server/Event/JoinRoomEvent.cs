using ExitGames.Client.Photon;
using Common;
using System.Runtime.InteropServices;

public class JoinRoomEvent : BaseEvent
{
#if UNITY_IOS
    [DllImport("__Internal")]
    private static extern void __IOSLogErrorMessage(string message);
#endif
    public override void OnEvent(EventData eventData)
    {
        GameData.playerCount = (int)MapTool.GetValue(eventData.Parameters, (byte)ParameterCode.RoomAction);
        GameData.nextBattleStartTime = (long)MapTool.GetValue(eventData.Parameters, (byte)ParameterCode.StartTime);
        int hostId = (int)MapTool.GetValue(eventData.Parameters, (byte)ParameterCode.HostId);
        Game.Instance.EventBus.SendEvents("ui_join_ready");

        //初始化
        if (GameData.isDefender)
        {
            GameData.isDefender = false;
            Game.Instance.EventBus.SendEvents("toast");
        }
        //同步玩家数据
        string stateDataStr = (string)MapTool.GetValue(eventData.Parameters, (byte)ParameterCode.PlayerState);

        if (stateDataStr != null)
        {
            PlayerStateData playerStateData = XmlUtil.Deserialize<PlayerStateData>(stateDataStr);
            GameData.playerStateData = playerStateData;
#if UNITY_IOS
            __IOSLogErrorMessage(playerStateData.roomId.ToString());
#endif
        }
    }
}
