using UnityEngine;

public class TestReflection : MonoBehaviour
{
    [SerializeField]
    private string fieldName;

    void Start()
    {
        Debug.Log(GameData.GetField(fieldName).ToString());
    }
}
