using UnityEngine;

public class TestCheckBox : MonoBehaviour
{
    private UIToggle toggle;

    [SerializeField]
    private bool boolean;

    void Start()
    {
        toggle = GetComponent<UIToggle>();
        toggle.value = boolean;
    }

    public void OnChange()
    {
        Debug.LogError(toggle.value);
    }
}
