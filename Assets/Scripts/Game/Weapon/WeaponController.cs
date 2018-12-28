using UnityEngine;
using System.Collections.Generic;

public class WeaponController : MonoBehaviour
{
    [SerializeField]
    private string bulletName;

    /// <summary>
    /// 武器冷却时间
    /// </summary>
    [SerializeField]
    private float cooldownTime;

    public float CooldownTime { get { return cooldownTime; } }

    /// <summary>
    /// 是否在播放攻击的动画
    /// </summary>
    private bool isShootingAvailable = true;

    public bool IsShootingAvailable {
        get { return isShootingAvailable; }
        set { isShootingAvailable = value; }
    }

    /// <summary>
    /// 当前瞄准的位置
    /// </summary>
    private Vector2 curPos;

    public Vector2 CurPos { get { return curPos; } }

    private Animator anim;

    void OnEnable()
    {
        anim = GetComponent<Animator>();
    }
    
    public void Shoot(Vector2 targetPos, float timeDelay, bool shootAtOnce)
    {
        if (isShootingAvailable)
        {
            isShootingAvailable = false;
            curPos = targetPos;
            IEnumerator<WaitForSeconds> coroutine = shootAtOnce ? Shoot(0.0f) : Shoot(timeDelay);
            StartCoroutine(coroutine);
        }
    }

    public void Shoot(Vector2 targetPos)
    {
        curPos = targetPos;

        if (anim)
            anim.Play("shoot");
        else
        {
            GameObject bulletObj = Game.Instance.ObjectPool.Spawn(bulletName);
            //把子弹的位置设置到枪口
            Transform aimTip = transform.Find("AimTip");
            bulletObj.transform.position = aimTip.position;
            Bullet bullet = bulletObj.GetComponent<Bullet>();

            bullet.SetTarget(curPos);
            bullet.GetComponent<SpriteRenderer>().enabled = true;
            bullet.Fire();
            //爆炸效果
            Game.Instance.EventBus.SendEvents("explosion", 0, bullet.transform.position);
        }
    }

    IEnumerator<WaitForSeconds> Shoot(float timeDelay)
    {
        yield return new WaitForSeconds(timeDelay);
        anim.Play("shoot");
    }
}
