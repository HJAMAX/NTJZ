using UnityEngine;
using System.Collections.Generic;
using BestHTTP;

public class PostEvent : AbstractSendMsgEventHandler
{
    [SerializeField]
    private Dictionary<string, string> postDataMap = new Dictionary<string, string>();

    [SerializeField]
    private string webLocation;

    [SerializeField]
    private string callbackEventName;

    public override void HandleEvent(params object[] data)
    {
        base.HandleEvent(null);

        OnRequestFinishedDelegate callback = OnRequestCallBack;
        postDataMap["user_id"] = GameData.playerData.user_id;
       
        Game.Instance.EventBus.SendEvents("post", webLocation, callback, postDataMap);

        postDataMap.Clear();
    }

    public void ResetPostMap()
    {
        postDataMap.Clear();
    }

    public void AddPostData(string name, string data)
    {
        postDataMap[name] = data;
    }

    public void AddGameData(string name, string gameDataFieldName)
    {
        postDataMap[name] = GameData.GetField(gameDataFieldName).ToString();
    }

    /// <summary>
    /// 改变与后端交互的地址和前端的回调函数
    /// </summary>
    public void UpdateWebInfo(string webLocation, string callbackEventName)
    {
        this.webLocation = webLocation;
        this.callbackEventName = callbackEventName;
    }

    protected override void OnRequestCallBack(HTTPRequest request, HTTPResponse response)
    {
        base.OnRequestCallBack(request, response);

        //更新UI
        Game.Instance.EventBus.SendEvents(callbackEventName, response.DataAsText);  
    }
}
