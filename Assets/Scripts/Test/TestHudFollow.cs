using UnityEngine;

public class TestHudFollow : MonoBehaviour
{
    public GameObject hud;

    private float height;

    void Start()
    {
        height = GetComponent<SpriteRenderer>().bounds.extents.y;
        hud = Game.Instance.ObjectPool.Spawn("TestSprite");
    }

    void Update()
    {
        if(hud)
        {
            hud.transform.position = WorldToUI(transform.position);
        }
    }

    public Vector3 WorldToUI(Vector3 pos)
    {
        //将世界坐标转换成屏幕坐标
        pos.y += height;
        Vector3 pt = Camera.main.WorldToScreenPoint(pos);
        pt.z = 0;
        //将屏幕坐标转换成NGUI坐标
        Vector3 ff = NGUITools.FindCameraForLayer(hud.layer).ScreenToWorldPoint(pt);
        return ff;
    }
}
