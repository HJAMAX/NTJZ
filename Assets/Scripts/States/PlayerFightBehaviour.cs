using UnityEngine;

public class PlayerFightBehaviour : StateMachineBehaviour
{
    /// <summary>
    /// 动画到达这个时间才会发送子弹
    /// </summary>
    [SerializeField]
    private float shootTime;

    /// <summary>
    /// 子弹资源名称，对应不同等级
    /// </summary>
    [SerializeField]
    private string bulletName;

    private float currentTime;

    /// <summary>
    /// 从资源池里读取的子弹会赋予它值
    /// </summary>
    private Bullet bullet;

    private bool isShooted;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameObject bulletObj = Game.Instance.ObjectPool.Spawn(bulletName);
        //把子弹的位置设置到枪口
        Transform aimTip = animator.transform.Find("AimTip");
        bulletObj.transform.position = aimTip.position;
        bullet = bulletObj.GetComponent<Bullet>();

        currentTime = animator.recorderStartTime;
        isShooted = false;
        //开始播放动画
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        currentTime += Time.deltaTime;
        if (currentTime - animator.recorderStartTime > shootTime && !isShooted)
        {
            bullet.SetTarget(animator.GetComponent<WeaponController>().CurPos);
            bullet.GetComponent<SpriteRenderer>().enabled = true;             
            bullet.Fire();
            
            isShooted = true;
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetComponent<WeaponController>().IsShootingAvailable = true;
    }
}
