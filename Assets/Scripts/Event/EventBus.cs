using UnityEngine;
using System.Collections.Generic;

public class EventBus : Singleton<EventBus>
{
    public List<EventController> list = new List<EventController>();

    public void RegisterController(EventController controller)
    {
        if (!list.Contains(controller))
        {
            list.Add(controller);
        }
    }

    public void SendEvents(string eventName, params object[] data)
    {
        foreach(EventController controller in list)
        {
            controller.HandleEvent(eventName, data);
        }
    }

    public void Clear()
    {
        list.Clear();
    }
}
