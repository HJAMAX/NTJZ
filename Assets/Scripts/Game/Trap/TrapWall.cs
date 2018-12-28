using UnityEngine;

public class TrapWall : MonoBehaviour
{
    /// <summary>
    /// 能够跳过的等级
    /// </summary>
    [SerializeField]
    private int levelCanJump;

    [SerializeField]
    private int direction;

    /// <summary>
    /// 不同的障碍触发不同的动作
    /// </summary>
    [SerializeField]
    private string animActionName;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy" && other.GetComponent<EnemyController>().level >= levelCanJump)
        {
            Vector2 boxSize = transform.parent.GetComponent<BoxCollider2D>().bounds.size;
            other.GetComponent<EnemyController>().Jump(boxSize, direction, animActionName);
        }
    }
}
