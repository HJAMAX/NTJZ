using System;
using UnityEngine;

public class ExplosionEventHandler : EventHandler
{
    [SerializeField]
    private string[] explotionType;

    public override void HandleEvent(params object[] data)
    {
        if(data != null && data.Length == 2 && data[0].GetType() == typeof(int))
        {
            GameObject gameObj = Game.Instance.ObjectPool.Spawn(explotionType[(int)data[0]]);
            gameObj.transform.position = (Vector3)data[1];
        }
        else
        {
            Debug.LogError("Explosion event handler : data is not corrent");
        }
    }
}
