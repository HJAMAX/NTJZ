
public class UIGameButton : UIButton
{
    public void SetEnabled(bool enabled)
    {
        isEnabled = enabled;
    }

    public void Click()
    {
        EventDelegate.Execute(onClick);
    }
}
