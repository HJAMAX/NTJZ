using UnityEngine;

public class UILocalListElement : MonoBehaviour
{
    protected UILocalListManager listManager;

    protected PostEvent postEvent;

    /// <summary>
    /// 
    /// </summary>
    protected GameObject selected;

    void Awake()
    {
        listManager = transform.parent.GetComponent<UILocalListManager>();
        postEvent = transform.parent.GetComponent<PostEvent>();
        selected = transform.Find("Selected").gameObject;
    }

    public virtual void Select()
    {
        selected.SetActive(true);
        listManager.OnSelect(this);
    }

    public virtual void Unselect()
    {
        selected.SetActive(false);
    }
}
