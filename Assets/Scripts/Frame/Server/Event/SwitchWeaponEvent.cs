using ExitGames.Client.Photon;
using Common;

public class SwitchWeaponEvent : BaseEvent
{
    public override void OnEvent(EventData eventData)
    {
        int weaponIndex = (int)MapTool.GetValue(eventData.Parameters, (byte)ParameterCode.WeaponIndex);
        GetComponent<BattleManager>().AutoChangeWeapon(weaponIndex);
    }
}
