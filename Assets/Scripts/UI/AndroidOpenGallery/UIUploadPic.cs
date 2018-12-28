using UnityEngine;
using BestHTTP;
using System.Collections.Generic;

public class UIUploadPic : AbstractSendMsgEventHandler
{
    [SerializeField]
    private string webLocation;

    [SerializeField]
    private PostInfo[] postInfoList;

    [SerializeField]
    private UIInput[] inputList;

    [SerializeField]
    private UIGalleryItem[] itemList;

    [SerializeField]
    private Texture2D[] texList;

    [SerializeField]
    private string callbackEventName;

    private List<PostData> postDataList = new List<PostData>();

    public void Upload()
    {
        base.HandleEvent();
        
        postDataList.Clear();
        postDataList.Add(new PostData("user_id", GameData.playerData.user_id));
        //加文字信息
        for (int i = 0; i < postInfoList.Length; i++)
        {
            postDataList.Add(new PostData(postInfoList[i].dataName, postInfoList[i].data));
        }
        for(int i = 0; i < inputList.Length; i++)
        {
            if(inputList[i].value == null || inputList[i].value == "")
            {
                Game.Instance.EventBus.SendEvents("msg_popup", "请输入执照号和地址");
                return;
            }
            postDataList.Add(new PostData(inputList[i].name, inputList[i].value));
        }
        
        //加图片信息
        PostBinaryData[] binaryData = new PostBinaryData[itemList.Length];
        for(int i = 0; i < binaryData.Length; i++)
        {
            if (itemList[i].fileData == null || itemList[i].fileData.Length == 0)
            {
                Game.Instance.EventBus.SendEvents("msg_popup", "请上传图片");
                return;
            }
            if(itemList[i].fileData.Length > 5120000)
            {
                Game.Instance.EventBus.SendEvents("msg_popup", "图片长度不能超过5M");
                return;
            }
            binaryData[i] = new PostBinaryData(itemList[i].name, itemList[i].fileData);
            //binaryData[i] = new PostBinaryData(itemList[i].name, texList[i].EncodeToPNG());
        }
        SendEvent(postDataList.ToArray(), binaryData, webLocation, OnRequestCallback);
    }

    private void OnRequestCallback(HTTPRequest request, HTTPResponse response)
    {
        base.OnRequestCallBack(request, response);

        Game.Instance.EventBus.SendEvents(callbackEventName, response.DataAsText);
    }
}
