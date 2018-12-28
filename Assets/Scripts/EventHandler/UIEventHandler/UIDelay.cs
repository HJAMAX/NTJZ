using System;
using System.Collections.Generic;
using UnityEngine;

public class UIDelay : EventHandler
{
    [SerializeField]
    private GameObject combatUI;

    public override void HandleEvent(params object[] data)
    {
        if(data.Length == 1)
        {
            bool isPopup = (bool)data[0];
            combatUI.gameObject.SetActive(isPopup);
        }
        else if (data.Length == 2)
        {
            float timeLag = (float)data[0];
            bool isPopup = (bool)data[1];
            IEnumerator<WaitForSeconds> showUI = ShowUICoroutine(timeLag, isPopup);
            StartCoroutine(showUI);
        }
    }

    IEnumerator<WaitForSeconds> ShowUICoroutine(float timeLag, bool isPopup)
    {
        yield return new WaitForSeconds(timeLag);
        combatUI.gameObject.SetActive(isPopup);
    }
}
