using UnityEngine;
using BestHTTP;

public class LoginEventHandler : AbstractSendMsgEventHandler
{
    [SerializeField]
    private UILabel mobileLabel;

    [SerializeField]
    private UIInput passwdInput;

    [SerializeField]
    private string loginLocation;

    private string mobile;

    private string password;

    void Start()
    {
        switch(GameData.returnStartCode)
        {
            case -1:
                Game.Instance.EventBus.SendEvents("msg_popup", "服务器暂时维修中，请稍后再试");
                break;
            case 1 :
                Game.Instance.EventBus.SendEvents("msg_popup", "偷酒中途断线退出");
                break;
            case 2 :
                Game.Instance.EventBus.SendEvents("msg_popup", "断线退出");
                break;
        }
    }

    void OnEnable()
    {
        mobileLabel.text = PlayerPrefs.GetString("mobile");
        passwdInput.value = PlayerPrefs.GetString("password");
    }

    public void Login()
    {
        base.HandleEvent(null);
        //cellArray = cellPositions.ToArray();
        OnRequestFinishedDelegate callback = OnRequestCallback;

        mobile = mobileLabel.text;
        password = passwdInput.value;

        PostData[] postDataList = { new PostData("mobile", mobileLabel.text),
                                    new PostData("password", password) ,
                                    new PostData("version", Application.version)};

        SendEvent(postDataList, loginLocation, callback);
    }

    public override void HandleEvent(params object[] data)
    {
        base.HandleEvent(data);
    }

    void OnRequestCallback(HTTPRequest request, HTTPResponse response)
    {
        base.OnRequestCallBack(request, response);
        
        JsonWrapperArray<string> wrapper = JsonWrapperArray<string>.FromJson<string>(response.DataAsText);
        
        //用户名密码正确
        if(wrapper.state == "1")
        {
            GameData.playerData = JsonUtility.FromJson<PlayerData>(wrapper.items[0]);
            //GameData.itemsData = JsonUtility.FromJson<ItemsData>(wrapper.items[1]);
            if(wrapper.items[1] != "null")
                GameData.upgradeData = JsonUtility.FromJson<UpgradeData>(wrapper.items[1]);

            PlayerPrefs.SetString("mobile", mobile);
            PlayerPrefs.SetString("password", password);
            
            Game.Instance.LoadingScene(GameData.myChateauIndex);
        }
        //版本号不对
        else if(wrapper.items != null && wrapper.items[0] == "0")
        {
            Game.Instance.EventBus.SendEvents("version_msg_popup", wrapper.msg);
        }
        else
        {
            Game.Instance.EventBus.SendEvents("msg_popup", wrapper.msg);
        }
    }
}