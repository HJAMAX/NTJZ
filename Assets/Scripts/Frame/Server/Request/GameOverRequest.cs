using ExitGames.Client.Photon;
using Common;

public class GameOverRequest : Request
{
    private bool isOver;

    void OnEnable()
    {
        isOver = false;
    }

    /// <summary>
    /// 主动离开
    /// </summary>
    public void Leave()
    {
        GameData.playerStateData.isGone = true;
    }

    public override void DefaultRequest()
    {
        if (isOver)
        {
            return;
        }

        data[(byte)ParameterCode.GameOver] = XmlUtil.Serialize(GameData.playerStateData);
        Game.Instance.serverEngine.Peer.OpCustom((byte)opCode, data, true);
        isOver = true;
    }

    public override void OnOperationResponse(OperationResponse opResponse)
    {
        data = opResponse.Parameters;
        object action = null;
        data.TryGetValue((byte)ParameterCode.RoomAction, out action);

        if ((string)action == "Leave")
        {
            Game.Instance.LoadingScene(GameData.sceneIndex);
        }
    }
}
