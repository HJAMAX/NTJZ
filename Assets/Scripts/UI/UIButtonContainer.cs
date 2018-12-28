using UnityEngine;

public class UIButtonContainer : EventHandler
{
    [SerializeField]
    private GameObject[] keepOpenList;

    public override void HandleEvent(params object[] data)
    {
        if (data.Length == 1 && data[0] != null && data[0].GetType() == typeof(string))
        {
            OnClickOpen((string)data[0]);
        }
        else if(data.Length == 2 && data[1] != null && data[1].GetType() == typeof(bool))
        {
            OnClickClose((string)data[0]);
        }
        else
        {
            Debug.LogError("");
        }
    }

    public void OnClickOpen(string uiName)
    {
        Transform trans = transform.Find(uiName);
        if (trans != null)
        { 
            trans.gameObject.SetActive(true);
        }
    }

    public void OnClickClose(string uiName)
    {
        Transform trans = transform.Find(uiName);
        if (trans != null)
        {
            trans.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 只打开一个界面，其他都关掉
    /// </summary>
    public void OnCLickOpenOne(string uiName)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject gameObj = transform.GetChild(i).gameObject;
            gameObj.SetActive(gameObj.name == uiName);
        }

        for (int i = 0; i < keepOpenList.Length; i++)
        {
            keepOpenList[i].SetActive(true);
        }
    }
}