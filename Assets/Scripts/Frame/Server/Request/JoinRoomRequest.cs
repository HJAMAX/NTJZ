using ExitGames.Client.Photon;
using Common;
using UnityEngine;

public class JoinRoomRequest : Request
{
    public override void DefaultRequest()
    {

    }

    public override void HandleEvent(params object[] eventData)
    {
        if (eventData.Length > 0 && eventData[0].GetType() == typeof(string))
        {
            if (GameData.playerStateData == null)
            {
                GameData.playerStateData = new PlayerStateData();
            }
            
            GameData.playerStateData.layer = Random.Range(0, GameData.maxBattleInitCount);
            GameData.playerStateData.isAlive = true;
            GameData.playerStateData.nicheng = GameData.playerData.nicheng;
            GameData.playerStateData.sex = int.Parse(GameData.playerData.sex);
            GameData.playerStateData.level = GameData.itemsData.characters;
            GameData.playerStateData.isGone = false;
            GameData.playerStateData.isDisconnect = false;
            
            //发送消息加入房间
            GameData.chosenRoomId = int.Parse(eventData[0].ToString());
            data[(byte)ParameterCode.RoomId] = GameData.chosenRoomId;
            data[(byte)ParameterCode.PlayerState] = XmlUtil.Serialize(GameData.playerStateData);
            Game.Instance.serverEngine.Peer.OpCustom((byte)opCode, data, true);

            //取消UI互动
            Game.Instance.EventBus.SendEvents("input", false);
            UIRoot.Broadcast("SetEnabled", false);
        }
    }

    public override void OnOperationResponse(OperationResponse opResponse)
    {
        if (opResponse.Parameters.ContainsKey((byte)ParameterCode.GameStart))
        {
            Game.Instance.EventBus.SendEvents("input", true);
            UIRoot.Broadcast("SetEnabled", true);
            Game.Instance.EventBus.SendEvents("msg_popup", "该酒庄已经开始偷酒了！");
        }
    }
}
