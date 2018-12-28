using UnityEngine;

public class EnemyHandler : EventHandler
{
    /// <summary>
    /// 角色等级
    /// </summary>
    [SerializeField]
    private int maskLevel;

    public int MaskLevel { get { return maskLevel; } }

    [SerializeField]
    private Vector3 oriPos;

    [SerializeField]
    private Vector3 oriSpeed;

    public Vector3 moveSpeed;

    private Animator anim;

    private SpriteRenderer spriteRenderer;

    public bool isMoving = false;

    private bool isJumping = false;

    /// <summary>
    /// 当前点与起跳点的距离
    /// </summary>
    private float distToLanded;

    private Vector2 landedPos;

    private Vector2 jumpPos;

    [SerializeField]
    private float jumpSpeed;

    [SerializeField]
    private float jumpOffset;

    private float counter;

    void Awake()
    {
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    void OnEnable()
    {
        moveSpeed = oriSpeed;
        isMoving = true;
    }

    void OnDisable()
    {
        transform.Find("player_tag").gameObject.SetActive(false);
    }
    
    void Update()
    {
        if (isMoving)
        {
            transform.Translate(moveSpeed);
            spriteRenderer.flipX = moveSpeed.x < 0 ? true : false;
        }
        else if(isJumping)
        {
            counter += Time.deltaTime;
            float lerp = counter * jumpSpeed / distToLanded;

            if (lerp < 1.05f)
            {
                Vector2 virtualPos = Vector2.Lerp(jumpPos, landedPos, lerp);
                float offset = Mathf.Sin(Mathf.PI * ((distToLanded - (virtualPos - jumpPos).magnitude) / distToLanded));
                transform.localPosition = virtualPos + jumpOffset * offset * Vector2.up;
            }
            else
            {
                isMoving = true;
                isJumping = false;
                GetComponent<BoxCollider2D>().enabled = true;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //射程范围内，发送事件
        if (other.tag == "InRange")
        {
            Game.Instance.EventBus.SendEvents("shoot", transform);
        }
        else if (other.tag == "Finish")
        {
            Game.Instance.EventBus.SendEvents("combat", "wine_stolen", gameObject);
        }
    }

    public void Jump(Vector2 distance, int direction, string animActionName)
    {
        //确定跳起后的着陆点
        Vector2 boxSize = GetComponent<BoxCollider2D>().bounds.size;
        Vector3 pos = new Vector2(transform.localPosition.x + distance.x + boxSize.x,
                                transform.localPosition.y + (distance.y) * direction);
        jumpPos = transform.localPosition;
        distToLanded = (pos - transform.localPosition).magnitude;//otherSize.x + boxSize.x;
        landedPos = pos;

        counter = 0.0f;
        isMoving = false;
        isJumping = true;
        GetComponent<BoxCollider2D>().enabled = false;

        GetComponent<Animator>().Play(animActionName);
    }

    public void ChangeLayer(int layerIndex)
    {
        GetComponent<SpriteRenderer>().sortingOrder = layerIndex;
    }

    public void Stop()
    {
        isMoving = false;
        isJumping = false;
    }

    public override void HandleEvent(params object[] data)
    {

    }
}
