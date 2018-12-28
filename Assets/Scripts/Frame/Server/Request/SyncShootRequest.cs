using ExitGames.Client.Photon;
using UnityEngine;
using Common;

public class SyncShootRequest : Request
{
    public int roomId;

    public Vector2 targetPos;

    /// <summary>
    /// 削减特定弹药数量
    /// </summary>
    public int bulletIndex;

    public override void DefaultRequest()
    {
        data[(byte)ParameterCode.RoomId] = roomId;
        data[(byte)ParameterCode.Position] = XmlUtil.Serialize(new Vector3Data { x = targetPos.x, y = targetPos.y, z = 0.0f });
        Game.Instance.serverEngine.Peer.OpCustom((byte)opCode, data, true);
    }

    public override void OnOperationResponse(OperationResponse opResponse)
    {
    }
}
