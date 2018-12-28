using UnityEngine;

public class UILocalListManager : MonoBehaviour
{

    private UILocalListElement lastElement = null;

    private UILocalListElement curElement = null;

    void OnEnable()
    {
        if(curElement != null)
        {
            curElement.Unselect();
        }
    }

    public virtual void OnSelect(UILocalListElement element)
    {
        if(curElement != null)
        {
            curElement.Unselect();
        }
        
        lastElement = curElement;
        curElement = element;
    }
}
