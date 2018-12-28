using UnityEngine;

public class JoyStickController : MonoBehaviour
{
    public Vector2 moveDirect = Vector2.zero;

    [SerializeField]
    private UIRoot uiRoot;

    private bool isPressed;

    [SerializeField]
    private float radius;

    private float rootWidth;

    private float rootHeight;

    /// <summary>
    /// 摇杆初始位置
    /// </summary>
    private Vector2 joyOriginPos;

    private Vector2 screenRatio;

    void OnEnable()
    {
        Transform joyStickBG = transform.parent;

        float ratio = Screen.height / (float)Screen.width;
        rootHeight = uiRoot.activeHeight;
        rootWidth = uiRoot.activeHeight / ratio;

        screenRatio = new Vector2(rootWidth / Screen.width, rootHeight / Screen.height);

        joyOriginPos = new Vector2(joyStickBG.localPosition.x + rootWidth / 2, 
                                   joyStickBG.localPosition.y + uiRoot.activeHeight / 2);
    }

    void Update()
    {
        if (isPressed)
        {
            Vector2 touchPos = UICamera.lastEventPosition;
            Vector2 screenPos = new Vector2(touchPos.x * screenRatio.x, touchPos.y * screenRatio.y);
            // 默认情况下，坐标原点位于父精灵左下角，下面的代码是调整原点到父物体中心
            screenPos -= joyOriginPos;
            // 计算原点和触摸点的距离
            float dist = Vector2.Distance(screenPos, Vector2.zero);
            if (dist < radius)
            {
                transform.localPosition = screenPos;
            }
            else
            {
                //触摸点到原点的距离超过半径，则把子精灵按钮的位置设置为在父精灵背景的圆上，即控制摇杆只能在父精灵圆内移动
                transform.localPosition = screenPos.normalized * radius;
            }
            moveDirect = transform.localPosition.normalized;
        }
        else
        {
            moveDirect = transform.localPosition = Vector2.zero;
        }
    }

    void OnPress(bool pressed)
    {
        isPressed = pressed;
    }

    /// <summary>
    /// 点击背景也能移动摇杆
    /// </summary>
    /// <param name="pressed"></param>
    public void Toggle(bool pressed)
    {
        isPressed = pressed;
    }
}
