using UnityEngine;

/// <summary>
/// 用于下拉框选择
/// </summary>
public class UILocalListSelectElement : UILocalListElement
{
    protected PostInfo postInfo;

    void Start()
    {
        postInfo = GetComponent<PostInfo>();
    }

    /// <summary>
    /// 更改目标对象
    /// </summary>
    public override void Select()
    {
        base.Select();
        postEvent.AddPostData(postInfo.dataName, postInfo.data);
    }
}
