using UnityEngine;
using BestHTTP;

public class TestUploadPic : AbstractSendMsgEventHandler
{
    public string webLocation;

    public string dataName;

    public string data;

    public string picName;

    public Texture2D pic;
    
    public void Upload()
    {
        byte[] fileData = pic.EncodeToPNG();
        SendEvent(new PostData[] { new PostData(dataName, data)},
                  new PostBinaryData[] { new PostBinaryData(picName, fileData)},
                  webLocation, OnRequestCallback);
    }

    public void OnRequestCallback(HTTPRequest request, HTTPResponse response)
    {
        base.OnRequestCallBack(request, response);
        Game.Instance.EventBus.SendEvents("msg_popup", response.DataAsText);
    }
}
