using UnityEngine;

public class TestDefence : MonoBehaviour
{
    public void Test()
    {
        //测试主动防御
        //Game.Instance.socket.isFoe = false;
        //GameData.chosenPlayerMaskLevel = int.Parse(GameData.playerData.maskId);
        Game.Instance.LoadingScene(GameData.battleIndex);
        //Game.Instance.EventBus.SendEvents("open_ui", "SpouseConfirm");
        return;
    }
}
