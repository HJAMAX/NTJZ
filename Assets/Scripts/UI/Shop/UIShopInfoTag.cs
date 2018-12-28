using UnityEngine;

public class UIShopInfoTag : MonoBehaviour
{
    [SerializeField]
    private string fieldName;

    void OnEnable()
    {
        GetComponent<UILabel>().text = GameData.shopData.GetField(fieldName).ToString();
    }
}
