using UnityEngine;
using System;

public class UIInvitedFriendsTag : UIListElement
{
    public override void SetData(params object[] data)
    {
        try
        {
            transform.Find("Label").GetComponent<UILabel>().text = ((PlayerData)data[0]).nicheng;
        }
        catch(Exception e)
        {
            Debug.LogError(e.StackTrace);
        }
    }
}
