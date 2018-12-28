using BestHTTP;
using UnityEngine;

/// <summary>
/// 用于处理注册以及用户信息变更
/// </summary>
public class UserDataEventHandler : AbstractSendMsgEventHandler
{
    [SerializeField]
    private string callbackOpenPanel;

    [SerializeField]
    private UILabel nicknameLabel;

    [SerializeField]
    private UILabel mobileLabel;

    [SerializeField]
    private UIInput passwdInput;

    [SerializeField]
    private UILabel refereeLabel;

    [SerializeField]
    private UILabel variCodeLabel;

    [SerializeField]
    private string smsWebLocation;

    [SerializeField]
    private string webLocation;

    [SerializeField]
    private string sex;

    public void SetSex(string s)
    {
        sex = s;
    }

    public void SendSms()
    {
        base.HandleEvent(null);

        OnRequestFinishedDelegate callback = OnSmsCallback;
        PostData[] postDataList = { new PostData("mobile", mobileLabel.text) };

        SendEvent(postDataList, smsWebLocation, callback);
    }

    public void UpdateUserData()
    {
        string nickname = "";
        string mobile = "";
        string passwd = "";
        string referee = "";
        string code = "";

        if (nicknameLabel != null)
        {
            if (nicknameLabel.text == "")
            {
                Game.Instance.EventBus.SendEvents("msg_popup", "昵称不能为空");
                return;
            }
            nickname = nicknameLabel.text;
        }
        if(mobileLabel != null)
        {
            if (mobileLabel.text == "")
            {
                Game.Instance.EventBus.SendEvents("msg_popup", "手机号不能为空");
                return;
            }
            mobile = mobileLabel.text;
        }
        if(passwdInput != null)
        {
            if (passwdInput.value == "")
            {
                Game.Instance.EventBus.SendEvents("msg_popup", "密码不能为空");
                return;
            }
            passwd = passwdInput.value;
        }
        if(variCodeLabel != null)
        {
            if (variCodeLabel.text == "")
            {
                Game.Instance.EventBus.SendEvents("msg_popup", "验证码不能为空");
                return;
            }
            code = variCodeLabel.text;
        }
        if(refereeLabel != null)
        {
            referee = refereeLabel.text;
        }

        base.HandleEvent(null);

        OnRequestFinishedDelegate callback = OnUpdateCallback;
        PostData[] postDataList = { new PostData("mobile", mobile),
                                    new PostData("code", code),
                                    new PostData("nicheng", nickname),
                                    new PostData("referee_mobile", referee),
                                    new PostData("password", passwd),
                                    new PostData("sex", sex)};

        SendEvent(postDataList, webLocation, callback);
    }

    public override void HandleEvent(params object[] data)
    {
        base.HandleEvent(data);
    }

    private void OnSmsCallback(HTTPRequest request, HTTPResponse response)
    {
        base.OnRequestCallBack(request, response);

        JsonWrapperArray<object> wrapper = JsonWrapperArray<object>.FromJson<object>(response.DataAsText);
        Game.Instance.EventBus.SendEvents("msg_popup", wrapper.msg);
    }

    private void OnUpdateCallback(HTTPRequest request, HTTPResponse response)
    {
        base.OnRequestCallBack(request, response);

        JsonWrapperArray<PlayerData> wrapper = JsonWrapperArray<PlayerData>.FromJson<PlayerData>(response.DataAsText);

        Game.Instance.EventBus.SendEvents("msg_popup", wrapper.msg);

        if (wrapper.items.Length > 1)
        {
            GameData.playerData = wrapper.items[0];
            Game.Instance.EventBus.SendEvents("open_ui", callbackOpenPanel);
            gameObject.SetActive(false);
        }
    }
}
