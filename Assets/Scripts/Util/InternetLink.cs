using UnityEngine;
using System.Runtime.InteropServices;

public class InternetLink : MonoBehaviour
{
    [SerializeField]
    private string url;

    private AndroidJavaObject jo;

    void Start()
    {
#if UNITY_ANDROID
        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
#endif
    }

    [DllImport("__Internal")]
    private static extern void __TurnWeb(string url);

    public void GetLink()
    {
#if UNITY_ANDROID
        jo.Call("loadWebView", url);
#elif UNITY_IOS
        __TurnWeb(url);
#endif
    }
}
