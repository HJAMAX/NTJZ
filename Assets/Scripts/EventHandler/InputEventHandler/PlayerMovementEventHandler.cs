using UnityEngine;

public class PlayerMovementEventHandler : EventHandler
{
    [SerializeField]
    private float oriSpeed;

    [SerializeField]
    private JoyStickController joyStick;

    private EnemyController target;

    public override void HandleEvent(params object[] data)
    {
        target = (EnemyController)data[0];
        oriSpeed = target.networkMoveSpeed;
    }

    void Update()
    {
        target.localMoveSpeed = joyStick.moveDirect * oriSpeed;     
    }
}

