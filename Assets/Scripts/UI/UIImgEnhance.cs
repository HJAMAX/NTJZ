using UnityEngine;

public class UIImgEnhance : MonoBehaviour
{
    /// <summary>
    /// 未被选中时，图层降低
    /// </summary>
    public int unchosenDepth;

    /// <summary>
    /// 被选中时，图层升高
    /// </summary>
    public int chosenDepth;

    public Vector3 shink;

    public Vector3 enhance;

    void OnTriggerEnter(Collider other)
    {
        other.GetComponent<UICharImg>().ModifyImg(enhance, chosenDepth, true);
    }

    void OnTriggerExit(Collider other)
    {
        other.GetComponent<UICharImg>().ModifyImg(shink, unchosenDepth, false);
    }
}
