using UnityEngine;

/// <summary>
/// 用于判断是否能买某样武器
/// </summary>
public class UIWeaponSelection : MonoBehaviour
{
    [SerializeField]
    private PostEvent postEvent;

    public void Select(PostInfo type, PostInfo orderName, string pos, string openUIName)
    {
        if(GameData.itemsData.HasWeapon(int.Parse(pos)))
        {
            Game.Instance.EventBus.SendEvents("msg_popup", "已拥有该武器");
        }
        else
        {
            postEvent.AddPostData(type.dataName, type.data);
            postEvent.AddPostData(orderName.dataName, orderName.data);
            Game.Instance.EventBus.SendEvents("open_ui", openUIName);
        }
    }
}
