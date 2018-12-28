using UnityEngine;
using System;

public class BattleEndEventHandler : EventHandler
{
    [SerializeField]
    private UILabel wineStolenLabel;

    [SerializeField]
    private UILabel wineCoinsLabel;

    public override void HandleEvent(params object[] data)
    {
        if (data != null && data.Length > 0)
        {
            Time.timeScale = 0.0f;
            string result = (string)data[0];
            ChateauInMap chateauInMap;

            switch (result)
            {
                case "attack_succeeded" :
                    Game.Instance.EventBus.SendEvents("open_ui", "Success");
                    GameData.chateausInMap.TryGetValue(GameData.chosenChateauId, out chateauInMap);
                    if (chateauInMap.id != null)
                    {
                        //设置这次偷到的酒量
                        GameData.wineStolen = (float)Math.Round(float.Parse((string)data[1]), 2);
                        wineStolenLabel.text = GameData.wineStolen + "ml";
                        wineCoinsLabel.text = (string)data[2];

                        //保存战斗结果,被打败
                        chateauInMap = GameData.chateausInMap[GameData.chosenChateauId];
                        chateauInMap.isDefeated = true;
                        chateauInMap.won = false;
                        GameData.chateausInMap[GameData.chosenChateauId] = chateauInMap;
                    }
                    break;
                case "attack_failed" :
                    Game.Instance.EventBus.SendEvents("open_ui", "Failed");
                    GameData.chateausInMap.TryGetValue(GameData.chosenChateauId, out chateauInMap);
                    if (chateauInMap.id != null)
                    {
                        //保存战斗结果,成功防御
                        chateauInMap.isDefeated = false;
                        chateauInMap.won = true;
                        GameData.chateausInMap[GameData.chosenChateauId] = chateauInMap;
                    }
                    break;
                case "defend_succeeded":
                    Game.Instance.EventBus.SendEvents("open_ui", "DefendSucceeded");
                    break;
                case "defend_failed":
                    Game.Instance.EventBus.SendEvents("open_ui", "DefendFailed");
                    break;
                //case "time_up" :
                  //  Game.Instance.EventBus.SendEvents("open_ui", "TimeUp");
                    //break;
                default: break;
            }
        }
    }

    public void ResetTimeScale()
    {
        Time.timeScale = 1.0f;
    }
}
