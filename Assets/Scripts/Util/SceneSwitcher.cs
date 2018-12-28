using UnityEngine;

public class SceneSwitcher : MonoBehaviour
{
    /// <summary>
    /// 跳转到景点地图
    /// </summary>
    public void GoScene()
    {
        Game.Instance.LoadingScene(GameData.sceneIndex);
    }

    /// <summary>
    /// 跳转到我的酒庄
    /// </summary>
    public void GoMyChateau()
    {
        if(Screen.orientation != ScreenOrientation.LandscapeLeft)
        {
            Screen.orientation = ScreenOrientation.LandscapeLeft;
        }

        Game.Instance.LoadingScene(GameData.myChateauIndex);
    }

    /// <summary>
    /// 跳转到其他玩家的酒庄
    /// </summary>
    public void GoBattle()
    {
        Game.Instance.LoadingScene(GameData.battleIndex);
    }
    
    /// <summary>
    /// 跳转到大地图场景
    /// </summary>
    public void GoMap()
    {
        Game.Instance.LoadingScene(GameData.mapIndex);
    }

    /// <summary>
    /// 登出
    /// </summary>
    public void GoStart()
    {
        PlayerPrefs.DeleteAll();
        Game.Instance.serverEngine.DisConnect();
        Game.Instance.LoadingScene(GameData.startIndex);
    }

    public void GoMiniGame(string indexStr, string isLandscape)
    {
        Screen.orientation = isLandscape == "1" ? ScreenOrientation.Portrait : ScreenOrientation.LandscapeLeft;
        Game.Instance.ObjectPool.ClearAll();
        Game.Instance.LoadingScene(int.Parse(indexStr));
    }
}
