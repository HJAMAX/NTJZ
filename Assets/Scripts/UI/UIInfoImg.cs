using UnityEngine;

public class UIInfoImg : MonoBehaviour
{
    [SerializeField]
    private string fieldName;

    [SerializeField]
    private GameObject[] imgs;

    void OnEnable()
    {
        imgs[int.Parse(GameData.GetField(fieldName).ToString())].SetActive(true);
    }
}
