using ExitGames.Client.Photon;
using UnityEngine;
using Common;

public class RoomRequest : Request
{
    [SerializeField]
    private string action = "Start";
    
    public void SetAction(string a)
    {
        action = a;
    }

    public override void DefaultRequest()
    {
        GameData.nextBattleStartTime = 0;

        data[(byte)ParameterCode.RoomAction] = action;
        data[(byte)ParameterCode.RoomId] = GameData.chosenRoomId;
        Game.Instance.serverEngine.Peer.OpCustom((byte)opCode, data, true);
    }

    public override void OnOperationResponse(OperationResponse opResponse)
    {
        data = opResponse.Parameters;
        object action = null;
        data.TryGetValue((byte)ParameterCode.RoomAction, out action);

        Game.Instance.EventBus.SendEvents("input", true);
        UIRoot.Broadcast("SetEnabled", true);
    }
}