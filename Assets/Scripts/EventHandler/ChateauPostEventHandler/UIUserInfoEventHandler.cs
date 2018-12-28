using UnityEngine;

/// <summary>
/// 更新用户信息
/// </summary>
public class UIUserInfo : MonoBehaviour
{
    [SerializeField]
    private Transform[] gameDataFieldList;

    [SerializeField]
    private Transform[] gameDataPropertyList;

    public void ShowGameDataField()
    {
        foreach(Transform gameData in gameDataFieldList)
        {
            gameData.Find("Label").GetComponent<UILabel>().text = GameData.GetField(gameData.name).ToString();
        }
    }

    public void ShowGameDataProperty()
    {
        foreach (Transform gameData in gameDataPropertyList)
        {
            gameData.Find("Label").GetComponent<UILabel>().text = GameData.GetPerperty(gameData.name).ToString();
        }
    }
}
