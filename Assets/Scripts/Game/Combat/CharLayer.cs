using UnityEngine;

/// <summary>
/// 用于改变场内角色的图层
/// </summary>
public class CharLayer : MonoBehaviour
{
    [SerializeField]
    private int layerOrder;

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Enemy")
        {
            other.GetComponent<EnemyController>().ChangeLayer(layerOrder);
        }
    }
}
