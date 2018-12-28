using UnityEngine;
using BestHTTP;
using System.Collections.Generic;

/// <summary>
/// 点击酿酒器：开始酿酒或取酒
/// </summary>
public class BrewbotEventHandler : AbstractSendMsgEventHandler
{
    [SerializeField]
    private string webLocation;

    private Dictionary<string, string> postDataMap = new Dictionary<string, string>();

    private BrewbotState state = BrewbotState.NOT_AVAILABLE;

    public BrewbotState State { get { return state; } }

    private static Dictionary<BrewbotState, string> stateMap;

    void Awake()
    {
        if (stateMap == null)
        {
            stateMap = new Dictionary<BrewbotState, string>();
            stateMap[BrewbotState.DONE] = "可取酒";
            stateMap[BrewbotState.AVAILABLE] = "空闲";
            stateMap[BrewbotState.NOT_AVAILABLE] = "不可用";
        }
    }

    void Start()
    {
        //postDataMap["player_id"] = GameData.playerData.id;
    }

    /// <summary>
    /// 在my_chateau场景中点击酿酒器来到这里触发酿酒器事件
    /// </summary>
    /// <param name="data"></param>
    public override void HandleEvent(params object[] data)
    {
        //if (data != null && data.Length > 0 && data[0].GetType() == typeof(GameObject))
        //{
            //GameObject gameObj = (GameObject)data[0];
            //Brewbot brewbot = gameObj.GetComponent<Brewbot>();
            OnRequestFinishedDelegate callback = OnRequestCallBack;

            //若酿酒器空闲，则开始酿酒
            if (state == BrewbotState.AVAILABLE)
            {
                base.HandleEvent();
                webLocation += "makeWine";
                //postDataMap["brewbot_num"] = brewbot.Number.ToString();
                Game.Instance.EventBus.SendEvents("post", webLocation, callback, postDataMap);
            }
            else if (state == BrewbotState.PROGRESSING)
            {

            }
            /*
            //若酿酒器酒满了，则取酒
            else if(brewbot.State == BrewbotState.DONE)
            {
                base.HandleEvent();
                webLocation += "getWine";
                postDataMap["brewbot_num"] = brewbot.Number.ToString();
                Game.Instance.EventBus.SendEvents("post", webLocation, callback, postDataMap);
            }
        }
        else
        {
            Debug.LogError(data);
        }*/
    }

    /// <summary>
    /// 更新UI：酿酒器的状态
    /// </summary>
    protected override void OnRequestCallBack(HTTPRequest request, HTTPResponse response)
    {
        base.OnRequestCallBack(request, response);

        JsonWrapperArray<string> wrapper = JsonWrapperArray<string>.FromJson<string>(response.DataAsText);
        PostData[] postDataList = JsonWrapperArray<string>.FromJsonStr(wrapper.items[0]);
        /*
        //更新UI
        foreach (PostData postData in postDataList)
        {
            Game.Instance.EventBus.SendEvents(postData.name, postData.field);
        }*/
    }
}

