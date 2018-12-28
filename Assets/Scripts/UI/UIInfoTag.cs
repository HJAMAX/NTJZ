using UnityEngine;

public class UIInfoTag : MonoBehaviour
{
    /// <summary>
    /// 变量对应的名字
    /// </summary>
    [SerializeField]
    private string fieldName;

    void OnEnable()
    {
        GetComponent<UILabel>().text = GameData.GetField(fieldName).ToString();
    }
}
