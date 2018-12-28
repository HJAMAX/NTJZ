using UnityEngine;

public abstract class EventHandler : MonoBehaviour
{
    [SerializeField]
    private string eventName;

    public string EventName { get { return eventName; } }

    public abstract void HandleEvent(params object[] data);
}
