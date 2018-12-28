using UnityEngine;

public class UICoverEventHandler : EventHandler
{
    /// <summary>
    /// 对应的在GameData里的变量名
    /// </summary>
    [SerializeField]
    private string fieldName;

    /// <summary>
    /// level若小于等于变量则拿掉遮罩
    /// </summary>
    public override void HandleEvent(params object[] data)
    {
        int playerLevel = int.Parse(GameData.GetField(fieldName).ToString());
        for(int i = 0; i < transform.childCount; i++)
        {
            Transform childObj = transform.GetChild(i);
            //等级即gameObject的name
            if(playerLevel % 2 == 1)
            {
                childObj.Find("Cover").gameObject.SetActive(false);
            }

            playerLevel = playerLevel >> 1;
        }
    }
}
