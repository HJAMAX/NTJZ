using ExitGames.Client.Photon;
using Common;
using UnityEngine;

public class SyncPosRequest : Request
{
    [HideInInspector]
    public CombatPlayerData combatPlayerData;

    public override void DefaultRequest()
    {
        data[(byte)ParameterCode.Position] = XmlUtil.Serialize(combatPlayerData);
        Game.Instance.serverEngine.Peer.OpCustom((byte)opCode, data, true);
    }

    public override void OnOperationResponse(OperationResponse opResponse)
    {

    }
}
