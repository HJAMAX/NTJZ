using UnityEngine;
using BestHTTP;

public class UIInvitedFriendsMgr : UIGetListManager<PlayerData>
{
    [SerializeField]
    private string vipTagName;

    public override void GetList()
    {
        if (postData == null)
        {
            postData = new PostData[] { new PostData("game_parent_id", GameData.playerData.user_id) };
        }

        base.GetList();
    }
    
    public override void OnListRequestCallback(HTTPRequest request, HTTPResponse response)
    {
        base.OnRequestCallBack(request, response);

        JsonWrapperArray<PlayerData> wrapper = JsonWrapperArray<PlayerData>.FromJson<PlayerData>(response.DataAsText);

        scrollView.ResetPosition();
        Game.Instance.ObjectPool.UnspawnAll();

        if (wrapper.state == "1")
        {
            for (int i = 0; i < wrapper.items.Length; i++)
            {
                GameObject info = wrapper.items[i].game_leaguer_rank > 0 ?
                    Game.Instance.ObjectPool.Spawn(vipTagName) : Game.Instance.ObjectPool.Spawn(spawnName);
                info.transform.parent = transform;
                info.transform.localScale = Vector3.one;
                info.GetComponent<UIInvitedFriendsTag>().SetData(wrapper.items[i]);
            }
            GetComponent<UIGrid>().enabled = true;
        }
    }
}