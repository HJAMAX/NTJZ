using ExitGames.Client.Photon;
using UnityEngine;
using Common;

public class SwitchWeaponRequest : Request
{
    public int roomId;

    /// <summary>
    /// 要发送的武器序号
    /// </summary>
    public int weaponIndex;

    public override void DefaultRequest()
    {
        data[(byte)ParameterCode.RoomId] = roomId;
        data[(byte)ParameterCode.WeaponIndex] = weaponIndex;
        Game.Instance.serverEngine.Peer.OpCustom((byte)opCode, data, true);
    }

    public override void OnOperationResponse(OperationResponse opResponse)
    {
        
    }
}
