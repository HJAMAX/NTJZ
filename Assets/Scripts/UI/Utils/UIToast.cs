using UnityEngine;
using System.Collections;

public class UIToast : EventHandler
{
    private UILabel contentLabel;

    /// <summary>
    /// 
    /// </summary>
    [SerializeField]
    private TweenAlpha alphaEffect;

    [SerializeField]
    private float disappearTime = 1.0f;

    void OnDisable()
    {
        alphaEffect.value = 1.0f;
    }

    void Update()
    {
        if(alphaEffect.value < 0.01)
        {
            gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 显示需要显示的内容
    /// </summary>
    /// <param name="content"></param>
    public void OnClick(string content)
    {
        contentLabel = contentLabel == null ? transform.Find("Label").GetComponent<UILabel>() : contentLabel;
        contentLabel.text = content;
        ShowToast();
    }

    public override void HandleEvent(params object[] data)
    {
        if(data.Length == 1 && data[0].GetType() == typeof(string))
        {
            OnClick((string)data[0]);
        }
        else if(data.Length == 0)
        {
            ShowToast();
        }
    }

    private void ShowToast()
    {
        if (gameObject.activeInHierarchy && alphaEffect.value <= 1.0f && alphaEffect.value >= 0.01)
        {
            return;
        }

        gameObject.SetActive(true);
        TweenAlpha.Begin(alphaEffect.gameObject, 2.0f, 0.0f);
    }
}
