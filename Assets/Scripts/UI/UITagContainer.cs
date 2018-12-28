using UnityEngine;

public  class UITagContainer : MonoBehaviour
{
    private GameObject lastTag;

    public void LightUp(GameObject chosenTag)
    {
        if(lastTag != null)
        {
            lastTag.SetActive(false);
        }

        chosenTag.SetActive(true);
        lastTag = chosenTag;
    }
}
