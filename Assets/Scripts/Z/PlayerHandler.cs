using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 防守方
/// </summary>
public class PlayerHandler : EventHandler
{
    public bool isAI = false;

    public string sex;

    private Animator anim;

    /// <summary>
    /// 因为射出子弹时间与敌人到来时间不一致，所以需要此列表
    /// </summary>
    private Queue<PlayerCombatAction> actionQueue = new Queue<PlayerCombatAction>();

    [SerializeField]
    private float attackTimeDelayMin = 0.5f;

    [SerializeField]
    private float attackTimeDelayMax = 1.0f;

    /// <summary>
    /// 场景中小偷人数达到临界点则开始无间隔射击
    /// </summary>
    [SerializeField]
    private int breakPoint;

    /// <summary>
    /// 武器序列
    /// </summary>
    [SerializeField]
    private WeaponController[] weapons;

    /// <summary>
    /// 正在使用的武器
    /// </summary>
    private WeaponController activeWeaponCtrl;

    void Awake()
    {
        anim = GetComponent<Animator>();
        activeWeaponCtrl = weapons[0];
        activeWeaponCtrl.gameObject.SetActive(true);
    }

    void Update()
    {
        //被动防御模式
        if(isAI && actionQueue.Count > 0 && activeWeaponCtrl.IsShootingAvailable)
        {
            PlayerCombatAction action = actionQueue.Dequeue();
            activeWeaponCtrl.Shoot(action.enemyTrans.position, action.timeDelay, (actionQueue.Count > breakPoint));
        }
    }

    /// <summary>
    /// 主动射击
    /// </summary>
    /// <param name="enemyTrans"></param>
    public void Shoot(Vector2 targetPos)
    {
        activeWeaponCtrl.Shoot(targetPos);
    }

    /// <summary>
    /// 换武器
    /// </summary>
    /// <param name="weaponIndex"></param>
    public void ChangeWeapon(int weaponIndex)
    {
        activeWeaponCtrl.gameObject.SetActive(false);
        activeWeaponCtrl = weapons[weaponIndex];
        activeWeaponCtrl.gameObject.SetActive(true);
    }

    public override void HandleEvent(params object[] data)
    {
        if(!isAI)
        {
            return;
        }

        float timeDelay = Random.Range(attackTimeDelayMin, attackTimeDelayMax);
        actionQueue.Enqueue(new PlayerCombatAction(timeDelay, (Transform)data[0]));
    }
}

public struct PlayerCombatAction
{
    public float timeDelay;

    public Transform enemyTrans;

    public PlayerCombatAction(float t, Transform e)
    {
        timeDelay = t;
        enemyTrans = e;
    }
}