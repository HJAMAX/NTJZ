using UnityEngine;

/// <summary>
/// 当玩家点击酿酒器时，弹出窗口询问是否偷酒
/// </summary>
public class UICombatPopup : EventHandler
{
    /// <summary>
    /// 可偷酒时弹出此窗口
    /// </summary>
    [SerializeField]
    private GameObject confirmObj;

    /// <summary>
    /// 不可偷酒时弹出此窗口
    /// </summary>
    [SerializeField]
    private GameObject alertObj;

    public override void HandleEvent(params object[] data)
    {
        if(data.Length > 0 && data[0].GetType() == typeof(GameObject))
        {
            GameObject gameObj = (GameObject)data[0];
            if (gameObj.transform.Find("Text").GetComponent<UITimeCountDown>().isAvailable())
            {
                confirmObj.SetActive(true);
            }
            else
            {
                alertObj.SetActive(true);
            }
        }
    }

    public void Click(GameObject gameObj)
    {
        gameObj.SetActive(false);
    }
}
