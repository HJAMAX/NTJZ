using ExitGames.Client.Photon;
using UnityEngine;
using Common;

public class SyncShootEvent : BaseEvent
{
    private DefenderController defenderController;

    public override void Start()
    {
        base.Start();
        defenderController = GetComponent<DefenderController>();
    }

    public override void OnEvent(EventData eventData)
    {
        string vec3Str = (string)MapTool.GetValue(eventData.Parameters, (byte)ParameterCode.Position);
        Vector3Data vec3 = XmlUtil.Deserialize<Vector3Data>(vec3Str);
        defenderController.Shoot(new Vector2(vec3.x, vec3.y));
    }
}
