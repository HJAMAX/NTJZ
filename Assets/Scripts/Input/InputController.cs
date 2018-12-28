using UnityEngine;

public class InputController : EventHandler
{
    /// <summary>
    /// 这些UI显示的时候，不可做输入操作
    /// </summary>
    [SerializeField]
    private GameObject[] blockUIList;

    [SerializeField]
    private SpriteRenderer map;

    public SpriteRenderer Map
    {
        get { return map; }
        set { map = value; }
    }

    private Camera cam;

    [SerializeField]
    private float[] zoomBounds;

    [SerializeField]
    private float zoomSpeed;

    [SerializeField]
    private Vector3 oriCamPos;

    private Vector2[] lastZoomPos;

    private bool isMoving;

    private Vector3 lastTouchPos = Vector3.zero;

    /// <summary>
    /// 触碰倒计时
    /// </summary>
    private float touchCountDown = 0.0f;

    /// <summary>
    /// 可否触碰
    /// </summary>
    public bool isTouchEnabled = true;

    void Start()
    {
        cam = GetComponent<Camera>();

        lastZoomPos = new Vector2[] { Vector2.zero, Vector2.zero };
    }

    void Update()
    {
        if(!isTouchEnabled)
        {
            isMoving = false;
            return;
        }
        if(touchCountDown > 0.0f)
        {
            touchCountDown -= Time.deltaTime;
            return;
        }
        //有UI显示的时候不能触发输入
        foreach(GameObject ui in blockUIList)
        {
            if (ui.activeInHierarchy)
            {
                return;
            }
        }
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            //点击场景内物体，触发事件
            RaycastHit2D hit;
            hit = Physics2D.Raycast(cam.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (hit.collider != null)
            {
                Game.Instance.EventBus.SendEvents(hit.collider.tag, hit.collider.gameObject);
            }
        }
#elif UNITY_ANDROID || UNITY_IOS
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                lastTouchPos = cam.ScreenToWorldPoint(touch.position);
                isMoving = false;
            }
            //点击场景内物体，触发事件
            if (touch.phase == TouchPhase.Ended && !isMoving)
            {
                RaycastHit2D hit;
                hit = Physics2D.Raycast(cam.ScreenToWorldPoint(touch.position), Vector2.zero);

                if (hit.collider != null)
                {
                    Game.Instance.EventBus.SendEvents(hit.collider.tag, hit.collider.gameObject);
                }
            }
            //通过划动屏幕移动相机
            else if (touch.phase == TouchPhase.Moved)
            {
                isMoving = true;
                Vector3 newWorldPos = cam.ScreenToWorldPoint(touch.position);
                Vector3 offset = lastTouchPos - newWorldPos;
                transform.Translate(offset.x, offset.y, 0.0f);
   
                ClampCamPos();
            }
            lastTouchPos = cam.ScreenToWorldPoint(touch.position);
        }
#endif


//#endif
        /*
        else if(Input.touchCount == 2)
        {
            Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);
            Vector2[] newZoomPos = new Vector2[] { touch1.position, touch2.position };

            if(touch1.phase == TouchPhase.Began || touch2.phase == TouchPhase.Began)
            {
                lastZoomPos = newZoomPos;
            }
            else if (touch1.phase == TouchPhase.Moved || touch2.phase == TouchPhase.Moved)
            {
                float oldDistance = Vector2.Distance(lastZoomPos[0], lastZoomPos[1]);
                float newDistance = Vector2.Distance(newZoomPos[0], newZoomPos[1]);
                float offset = newDistance - oldDistance;
                lastZoomPos = newZoomPos;

                cam.orthographicSize = Mathf.Clamp(cam.orthographicSize - offset * zoomSpeed, zoomBounds[0], zoomBounds[1]);
                ClampCamPos();
            }
        }*/
    }

    public override void HandleEvent(params object[] data)
    {
        if (data.Length > 0 && data[0].GetType() == typeof(bool))
        {
            isTouchEnabled = (bool)data[0];
        }
    }

    public void ToggleTouch()
    {
        isTouchEnabled = !isTouchEnabled;
    }

    public void centerCamera()
    {
        cam.transform.position = oriCamPos;
    }

    /// <summary>
    /// 点了确定后x时间内输入不能生效
    /// </summary>
    public void MsgPopupCD()
    {
        touchCountDown = 0.2f;
    }

    private void ClampCamPos()
    {
        Vector3 pos = transform.position;
        float camWidth = cam.orthographicSize * cam.aspect;
        pos.x = Mathf.Clamp(transform.position.x, map.bounds.min.x + camWidth, map.bounds.max.x - camWidth);
        pos.y = Mathf.Clamp(transform.position.y, map.bounds.min.y + cam.orthographicSize, map.bounds.max.y - cam.orthographicSize);
        transform.position = pos;
    }
}
