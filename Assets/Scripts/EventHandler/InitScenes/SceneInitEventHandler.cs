using System.Collections.Generic;
using UnityEngine;
using BestHTTP;

public class SceneInitEventHandler : AbstractSendMsgEventHandler
{
    [SerializeField]
    private SpriteRenderer map;

    [SerializeField]
    private string[] mapPaths;

    /// <summary>
    /// 酒庄asset的前缀名
    /// </summary>
    [SerializeField]
    private string spawnName;

    /// <summary>
    /// 每张地图的酒庄所在的地图位置
    /// </summary>
    [SerializeField]
    private ChateauPositions[] chateauPositions;

    [SerializeField]
    private string webLocation;

    void Awake()
    {
        Sprite mapSprite = Instantiate(Resources.Load<Sprite>(mapPaths[GameData.chosenMapId]));
        map.sprite = mapSprite;
        
        //如果地图上的酒庄已经初始化过，则读取上次保存的信息
        if(GameData.lastChosenMapId == GameData.chosenMapId && GameData.chateausInMap != null)
        {
            foreach(KeyValuePair<string, ChateauInMap> pair in GameData.chateausInMap)
            {
                GameObject chateauObj = Game.Instance.ObjectPool.Spawn(spawnName + pair.Value.level);
                chateauObj.transform.position = pair.Value.position;

                ChateauController chateauCtrl = chateauObj.GetComponent<ChateauController>();
                chateauCtrl.Init(pair.Value.id, pair.Value.nicheng, pair.Value.startTime,
                                 pair.Value.stopTime, pair.Value.wineStored, pair.Value.wineLeftPer);

                if (pair.Value.isDefeated)
                    chateauCtrl.SetFlag("fail_flag", true);
                else if(pair.Value.won)
                    chateauCtrl.SetFlag("win_flag", true);
            }
            return;
        }

        GameData.lastChosenMapId = GameData.chosenMapId;
        //若第一次登入，则重新加载
        Reload();
    }

    void Start()
    {
        Game.Instance.EventBus.SendEvents("gold_coins_info", GameData.playerData.goldCoins.ToString());
    }

    /// <summary>
    /// 接收其他玩家信息，包括用户名，ID之后，在地图上显示
    /// </summary>
    /// <param name="data"></param>
    public override void HandleEvent(params object[] data)
    {

    }

    /// <summary>
    /// 重新加载
    /// </summary>
    public void Reload()
    {
        base.HandleEvent();
        Game.Instance.ObjectPool.UnspawnAll();
        OnRequestFinishedDelegate callback = OnRequestCallback;
        string chateauCount = chateauPositions[GameData.chosenMapId].positions.Length.ToString();
        SendEvent(new PostData[] { new PostData("chateau_count", chateauCount) }, webLocation, callback);
    }

    void OnRequestCallback(HTTPRequest request, HTTPResponse response)
    {
        base.OnRequestCallBack(request, response);
        JsonWrapperArray<ChateauData> wrapper = JsonWrapperArray<ChateauData>.FromJson<ChateauData>(response.DataAsText.ToString());

        if (wrapper.state == "1")
        {
            ChateauData[] items = wrapper.items;
            Game.Instance.ObjectPool.UnspawnAll();
            Dictionary<string, ChateauInMap> chateausInMap = new Dictionary<string, ChateauInMap>();
            if (GameData.chateausInMap == null)
            {
                GameData.chateausInMap = new Dictionary<string, ChateauInMap>();
            }

            int mapId = GameData.chosenMapId;
            //int chateauCount = chateauPositions[mapId].positions.Length;

            for (int i = 0; i < items.Length; i++)
            {
                if(items[i].level > 0)
                {
                    ChateauController chateauObj = Game.Instance.ObjectPool.Spawn(spawnName + items[i].level).GetComponent<ChateauController>();
                    chateauObj.transform.position = chateauPositions[mapId].positions[i];
                    //初始化
                    chateauObj.Init(items[i].uid, items[i].nicheng, items[i].wineStartTime, items[i].wineStopTime,
                                    items[i].wineStored, items[i].wineLeftPer);
                    //保存这次的酒庄信息
                    if (GameData.chateausInMap.ContainsKey(items[i].uid))
                    {
                        if (GameData.chateausInMap[items[i].uid].isDefeated)
                        {
                            chateauObj.SetFlag("fail_flag", true);
                        }
                        else if (GameData.chateausInMap[items[i].uid].won)
                            chateauObj.SetFlag("win_flag", true);
                    }
                    chateausInMap.Add(items[i].uid, new ChateauInMap(items[i].nicheng, items[i].uid, items[i].level, chateauPositions[mapId].positions[i],
                                                                              items[i].wineStartTime, items[i].wineStopTime, items[i].wineStored, items[i].wineLeftPer));
                }
            }
            GameData.chateausInMap.Clear();
            GameData.chateausInMap = chateausInMap;
        }
        else
        {
            Game.Instance.EventBus.SendEvents("msg_popup", wrapper.msg);
        }
    }
}
