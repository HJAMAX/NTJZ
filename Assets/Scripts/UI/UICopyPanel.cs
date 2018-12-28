using UnityEngine;

public class UICopyPanel : MonoBehaviour
{
    /// <summary>
    /// 从此复制
    /// </summary>
    [SerializeField]
    private UIPanel targetPanel;

    private UIPanel prgsPanel;

    void Awake()
    {
        prgsPanel = GetComponent<UIPanel>();
    }

    void Update()
    {
        if(prgsPanel.baseClipRegion != targetPanel.baseClipRegion)
        {
            prgsPanel.baseClipRegion = targetPanel.baseClipRegion;
        }
    }
}
