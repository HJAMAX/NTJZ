using UnityEngine;

public class UISelectCoverEventHandler : EventHandler
{
    /// <summary>
    /// 对应的在GameData里的变量名
    /// </summary>
    [SerializeField]
    private string fieldName;

    [SerializeField]
    private GameObject[] purchaseButton;

    /// <summary>
    /// level若小于等于变量则拿掉遮罩
    /// </summary>
    public override void HandleEvent(params object[] data)
    {
        int playerLevel = int.Parse(GameData.GetField(fieldName).ToString());
        purchaseButton[int.Parse(GameData.playerData.sex)].gameObject.SetActive(true);

        for (int i = 0; i < transform.childCount; i++)
        {
            Transform childObj = transform.GetChild(i);
            //等级即gameObject的name
            childObj.Find("Cover" + GameData.playerData.sex).gameObject.SetActive(playerLevel - 1 != i);
            //childObj.Find(GameData.playerData.sex).gameObject.SetActive(true);
        }
    }
}
