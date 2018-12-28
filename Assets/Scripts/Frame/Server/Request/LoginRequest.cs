using ExitGames.Client.Photon;
using Common;
using UnityEngine;

public class LoginRequest : Request
{
    [SerializeField]
    private bool isTest;

    [SerializeField]
    private UILabel testIdLabel;

    public override void DefaultRequest()
    {
        int userId = 0;
        if (isTest)
        {
            GameData.playerData = new PlayerData();
            userId = int.Parse(testIdLabel.text);
            GameData.playerData.user_id = testIdLabel.text;
        }
        else
        {
            userId = int.Parse(GameData.playerData.user_id);
        }

        data[(byte)ParameterCode.UserId] = userId;
        Game.Instance.serverEngine.Peer.OpCustom((byte)opCode, data, true);
    }

    public override void OnOperationResponse(OperationResponse opResponse)
    {
        Debug.LogError(opResponse.Parameters[(byte)ParameterCode.UserId]);
    }
}
