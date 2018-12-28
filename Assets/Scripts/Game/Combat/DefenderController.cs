using UnityEngine;

/// <summary>
/// 防守方
/// </summary>
public class DefenderController : MonoBehaviour
{
    public string sex;

    public bool isAI = false;

    /// <summary>
    /// 人为操作
    /// </summary>
    public bool isHuman = false; 

    private Animator anim;

    /// <summary>
    /// 武器序列
    /// </summary>
    [SerializeField]
    private WeaponController[] weapons;

    /// <summary>
    /// 正在使用的武器
    /// </summary>
    private WeaponController activeWeaponCtrl;

    /// <summary>
    /// 武器冷却组件
    /// </summary>
    [SerializeField]
    private SkillCD skillCD;

    private EnemyController[] enemyList;

    /// <summary>
    /// 同步射击
    /// </summary>
    private SyncShootRequest syncShootRequest;

    /// <summary>
    /// 防御者昵称
    /// </summary>
    [SerializeField]
    private UILabel nicknameHud;

    /// <summary>
    /// 当前武器序号
    /// </summary>
    private int curWeaponIndex;

    /// <summary>
    /// 是否没有弹药
    /// </summary>
    private bool isOutOfAmmo;

    void Awake()
    {
        activeWeaponCtrl = weapons[0];
        activeWeaponCtrl.gameObject.SetActive(true);
        anim = GetComponent<Animator>();
        syncShootRequest = GetComponent<SyncShootRequest>();
    }

    void Update()
    {
#if UNITY_EDITOR
        if (isHuman && !skillCD.isCoolingDown && !isOutOfAmmo && Input.GetMouseButtonDown(0))
#elif UNITY_ANDROID || UNITY_IOS
        if (isHuman && !skillCD.isCoolingDown && !isOutOfAmmo &&
            Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Ended)
#endif
        {
            Vector3 targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //Vector3 targetPos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            RaycastHit2D hit;
            hit = Physics2D.Raycast(targetPos, Vector2.zero);

            if (hit.collider != null)
            {
                syncShootRequest.targetPos = targetPos;
                syncShootRequest.DefaultRequest();
                //Shoot(targetPos);
            }
        }
    }

    public void Init(EnemyController[] list, bool ai, bool human, int roomId, string nicheng)
    {
        //显示标签
        nicknameHud.text = GameData.isDefender ? GameData.playerData.nicheng : nicheng;

        enemyList = list;
        isAI = ai;
        isHuman = human;
        syncShootRequest.roomId = roomId;

        if(isHuman)
        {
            Game.Instance.EventBus.SendEvents("open_ui", "WeaponOptions");
        }
    }

    public void StartBattle()
    {
        if (!isHuman && isAI)
        {
            //若本机是Host，则AI从本机发送RPC
            InvokeRepeating("AutoShoot", 1.0f, activeWeaponCtrl.CooldownTime);
        }
    }

    /// <summary>
    /// 主动射击
    /// </summary>
    /// <param name="enemyTrans"></param>
    public void Shoot(Vector2 targetPos)
    {
        activeWeaponCtrl.Shoot(targetPos);
        if (isHuman)
        {
            Game.Instance.EventBus.SendEvents("skill_cd", curWeaponIndex);
        }
    }

    /// <summary>
    /// 换武器
    /// </summary>
    /// <param name="weaponIndex"></param>
    public void ChangeWeapon(int weaponIndex)
    {
        if(weaponIndex < 0)
        {
            CancelInvoke();
            return;
        }

        curWeaponIndex = weaponIndex;
        activeWeaponCtrl.gameObject.SetActive(false);
        activeWeaponCtrl = weapons[curWeaponIndex];
        activeWeaponCtrl.gameObject.SetActive(true);

        if (!isHuman && isAI)
        {
            CancelInvoke();
            InvokeRepeating("AutoShoot", activeWeaponCtrl.CooldownTime, activeWeaponCtrl.CooldownTime);
        }
    }

    /// <summary>
    /// 自动射击
    /// </summary>
    private void AutoShoot()
    {
        float dist = float.MaxValue;
        Vector2 nearPos = Vector2.zero;

        foreach (EnemyController enemy in enemyList)
        {
            //敌人已死或退出
            if(!enemy.gameObject.activeInHierarchy)
            {
                continue;
            }
            //取最近的射击
            float tempDist = Vector2.Distance(enemy.transform.localPosition, transform.localPosition);
            if (tempDist < dist)
            {
                dist = tempDist;
                nearPos = enemy.transform.localPosition;
            }
        }

        if(nearPos.magnitude > 0.0f && !isOutOfAmmo)
        {
            syncShootRequest.targetPos = nearPos;
            syncShootRequest.DefaultRequest();
            //Shoot(nearPos);
        }
        else
        {
            //酒庄主胜利或者没有弹药
            CancelInvoke();
        }
    }
}
/*
public struct PlayerCombatAction
{
    public float timeDelay;

    public Transform enemyTrans;

    public PlayerCombatAction(float t, Transform e)
    {
        timeDelay = t;
        enemyTrans = e;
    }
}*/