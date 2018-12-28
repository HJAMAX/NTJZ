using UnityEngine;

public class JoyStickBackground : MonoBehaviour
{
    [SerializeField]
    private JoyStickController joyStick;

    void OnPress(bool pressed)
    {
        joyStick.Toggle(pressed);
    }
}
