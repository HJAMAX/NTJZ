using UnityEngine;

public class BulletsAmountEventHandler : EventHandler
{
    [SerializeField]
    private UILabel arrowAmount;

    [SerializeField]
    private UILabel bulletAmount;

    [SerializeField]
    private UILabel cannonballAmount;

    [SerializeField]
    private UILabel rocketAmount;

    [SerializeField]
    private UILabel missileAmount;

    public override void HandleEvent(params object[] data)
    {
        ItemsData itemsData = GameData.itemsData;

        arrowAmount.text = itemsData.arrow.ToString();
        bulletAmount.text = itemsData.bullet.ToString();
        cannonballAmount.text = itemsData.cannonball.ToString();
        rocketAmount.text = itemsData.rocket.ToString();
        missileAmount.text = itemsData.missile.ToString();
    }
}
