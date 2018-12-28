using UnityEngine;

public class UITabManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] tabs;

    public void Tab(string name)
    {
        foreach(GameObject tab in tabs)
        {
            if(tab.name == name)
            {
                tab.SetActive(true);
            }
            else
            {
                tab.SetActive(false);
            }
        }
    }
}
