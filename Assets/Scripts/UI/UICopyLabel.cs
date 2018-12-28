using UnityEngine;

public class UICopyLabel : MonoBehaviour
{
    /// <summary>
    /// 从此label复制
    /// </summary>
    [SerializeField]
    private UILabel targetlabel;

    private UILabel label;

    void Awake()
    {
        label = GetComponent<UILabel>();
    }

    void Update()
    {
        if (label.text != targetlabel.text)
        {
            label.text = targetlabel.text;
        }
    }
}
