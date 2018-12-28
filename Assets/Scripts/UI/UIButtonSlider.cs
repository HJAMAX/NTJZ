using UnityEngine;

public class UIButtonSlider : MonoBehaviour
{
    [SerializeField]
    private int slideDistance;

    private int currentIdx = 0;

    public string CurrentIdx { get { return currentIdx.ToString(); } }

    private int originXPos;

    void Start()
    {
        originXPos = (int)transform.localPosition.x;
    }

    public void Slide()
    {
        int childCount = transform.childCount;
        currentIdx = (currentIdx + childCount + 1) % childCount;

        Vector3 position = transform.localPosition;
        position.x = (currentIdx) * slideDistance + originXPos;
        transform.localPosition = position;
    }
}
