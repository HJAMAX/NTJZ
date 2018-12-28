using UnityEngine;
using BestHTTP;

/// <summary>
/// 用于从http获取列表
/// </summary>
public class UIGetListManager<T> : AbstractSendMsgEventHandler
{
    [SerializeField]
    protected string webLocation;

    [SerializeField]
    protected string spawnName;

    protected UIScrollView scrollView;

    protected PostData[] postData;

    void Awake()
    {
        scrollView = transform.parent.GetComponent<UIScrollView>();
    }

    public virtual void GetList()
    {
        OnRequestFinishedDelegate callback = OnListRequestCallback;
        SendEvent(postData, webLocation, callback);
    }

    public virtual void OnListRequestCallback(HTTPRequest request, HTTPResponse response)
    {
        base.OnRequestCallBack(request, response);

        JsonWrapperArray<T> wrapper = JsonWrapperArray<T>.FromJson<T>(response.DataAsText);

        scrollView.ResetPosition();
        Game.Instance.ObjectPool.UnspawnAll();

        if (wrapper.state == "1")
        {
            for (int i = 0; i < wrapper.items.Length; i++)
            {
                GameObject info = Game.Instance.ObjectPool.Spawn(spawnName);
                info.transform.parent = transform;
                info.transform.localScale = Vector3.one;
                info.GetComponent<UIListElement>().SetData(wrapper.items[i]);
            }
            GetComponent<UIGrid>().enabled = true;
        }
    }
}
