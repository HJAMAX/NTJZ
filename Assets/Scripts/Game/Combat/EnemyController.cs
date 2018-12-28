using UnityEngine;
using Common;

public class EnemyController : MonoBehaviour
{
    /// <summary>
    /// 玩家昵称标识
    /// </summary>
    private GameObject nicknameHud;

    [HideInInspector]
    public bool isLocal = false;

    /// <summary>
    /// 位移同步请求
    /// </summary>
    private SyncPosRequest syncPosRequest;

    /// <summary>
    /// 游戏结束请求
    /// </summary>
    private GameOverRequest gameOverRequest;

    [HideInInspector]
    public CombatPlayerData combatPlayerData;

    private Animator anim;

    private SpriteRenderer spriteRenderer;

    /// <summary>
    /// 角色等级
    /// </summary>
    public int level;

    #region 移动相关属性
    [SerializeField]
    private float repeatRate = 0.2f;

    public Vector2 localMoveSpeed;
    
    public float networkMoveSpeed;
    #endregion

    /// <summary>
    /// 外网传过来的位置，需同步
    /// </summary>
    //[HideInInspector]
    public Vector2 networkPos;

    private Vector2 lastPosition;

    #region 跳跃相关属性

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

    #endregion

    #region 被打退相关属性

    private bool isFallingBack = false;

    [SerializeField]
    private float fallbackSpeed;

    [SerializeField]
    private float fallbackOffset;

    #endregion

    private float timePassed = 0.0f;

    private Vector3 startPos;

    void Awake()
    {
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (nicknameHud)
        {
            nicknameHud.transform.position = WorldToUI(transform.position);
        }

        if (isLocal)
        {
            timePassed += Time.deltaTime;

            if (isMoving)
            {
                transform.Translate(localMoveSpeed * Time.deltaTime);
                spriteRenderer.flipX = localMoveSpeed.x < 0 ? true : false;
            }
        }
        else
        {
            UpdateNetworkPosition();
        }

        if(isJumping)
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

        if(isFallingBack)
        {
            counter += Time.deltaTime;
            float lerp = counter * fallbackSpeed / distToLanded;

            if(lerp < 1.05f)
            {
                transform.localPosition = Vector2.Lerp(jumpPos, landedPos, lerp);
            }
            else
            {
                isMoving = true;
                isFallingBack = false;
                GetComponent<BoxCollider2D>().enabled = true;
            }
        }
    }

