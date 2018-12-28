using UnityEngine;
using System.Collections.Generic;
using Common;

public class BattleManager : MonoBehaviour
{
    [SerializeField]
    private DefenderController[] defenders;

    private DefenderController activeDefender;
    
    public DefenderController ActiveDefender { get { return activeDefender; } }

    [SerializeField]
    private int totalLevels;

    /// <summary>
    /// 角色模型路径
    /// </summary>
    [SerializeField]
    private string[] prefabPaths;

    /// <summary>
    /// 技能冷却
    /// </summary>
    [SerializeField]
    private SkillCD skillCD;

    [SerializeField]
    private List<GameObject> enemyList = new List<GameObject>();

    private bool isFinished = false;

    [SerializeField]
    private Vector2[] spawnPostions;

    private Dictionary<int, EnemyController> enemyMap = new Dictionary<int, EnemyController>();

    public Dictionary<int, EnemyController> EnemyMap { get { return enemyMap; } }

    private SwitchWeaponRequest switchWeaponRequest;

    void Start()
    {
        switchWeaponRequest = GetComponent<SwitchWeaponRequest>();
        switchWeaponRequest.roomId = GameData.chosenRoomId;

        Time.timeScale = 1.0f;
        InitializePlayers(GameData.playerStateDataList, GameData.defenderData);
    }

    void Update()
    {
        /*//酒庄主胜利
        if (activeDefender.isHuman && enemyMap.Count == 0)
        {
            Game.Instance.EventBus.SendEvents("battle_end", "defend_succeeded");
        }*/
    }

    public void InitializePlayers(List<PlayerStateData> playerStateDataList, DefenderData defenderData)
    {
        //初始化所有其他玩家
        foreach (PlayerStateData stateData in playerStateDataList)
        {
            if (stateData.level > 0 && stateData.id != defenderData.id)
            {
                int pathIndex = stateData.level + totalLevels * stateData.sex;
                GameObject enemy = Game.Instance.ObjectPool.Spawn(prefabPaths[pathIndex]);
                enemy.transform.position = spawnPostions[stateData.layer];

                EnemyController enemyController = enemy.GetComponent<EnemyController>();
                enemyController.networkPos = spawnPostions[stateData.layer];
                enemyController.combatPlayerData.id = stateData.id;
                enemyMap.Add(stateData.id, enemyController);

                if (GameData.playerStateData != null && stateData.id == GameData.playerStateData.id && 
                    defenderData.id != GameData.playerStateData.id)
                {
                    enemyController.SetLocal(spawnPostions[stateData.layer]);
                }

                enemyController.SetNicknameHud(stateData.nicheng);
            }
        }

        //初始化防御者
        defenders[0].gameObject.SetActive(true);
        activeDefender = defenders[defenderData.sex];

        //
        bool isAi = defenderData.hostId == GameData.playerStateData.id;
        bool isHuman = defenderData.id == GameData.playerStateData.id;

        //
        EnemyController[] enemyList = new EnemyController[enemyMap.Count];
        enemyMap.Values.CopyTo(enemyList, 0);
        activeDefender.Init(enemyList, isAi, isHuman, defenderData.id, defenderData.nicheng);
        activeDefender.ChangeWeapon(defenderData.curWeaponIndex);
    }

    /// <summary>
    /// 改武器并发送改武器请求
    /// </summary>
    /// <param name="weaponIndexStr"></param>
    public void ChangeWeapon(string weaponIndexStr)
    {
        int weaponIndex = int.Parse(weaponIndexStr);

        if (skillCD.SwitchWeaponCD(weaponIndex))
        {
            activeDefender.ChangeWeapon(weaponIndex);
            switchWeaponRequest.weaponIndex = weaponIndex;
            switchWeaponRequest.DefaultRequest();
        }
    }

    /// <summary>
    /// 非host非人为操作改武器
    /// </summary>
    /// <param name="weaponIndex"></param>
    public void AutoChangeWeapon(int weaponIndex)
    {
        if (!activeDefender.isHuman)
        {
            activeDefender.ChangeWeapon(weaponIndex);
        }
    }
}
