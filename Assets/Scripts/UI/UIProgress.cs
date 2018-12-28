using System.Collections;
using UnityEngine;

public class UIProgress : MonoBehaviour
{
    [SerializeField]
    private Vector4 barSize;

    private int start;

    private int end;

    private int rest;

    private UIPanel prgsPanel;

    void Awake()
    {
        prgsPanel = GetComponent<UIPanel>();
    }

    public void SetProgressBar(int s, int e, int r)
    {
        start = s;
        end = e;
        rest = r;
        
        IEnumerator coroutine = CountDown();
        StartCoroutine(coroutine);
    }

    private IEnumerator CountDown()
    {
        int total = end - start;

        while (rest < total)
        {
            rest++;
            float barLength = (float) rest / total * barSize.z;
            prgsPanel.baseClipRegion = new Vector4(barSize.x, barSize.y, barLength, barSize.w);

            yield return new WaitForSeconds(1.0f);
        }
    }

    void OnDisable()
    {
        StopAllCoroutines();
    }
}
