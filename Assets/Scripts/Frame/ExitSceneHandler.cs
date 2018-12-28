using UnityEngine;

public class ExitSceneHandler : EventHandler
{
    public override void HandleEvent(params object[] data)
    {
        Game.Instance.ObjectPool.ClearAll();
        Game.Instance.ObjectPool.UnspawnAll();
    }
}
