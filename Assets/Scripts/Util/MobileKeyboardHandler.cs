using UnityEngine;

public class MobileKeyboardHandler : MonoBehaviour
{
    [SerializeField]
    private GameObject[] sourceObjs;

    private AndroidJavaObject jo;

    private float RESOLUTION_HETGHT = 720f;

    private Vector3 oriPos;

    private float keyboardHeight;

    void Start()
    {
        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        jo = jc.GetStatic<AndroidJavaObject>("currentActivity");

        foreach (GameObject obj in sourceObjs)
        {
            UIEventListener.Get(obj).onSelect = selectStatus;
        }

        Reset();
    }

    public void Reset()
    {
        transform.localPosition = oriPos;
#if UNITY_ANDROID
        keyboardHeight = AndroidGetKeyboardHeight();
#endif
    }
    /*
    public void GetLink()
    {
        textLabel.text = url;
        jo.Call("loadWebView", url);
    }*/

    private void selectStatus(GameObject rawObj, bool status)
    {
        if (status)
        {
#if UNITY_ANDROID
            float tempKeyboardHeight = keyboardHeight * RESOLUTION_HETGHT / Screen.height;
            float widgetHeight = rawObj.GetComponent<UIWidget>().height;
            Vector3 targetPos = new Vector2(transform.localPosition.x, (tempKeyboardHeight + widgetHeight) / 2);

            if (targetPos.y > rawObj.transform.localPosition.y)
            {
                Vector3 dist = new Vector3(0.0f, targetPos.y - rawObj.transform.localPosition.y, 0.0f);
                transform.localPosition += dist;
            }
#endif
        }
    }

#if UNITY_ANDROID
    /// <summary>
    /// 获取安卓平台上键盘的高度
    /// </summary>
    /// <returns></returns>
    private int AndroidGetKeyboardHeight()
    {
        AndroidJavaObject view = jo.Get<AndroidJavaObject>("mUnityPlayer").Call<AndroidJavaObject>("getView");
        AndroidJavaObject Rct = new AndroidJavaObject("android.graphics.Rect");
        view.Call("getWindowVisibleDisplayFrame", Rct);

        return Screen.height == Rct.Call<int>("height") ? Screen.height / 2 : Screen.height - Rct.Call<int>("height");
    }
#endif
}
