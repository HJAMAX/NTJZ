/// <summary>
/// 非立即注册的EventController
/// </summary>
public class OnEnableEventController : EventController
{
    void OnEnable()
    {
        if (Game.Instance)
        {
            Game.Instance.EventBus.RegisterController(this);
        }
    }
}
