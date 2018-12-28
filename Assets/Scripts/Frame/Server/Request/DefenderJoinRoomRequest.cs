using ExitGames.Client.Photon;
using Common;
using UnityEngine;

public class DefenderJoinRoomRequest : Request
{
    public void SetRoomId(int rid)
    {
        if(rid > 0)
        {
            GameData.chosenRoomId = rid;
        }
    }

    public void Join()
    {
        if (GameData.playerStateData == null)
        {
            GameData.playerStateData = new PlayerStateData();
            GameData.playerStateData.roomId = int.Parse(GameData.playerData.user_id);
            GameData.playerStateData.id = GameData.playerStateData.roomId;
            GameData.playerStateData.nicheng = GameData.playerData.nicheng;
            GameData.playerStateData.sex = int.Parse(GameData.playerData.sex);
            GameData.playerStateData.level = GameData.itemsData.characters;
            GameData.playerStateData.isGone = false;
            GameData.playerStateData.isDisconnect = false;
        }

        GameData.isDefender = true;
        DefaultRequest();
    }

    public void Leave()
    {
        GameData.isDefender = false;
        DefaultRequest();
    }

    public override void DefaultRequest()
    {
        data[(byte)ParameterCode.Defender] = GameData.isDefender;
        Game.Instance.serverEngine.Peer.OpCustom((byte)opCode, data, true);
    }

    public override void OnOperationResponse(OperationResponse opResponse)
    {
        Debug.LogError("找不到房间");
    }
}
