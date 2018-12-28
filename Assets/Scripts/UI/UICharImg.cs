using UnityEngine;

//被选中的角色被放大
public class UICharImg : MonoBehaviour
{
    [SerializeField]
    private int index;

    [SerializeField]
    private UISprite[] charImg;

    private GameObject chosenBG;

    private UIPanel parentPanel;

    void Start()
    {
        parentPanel = transform.parent.GetComponent<UIPanel>();
        chosenBG = transform.Find("Chosen").gameObject;
    }

    void OnEnable()
    {
        charImg[int.Parse(GameData.playerData.sex)].gameObject.SetActive(true);
    }

    /// <summary>
    /// 放大缩小图像，改变图层
    /// </summary>
    public void ModifyImg(Vector3 scale, int depth, bool isChosen)
    {
        transform.localScale = scale;
        parentPanel.depth = depth;
        chosenBG.SetActive(isChosen);

        Game.Instance.EventBus.SendEvents("char_info", index);
    }
}
