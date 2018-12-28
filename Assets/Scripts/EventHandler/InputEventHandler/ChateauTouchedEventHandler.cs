using BestHTTP;
using UnityEngine;

/// <summary>
/// 点击进入战斗场景
/// </summary>
public class ChateauTouchedEventHandler : AbstractSendMsgEventHandler
{
    [SerializeField]
    private string msg;

    [SerializeField]
    private string webLocation;

    private ChateauController chateauCtrl;

    public override void HandleEvent(params object[] data)
    {
        if (data.Length > 0 && data[0].GetType() == typeof(GameObject))
        {
            chateauCtrl = ((GameObject)data[0]).GetComponent<ChateauController>();

            if (chateauCtrl.id == GameData.playerChateauData.uid)
            {
                Game.Instance.LoadingScene(GameData.myChateauIndex);
                return;
            }
            if (GameData.itemsData.characters == 0)
            {
                Game.Instance.EventBus.SendEvents("msg_popup", "请至少购买一个角色来参与偷酒");
                return;
            }
            if (chateauCtrl.HasFlag())
            {
                Game.Instance.EventBus.SendEvents("msg_popup", "已经偷过该酒庄！");
                return;
            }
            if (chateauCtrl.IsProgressing())
            {
                Game.Instance.EventBus.SendEvents("msg_popup", "不能偷正在酿酒的酒庄！");
                return;
            }
            if (chateauCtrl.IsLongTimeNoWine())
            {
                Game.Instance.EventBus.SendEvents("msg_popup", "超过24小时没有酿酒，不可以参与偷酒！");
                return;
            }

            PostData[] postDataList = { new PostData("uid", chateauCtrl.id) };
            SendEvent(postDataList, webLocation, OnRequestCallBack);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    public void EnterChateau(string id)
    {
        if (id == GameData.playerChateauData.uid)
        {
            Game.Instance.LoadingScene(GameData.myChateauIndex);
            return;
        }
        if (GameData.itemsData.characters == 0)
        {
            Game.Instance.EventBus.SendEvents("msg_popup", "请至少购买一个角色来参与偷酒");
            return;
        }
        PostData[] postDataList = { new PostData("uid", id) };
        SendEvent(postDataList, webLocation, OnRequestCallBack);
    }

    protected override void OnRequestCallBack(HTTPRequest request, HTTPResponse response)
    {
        base.OnRequestCallBack(request, response);

        JsonWrapperArray<ChateauData> wrapper = JsonWrapperArray<ChateauData>.FromJson<ChateauData>(response.DataAsText);

        if (wrapper.state == "1")
        {
            ChateauData chateauData = wrapper.items[0];
            if (int.Parse(chateauData.wineStored) <= 0)
            {
                Game.Instance.EventBus.SendEvents("msg_popup", "这家伙很懒，滴酒未酿！");
                return;
            }
            if (int.Parse(chateauData.wineLeftPer) <= GameData.wineLeftPerCanBeStolen)
            {
                Game.Instance.EventBus.SendEvents("msg_popup", "该酒庄只剩下不足60%的酒了，请手下留情！");
                return;
            }

            GameData.chosenChateauId = chateauData.uid;
            //GameData.chosenChateauCtrl = chateauCtrl;

            //Game.Instance.EventBus.SendEvents("open_ui", "TeamUp");
            Game.Instance.EventBus.SendEvents("join_room", chateauData.uid, chateauData.nicheng);
        }
    }
}
