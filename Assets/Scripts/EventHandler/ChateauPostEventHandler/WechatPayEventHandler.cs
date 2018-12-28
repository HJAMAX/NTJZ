using UnityEngine;
using BestHTTP;
using System.Runtime.InteropServices;

public class WechatPayEventHandler : AbstractSendMsgEventHandler
{
    [SerializeField]
    private string webLocation;

    [SerializeField]
    private string wxFuncName;

    [SerializeField]
    private string callbackFuncName;

#if UNITY_ANDROID
    private AndroidJavaObject jo;
#endif

    void Start()
    {
#if UNITY_ANDROID
        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
#endif
    }

    public void OnWxpayCallback(string isPassed)
    {
        if (isPassed == "ok")
        {
            base.HandleEvent();
            OnRequestFinishedDelegate callback = OnRequestCallBack;
            SendEvent(new PostData[] { new PostData("user_id", GameData.playerData.user_id)},
                                       webLocation, callback);
        }
    }

    public override void HandleEvent(params object[] data)
    {
        if (data.Length > 0 && data[0].GetType() == typeof(string))
        {
            JsonWrapperArray<WechatPayData> wrapper = JsonWrapperArray<WechatPayData>.FromJson<WechatPayData>((string)data[0]);
            WechatPayData payData = wrapper.items[0];
#if UNITY_ANDROID
            jo.Call(wxFuncName, gameObject.name, callbackFuncName,
                    payData.appid, payData.partnerid, payData.prepayid,
                    payData.noncestr, payData.timestamp, payData.sign);
#elif UNITY_IOS
            __WXPay(payData.partnerid, payData.prepayid, payData.noncestr, payData.timestamp, payData.sign);
#endif
        }
    }

    protected override void OnRequestCallBack(HTTPRequest request, HTTPResponse response)
    {
        base.OnRequestCallBack(request, response);
        JsonWrapperArray<string> wrapper = JsonWrapperArray<string>.FromJson<string>(response.DataAsText);

        GameData.playerData = JsonUtility.FromJson<PlayerData>(wrapper.items[0]);
        GameData.itemsData = JsonUtility.FromJson<ItemsData>(wrapper.items[1]);
        if(wrapper.items[2] != "null")
        {
            GameData.upgradeData = JsonUtility.FromJson<UpgradeData>(wrapper.items[2]);
        }
        Game.Instance.EventBus.SendEvents("upgrade_user_level");

        Game.Instance.EventBus.SendEvents("gold_coins_info", GameData.playerData.goldCoins.ToString());
        Game.Instance.EventBus.SendEvents("msg_popup", "购买成功");
    }
}