    #region 检测交互时间
    /// <summary>
    /// 发送当前位置与网络同步
    /// </summary>
    void SyncPosition()
    {
        if(isLocal && isMoving && Vector2.Distance(transform.position, lastPosition) > 0.01f)
        {
            lastPosition = transform.position;
            combatPlayerData.Pos = new Vector3Data {x = lastPosition.x, y = lastPosition.y, z = 0.0f };
            syncPosRequest.combatPlayerData = combatPlayerData;
            syncPosRequest.DefaultRequest();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(!isLocal)
        {
            return;
        }

        if (other.tag == "Bullet")
        {
            int bulletLevel = other.GetComponent<Bullet>().bulletLevel;
            if (level <= bulletLevel && isLocal)
            {
                GameData.playerStateData.isAlive = false;
                gameOverRequest.DefaultRequest();
            }
            else if(level > other.GetComponent<Bullet>().bulletLevel)
            {
                FallBack(fallbackOffset * bulletLevel);
            }
        }
        else if (other.tag == "Finish")
        {
            if(timePassed < 1.0f)
            {
                ResetPosition();
                return;
            }

            GameData.playerStateData.isAttackSucceed = true;
            GameData.playerStateData.isAlive = false;
            gameOverRequest.DefaultRequest();
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "Limit")
        {
            isMoving = true;
            isFallingBack = false;
            GetComponent<BoxCollider2D>().enabled = true;
        }
    }

    void OnEnable()
    {
        timePassed = 0.0f;
    }

    void OnDisable()
    {
        Game.Instance.ObjectPool.Unspawn(nicknameHud);
        GetComponent<SyncPosRequest>().enabled = false;
        GetComponent<GameOverRequest>().enabled = false;
    }

    public void ResetPosition()
    {
        transform.position = startPos;
    }

    /// <summary>
    /// 同步位置
    /// </summary>
    public void UpdateNetworkPosition()
    {
        float pingInSeconds = (float)Game.Instance.serverEngine.Peer.RoundTripTime * 0.001f;
        float timeSincelastUpdate = (float)Game.Instance.serverEngine.TimeSinceLastUpdate * 0.001f;
        float totalTimePassed = pingInSeconds + timeSincelastUpdate;

        Vector2 curPos = new Vector2(transform.position.x, transform.position.y);
        Vector2 direction = (networkPos - curPos).normalized;
        //spriteRenderer.flipX = direction.x < 0 ? true : false;
        Vector2 targetPos = networkPos + direction * networkPos * totalTimePassed;

        Vector2 newPos = Vector2.MoveTowards(transform.position, targetPos, networkMoveSpeed * Time.deltaTime);
        /*
        if (Vector2.Distance(transform.position, newPos) < 0.01f ||
            Vector2.Distance(transform.position, targetPos) > 2.0f)
        {
            newPos = targetPos;
        }*/

        transform.position = newPos;
    }
    #endregion

    /// <summary>
    /// 遇到障碍物跳起
    /// </summary>
    /// <param name="distance"></param>
    /// <param name="direction"></param>
    /// <param name="animActionName"></param>
    public void Jump(Vector2 distance, int direction, string animActionName = null)
    {
        //确定跳起后的着陆点
        Vector2 boxSize = GetComponent<BoxCollider2D>().bounds.size;
        Vector3 pos = new Vector2(transform.localPosition.x + distance.x + boxSize.x,
                                  transform.localPosition.y + distance.y * direction);
        jumpPos = transform.localPosition;
        distToLanded = (pos - transform.localPosition).magnitude;
        landedPos = pos;

        counter = 0.0f;
        isMoving = false;
        isJumping = true;

        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<Animator>().Play(animActionName);
    }

    /// <summary>
    /// 被子弹打中后退
    /// </summary>
    public void FallBack(float distance)
    {
        Vector3 pos = new Vector2(transform.localPosition.x - distance, transform.localPosition.y);
        jumpPos = transform.localPosition;
        distToLanded = (pos - transform.localPosition).magnitude;
        landedPos = pos;

        counter = 0.0f;
        isMoving = false;
        isFallingBack = true;

        GetComponent<BoxCollider2D>().enabled = false;
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

    /// <summary>
    /// 设为本地可操纵
    /// </summary>
    public void SetLocal(Vector3 sp)
    {
        startPos = sp;
        GameData.playerStateData.isAttackSucceed = false;

        isLocal = true;
        isMoving = true;
        syncPosRequest = GetComponent<SyncPosRequest>();
        syncPosRequest.enabled = true;
        gameOverRequest = GetComponent<GameOverRequest>();
        gameOverRequest.enabled = true;

        Game.Instance.EventBus.SendEvents("player_movement", this);
        Game.Instance.EventBus.SendEvents("open_ui", "Movement");

        InvokeRepeating("SyncPosition", 0.0f, repeatRate);
    }

    /// <summary>
    /// 显示玩家标识
    /// </summary>
    public void SetNicknameHud(string nicheng)
    {
        nicknameHud = isLocal ? Game.Instance.ObjectPool.Spawn("LocalTag") : 
                                Game.Instance.ObjectPool.Spawn("NetworkTag");
        nicknameHud.GetComponent<UILabel>().text = nicheng;
    }

    /// <summary>
    /// 玩家昵称标识跟随
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    private Vector3 WorldToUI(Vector3 pos)
    {
        //将世界坐标转换成屏幕坐标
        pos.y += GetComponent<SpriteRenderer>().bounds.extents.y + 0.3f;
        pos = Camera.main.WorldToScreenPoint(pos);
        pos.z = 0;
        //将屏幕坐标转换成NGUI坐标
        return NGUITools.FindCameraForLayer(nicknameHud.layer).ScreenToWorldPoint(pos);
    }
}
