using UnityEngine;
using Common;
using ExitGames.Client.Photon;

public abstract class BaseEvent : MonoBehaviour
{
    public EventCode eventCode;

    public abstract void OnEvent(EventData eventData);

    public virtual void Start()
    {
        Game.Instance.serverEngine.AddEvent(this);
    }

    public void OnDestroy()
    {
        Game.Instance.serverEngine.RemoveEvent(eventCode);
    }
}
