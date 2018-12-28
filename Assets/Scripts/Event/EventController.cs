using System.Collections.Generic;
using UnityEngine;

public class EventController : MonoBehaviour
{
    [SerializeField]
    private EventHandler[] list;

    private Dictionary<string, EventHandler> handlerMap = new Dictionary<string, EventHandler>();

    void Awake()
    {
        foreach(EventHandler handler in list)
        {
            RegisterHandler(handler);
        }
    }

    public void HandleEvent(string eventName, params object[] data)
    {
        EventHandler handler = null;

        if (handlerMap.TryGetValue(eventName, out handler))
        {
            handler.HandleEvent(data);
        }
    }

    public void RegisterHandler(EventHandler handler)
    {
        handlerMap.Add(handler.EventName, handler);
    }
}
