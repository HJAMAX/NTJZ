using System;
using UnityEngine;
using BestHTTP;
using System.Collections.Generic;

public class HttpEventHandler : EventHandler
{
    [SerializeField]
    private string hostUrl = "http://localhost:800/chateau_game/index.php/";

    private OnRequestFinishedDelegate callback;

    /// <summary>
    /// data[0] : url 
    /// data[1] : parameters to upload
    /// data[2] : callback
    /// </summary>
    /// <param name="data"></param>
    public override void HandleEvent(params object[] data)
    {
        if(data[0].GetType() != typeof(string))
        {
            Debug.LogError("");
            return;
        }
        if (data[1].GetType() != typeof(OnRequestFinishedDelegate))
        {
            Debug.LogError("");
            return;
        }

        string url = hostUrl + (string)data[0];
        Debug.Log(url + " data length:" + data.Length);

        callback = (OnRequestFinishedDelegate)data[1];
        HTTPRequest request = new HTTPRequest(new Uri(url), HTTPMethods.Post, HTTPCallback);

        if (data.Length > 2)
        {
            if (data[2].GetType() == typeof(Dictionary<string, string>))
            {
                Dictionary<string, string> postDataMap = (Dictionary<string, string>)data[2];
                foreach (KeyValuePair<string, string> postData in postDataMap)
                {
                    request.AddField(postData.Key, postData.Value);
                    Debug.Log(postData.Key + ":" + postData.Value);
                }
            }
            else if (data[2].GetType() == typeof(PostData[]))
            {
                PostData[] postDataList = (PostData[])data[2];
                foreach (PostData postData in postDataList)
                {
                    Debug.Log(postData.name + ":" + postData.field);
                    request.AddField(postData.name, postData.field);
                }
            }
            else
            {
                Debug.LogError(data[2].GetType().ToString());
            }
        }
        //上传图片
        if(data.Length > 3)
        {
            if(data[3].GetType() == typeof(PostBinaryData[]))
            {
                PostBinaryData[] binaryDataList = (PostBinaryData[])data[3];
                foreach (PostBinaryData binaryData in binaryDataList)
                {
                    Debug.Log(binaryData.name + ":" + binaryData.field.Length);
                    request.AddBinaryData(binaryData.name, binaryData.field, null, "image/png");
                }
            }
        }

        request.Send();
    }

    private void HTTPCallback(HTTPRequest request, HTTPResponse response)
    {
        //Debug.LogError("http");
        //response.
        JsonWrapperArray<string> wrapper = JsonWrapperArray<string>.FromJson<string>(response.DataAsText);
        if(wrapper.state == "-1")
        {
            GameData.returnStartCode = -1;
            Game.Instance.LoadingScene(GameData.startIndex);
            return;
        }

        callback(request, response);
    }
}
