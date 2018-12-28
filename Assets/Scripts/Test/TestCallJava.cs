using UnityEngine;

public class TestCallJava : MonoBehaviour
{
    public UILabel label;

    private AndroidJavaObject jo;

    void Start()
    {
        //AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        //jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
        //jo.Call("Play", "appId", "partnerId", "prepayId", "nonceStr", "timeStamp", "sign");
    }

    public void TestClick()
    {
        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
        label.text = jo.Call<string>("wxPlay", "appId", "partnerId", "prepayId", "nonceStr", "timeStamp", "sign");       
    }

    public void Add()
    {
        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
        label.text = jo.Call<int>("add", 1, 2).ToString();
    }
}
