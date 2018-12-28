using UnityEngine;

public class UISkillCD : MonoBehaviour
{
    public bool isCoolingDown = false;

    /// <summary>
    /// 攻击后的冷却时间
    /// </summary>
    public float attackCDT;

    /// <summary>
    /// 当前的冷却时间
    /// </summary>
    private float curCDT;

    private UISprite uiSkillCDSprite;

    private UILabel uibulletAmount;

    private int bulletAmount;

    public int BulletAmount { get { return bulletAmount; } }

    void Awake()
    {
        uiSkillCDSprite = GetComponent<UISprite>();
        uibulletAmount = transform.Find("Label").GetComponent<UILabel>();
    }

    void Update()
    {
        if(isCoolingDown)
        {
            uiSkillCDSprite.fillAmount -= (1.0f / curCDT) * Time.deltaTime;
            if(uiSkillCDSprite.fillAmount <= 0.01f)
            {
                uiSkillCDSprite.fillAmount = 0;
                isCoolingDown = false;
                Game.Instance.EventBus.SendEvents("skill_cd", isCoolingDown);
            }
        }
    }

    public void InitBulletUI(int bullet)
    {
        uibulletAmount.text = bullet.ToString();
        bulletAmount = bullet;
    }

    public void SwitchWeaponCD(float switchWeaponCDT)
    {
        curCDT = switchWeaponCDT;
        uiSkillCDSprite.fillAmount = 1.0f;
        isCoolingDown = true;
    }

    public void AttackCD()
    {
        curCDT = attackCDT;
        uiSkillCDSprite.fillAmount = 1.0f;
        isCoolingDown = true;

        bulletAmount--;
        uibulletAmount.text = bulletAmount.ToString();
    }
}
