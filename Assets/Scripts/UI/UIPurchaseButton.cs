using UnityEngine;

public class UIPurchaseButton : MonoBehaviour
{
    [SerializeField]
    private UIButtonContainer uiContainer;

    [SerializeField]
    private PostEvent postEvent;

    private string dataName;

    private string data;

    /// <summary>
    /// 购买物品数量为0时，则不出现指定UI
    /// </summary>
    /// <param name="targetAmount"></param>
    /// <param name="uiName"></param>
    public void OnClickCheck(string targetAmount, string uiName)
    {
        if(int.Parse(targetAmount) > 0)
        {
            postEvent.AddPostData(dataName, data);
            uiContainer.OnClickOpen(uiName);
        }
    }

    public void AddItemData(string dn, string d)
    {
        dataName = dn;
        data = d;
    }
}
