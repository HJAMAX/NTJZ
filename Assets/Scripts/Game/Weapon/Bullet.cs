using UnityEngine;

public class Bullet : ReusableObject
{
    [SerializeField]
    private Vector2 unaccessablePoint;

    /// <summary>
    /// 位移修正
    /// </summary>
    [SerializeField]
    private Vector2 positionAdjustment;
    
    // Ballistic trajectory offset (in distance to target)
    public float ballisticOffset = 0.5f;

    public float speedUpOverTime = 0.5f;

    public int bulletLevel;

    public int explosionIndex = -1;

    // From this position bullet was fired
    private Vector2 originPoint;
    
    //Last target's position;
    private Vector2 aimPoint;
    //Current position without ballistic offset
    private Vector2 virtualPos;
    //Positin on last frame
    private Vector3 prePos = Vector3.zero;
    //Counter for acceleration calculation
    private float counter = 0.0f;

    private bool canExplode = false;

    void OnEnable()
    {
        canExplode = true;
    }

    public override void OnUnspawn()
    {
        base.OnUnspawn();
        GetComponent<SpriteRenderer>().enabled = false;
        transform.position = unaccessablePoint;
        counter = 0.0f;
    }

    public void SetTarget(Vector2 targetPos)
    {
        aimPoint = targetPos;
    }

    public void Fire()
    {
        originPoint = virtualPos = prePos = transform.position;
    }

    protected void FixedUpdate()
    {
        if(!GetComponent<SpriteRenderer>().enabled)
        {
            return;
        }
        if (transform.position.x <= aimPoint.x)
        {
            if (explosionIndex > 0 && canExplode)
            {
                Game.Instance.EventBus.SendEvents("explosion", explosionIndex, transform.position);
                canExplode = false;
            }
            Game.Instance.ObjectPool.Unspawn(gameObject);
        }

        counter += Time.fixedDeltaTime;

        Vector2 originDistance = aimPoint - originPoint;
        Vector2 distanceToAim = aimPoint - virtualPos;
        virtualPos = Vector2.Lerp(originPoint, aimPoint, counter * speedUpOverTime / originDistance.magnitude) + positionAdjustment;

        transform.position = AddBallisticOffset(originDistance.magnitude, distanceToAim.magnitude);

        LookAtDirection2D(prePos - transform.position);
        prePos = transform.position;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        /*
        if(other.tag == "Enemy")
        {
            //减少弹药
            if(other.GetComponent<EnemyHandler>().MaskLevel <= bulletLevel)
            {
                Game.Instance.EventBus.SendEvents("combat", "enemy_killed", other.gameObject);
            }
            if(explosionIndex > 0 && canExplode)
            {
                Game.Instance.EventBus.SendEvents("explosion", explosionIndex, transform.position);
                canExplode = false;
            }
            Game.Instance.ObjectPool.Unspawn(gameObject);           
        }*/
    }

    private Vector2 AddBallisticOffset(float originDistance, float distanceToAim)
    {
        if(ballisticOffset > 0.0f)
        {
            float offset = Mathf.Sin(Mathf.PI * ((originDistance - distanceToAim) / originDistance));
            offset *= originDistance;
            return virtualPos + (ballisticOffset * offset * Vector2.up);
        }
        else
        {
            return virtualPos;
        }
    }

    /// <summary>
    /// 调整方向
    /// </summary>
    /// <param name="direction"></param>
    private void LookAtDirection2D(Vector2 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}
