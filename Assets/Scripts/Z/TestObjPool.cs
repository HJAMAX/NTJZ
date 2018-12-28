using UnityEngine;

public class TestObjPool : MonoBehaviour
{
    [SerializeField]
    private string fieldName;

    void Start()
    {
        Game.Instance.ObjectPool.Spawn(fieldName);
    }
}
