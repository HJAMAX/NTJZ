using UnityEngine;

/// <summary>
/// 技能冷却
/// </summary>
public class SkillCD : EventHandler
{
    public bool isCoolingDown = false;

    private UISprite skillCDCover;

    [SerializeField]
    private UISkillCD[] skillCDList;

    [SerializeField]
    private float switchWeaponCDT;

    private GameObject lastTag;
    
    void Start()
    {
        skillCDList[0].InitBulletUI(GameData.itemsData.arrow);
        skillCDList[1].InitBulletUI(GameData.itemsData.bullet);
        skillCDList[2].InitBulletUI(GameData.itemsData.cannonball);
        skillCDList[3].InitBulletUI(GameData.itemsData.rocket);
        skillCDList[4].InitBulletUI(GameData.itemsData.missile);
    }

    private void LightUp(GameObject chosenTag)
    {
        //技能冷却中不能换武器
        if (isCoolingDown)
        {
            return;
        }

        if (lastTag != null)
        {
            lastTag.SetActive(false);
        }

        chosenTag.SetActive(true);
        lastTag = chosenTag;
    }

    /// <summary>
    /// 判断能否换武器
    /// </summary>
    /// <returns></returns>
    public bool SwitchWeaponCD(int weaponIndex)
    {
        //技能冷却中不能换武器
        if(isCoolingDown || skillCDList[weaponIndex].BulletAmount <= 0)
        {
            return false;
        }

        GameObject chosenTag = skillCDList[weaponIndex].transform.Find("Pressed").gameObject;
        LightUp(chosenTag);

        foreach(UISkillCD uiSkillCD in skillCDList)
        {
            uiSkillCD.SwitchWeaponCD(switchWeaponCDT);
            isCoolingDown = true;
        }
      
        return true;
    }

    public override void HandleEvent(params object[] data)
    {
        if (data.Length > 0)
        {
            if (data[0].GetType() == typeof(int))
            {
                int weaponIndex = (int)data[0];
                skillCDList[weaponIndex].AttackCD();
                isCoolingDown = true;
            }
            else if(data[0].GetType() == typeof(bool))
            {
                isCoolingDown = (bool)data[0];
            }
        }
    }
}
