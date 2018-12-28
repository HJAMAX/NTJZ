using System;
using UnityEngine;

/// <summary>
/// 转入景点场景时收到的其他玩家的信息
/// </summary>
[Serializable]
public struct PlayerData : IData
{
    public string user_id;

    public string nicheng;

    public string sex;

    public int goldCoins;

    public float wine_coins;

    public int yuanbao;

    /// <summary>
    /// 游戏会员等级
    /// </summary>
    public int game_leaguer_rank;

    /// <summary>
    /// 直接邀请到的朋友数量
    /// </summary>
    public int invite_member;

    /// <summary>
    /// 间接邀请到的朋友数量
    /// </summary>
    public int indirect_member;

    //public string[] Bullets { get { return StringUtil.StringToArray(bullets, ","); } }
}

[Serializable]
public struct UpgradeData : IData
{
    public int application_rank;

    public int check_type;

    public int is_pay;
}

[Serializable]
public struct ItemsData : IData
{
    public string id;

    public string uid;

    public int arrow;

    public int bullet;

    public int cannonball;

    public int rocket;

    public int missile;

    public int characters;

    public int weapons;

    public bool HasWeapon(int pos)
    {
        int level = weapons >> pos;
        return level % 2 == 1;
    }
}

/// <summary>
/// 转入酒庄场景时收到选中玩家的酒庄的信息
/// </summary>
[Serializable]
public struct ChateauData : IData
{
    private static string[] mapNameArray = 
        {"卧佛神泉", "大悲咒", "岭南第一窖", "龟仙洞",
         "南台遗址", "母子石", "元帅石", "相思洞", };

    public string uid;

    public string nicheng;

    public int level;

    public string wineStartTime;

    public string wineStopTime;

    public string wineStored;

    public string wineLeftPer;

    public string totalWineStored;

    public string total_wine_stealed;

    public string total_wine_lost;

    public int map_id;

    public string MapName { get { return mapNameArray[map_id]; } }
}

/// <summary>
/// 大地图上的酒庄数据
/// </summary>
public struct ChateauInMap
{
    public string id;

    public int level;

    /// <summary>
    /// 是否还能偷酒，低于50%就不行
    /// </summary>
    public bool isAvailable;

    /// <summary>
    /// 在大地图上的位置
    /// </summary>
    public Vector3 position;

    /// <summary>
    /// 被打败挂海盗旗
    /// </summary>
    public bool isDefeated;

    public string startTime;

    public string stopTime;

    public string wineStored;

    public string wineLeftPer;

    public string nicheng;
    
    /// <summary>
    /// 成功抵御进攻，挂南台旗
    /// </summary>
    public bool won;

    public ChateauInMap(string n, string i, int l, Vector3 p,
                        string start, string stop,  string ws, string wlp)
    {
        nicheng = n;
        id = i;
        level = l;
        position = p;
        won = isAvailable = isDefeated = false;

        startTime = start;
        stopTime = stop;
        wineStored = ws;
        wineLeftPer = wlp;
    }
}

[Serializable]
public struct MailData
{
    public uint mail_id;

    public short mail_type;

    public uint sender_id;

    public string sender_nicheng;

    public uint receiver_id;

    public string receiver_nicheng;

    public string title;

    public string content;

    /// <summary>
    /// 附件名
    /// </summary>
    public string item_name;

    /// <summary>
    /// 附件数量
    /// </summary>
    public int item_num;

    public string expire_time;

    public string deliver_time;

    /// <summary>
    /// 手否一度
    /// </summary>
    public bool is_checked;
}