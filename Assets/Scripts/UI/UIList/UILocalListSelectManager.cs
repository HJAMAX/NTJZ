using UnityEngine;

public class UILocalListSelectManager : UILocalListManager
{
    /// <summary>
    /// 
    /// </summary>
    [SerializeField]
    private GameObject backgroundPanel;

    /// <summary>
    /// 
    /// </summary>
    [SerializeField]
    private UILabel label;

    public override void OnSelect(UILocalListElement element)
    {
        base.OnSelect(element);
        label.text = element.name;

        if (backgroundPanel != null)
        {
            backgroundPanel.SetActive(false);
        }
    }
}
