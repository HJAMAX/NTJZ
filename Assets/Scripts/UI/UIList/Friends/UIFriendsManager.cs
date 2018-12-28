using UnityEngine;
using BestHTTP;

public class UIFriendsManager : AbstractSendMsgEventHandler
{
    [SerializeField]
    private string[] webLocation;

    [SerializeField]
    private string spawnName;

    /// <summary>
    /// 更换不同功能
    /// </summary>
    [SerializeField]
    private GameObject[] funcLabel;

    private int funcIndex;

    /// <summary>
    /// 用于复位
    /// </summary>
    private UIScrollView scrollView;

    void Awake()
    {
        scrollView = transform.parent.GetComponent<UIScrollView>();
    }

    /// <summary>
    /// 搜索特定好友请求
    /// </summary>
    public void FindFriendsByNicheng(string nicheng)
    {
        funcIndex = 0;
        funcLabel[0].SetActive(true);
        funcLabel[1].SetActive(false);

        OnRequestFinishedDelegate callback = OnRequestCallback;
        SendEvent(new PostData[] { new PostData("user_id", GameData.playerData.user_id),
                                   new PostData("nicheng", nicheng) }, webLocation[2], callback);
    }

    /// <summary>
    /// 发送获得推荐好友请求
    /// </summary>
    public void GetRandomFriendList()
    {
        funcIndex = 0;
        funcLabel[0].SetActive(true);
        funcLabel[1].SetActive(false);

        OnRequestFinishedDelegate callback = OnRequestCallback;
        SendEvent(webLocation[0], callback);
    }

    /// <summary>
    /// 发送获得我的好友请求
    /// </summary>
    public void GetMyFriendList()
    {
        funcIndex = 1;
        funcLabel[0].SetActive(false);
        funcLabel[1].SetActive(false);

        OnRequestFinishedDelegate callback = OnRequestCallback;
        SendEvent(new PostData[] { new PostData("user_id", GameData.playerData.user_id) }, 
                  webLocation[1], callback);
    }

    /// <summary>
    /// 获得好友列表并拼装UI
    /// </summary>
    /// <param name="request"></param>
    /// <param name="response"></param>
    private void OnRequestCallback(HTTPRequest request, HTTPResponse response)
    {
        base.OnRequestCallBack(request, response);
        JsonWrapperArray<PlayerData> wrapper = JsonWrapperArray<PlayerData>.FromJson<PlayerData>(response.DataAsText);

        if (wrapper.state == "1")
        {
            scrollView.ResetPosition();
            Game.Instance.ObjectPool.UnspawnAll();

            for (int i = 0; i < wrapper.items.Length; i++)
            {
                GameObject friendInfo = Game.Instance.ObjectPool.Spawn(spawnName);
                friendInfo.transform.parent = transform;
                friendInfo.transform.localScale = Vector3.one;
                friendInfo.GetComponent<UIFriendInfo>().SetPlayerData(wrapper.items[i], funcIndex);
            }
            GetComponent<UIGrid>().enabled = true;
        }
        else
        {
            Game.Instance.EventBus.SendEvents("msg_popup", wrapper.msg);
        }
    }
}
