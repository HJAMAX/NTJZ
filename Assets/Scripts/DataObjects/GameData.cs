using System;
using UnityEngine;
using System.Reflection;
using System.Collections.Generic;
using Common;

public static class GameData
{
    #region 场景跳转相关
    /// <summary>
    /// 跳转时，下个场景的index
    /// </summary>
    public static int nextSceneIndex = 2;

    /// <summary>
    /// 登录界面场景序号
    /// </summary>
    public static int startIndex = 2;

    /// <summary>
    /// 当前玩家酒庄场景序号
    /// </summary>
    public static int myChateauIndex = 3;

    /// <summary>
    /// 大地图场景序号
    /// </summary>
    public static int mapIndex = 4;

    /// <summary>
    /// 景点地图场景序号
    /// </summary>
    public static int sceneIndex = 5;

    /// <summary>
    /// 其他玩家酒庄战斗场景序号
    /// </summary>
    public static int battleIndex = 6;

    /// <summary>
    /// 返回初始页的跳转码
    /// 0 为登出或刚登入
    /// 1 为战斗中途断线
    /// 2 为战斗场景外的其它场景断线
    /// </summary>
    public static int returnStartCode = 0;
    #endregion

    #region 玩家相关信息
    /// <summary>
    /// 当前玩家信息
    /// </summary>
    public static PlayerData playerData;

    /// <summary>
    /// 申请升级的信息
    /// </summary>
    public static UpgradeData upgradeData;

    /// <summary>
    /// 开战前本机玩家的初始状态数据
    /// </summary>
    public static PlayerStateData playerStateData;

    /// <summary>
    /// 玩家拥有的道具信息
    /// </summary>
    public static ItemsData itemsData;
    #endregion

    #region 酿酒相关信息
    /// <summary>
    /// 酿酒结束后，须等X秒钟才能取酒
    /// </summary>
    public static int secWaitWine = 600;

    /// <summary>
    /// 酿酒所需时间
    /// </summary>
    public static int timeMakeWine = 3600;

    /// <summary>
    /// 场景地图里，酿酒进度条每秒减少数值
    /// </summary>
    public static float progress = 0.00008f;

    /// <summary>
    /// 每个酿酒器储存的总量
    /// </summary>
    public static int wineStoredEach = 100;

    /// <summary>
    /// 最少用于酿酒的金币数量
    /// </summary>
    public static int leastCoinMakeWine = 3;
    #endregion

    #region 战斗相关信息

    public static int chosenRoomId;

    public static int maxBattleInitCount = 5;

    public static bool isDefender = false;

    /// <summary>
    /// 下场战斗的人数
    /// </summary>
    public static int playerCount = 0;

    /// <summary>
    /// 下一场战斗开始时间
    /// </summary>
    public static long nextBattleStartTime;

    /// <summary>
    /// 下场战斗中参与战斗守方玩家的信息
    /// </summary>
    public static DefenderData defenderData;

    /// <summary>
    /// 这次被偷的酒量
    /// </summary>
    public static float wineStolen;

    /// <summary>
    /// 下场战斗中参与战斗攻方玩家的信息
    /// </summary>
    public static List<PlayerStateData> playerStateDataList;
    #endregion

    #region 大地图中的信息
    /// <summary>
    /// 玩家选择的景点地图编号
    /// </summary>
    public static int chosenMapId = 0;

    /// <summary>
    /// 玩家上次选择的景点地图编号
    /// </summary>
    public static int lastChosenMapId = 0;

    /// <summary>
    /// 大地图中点击酒庄，进入酒庄场景前，保存拥有该酒庄的玩家ID
    /// </summary>
    public static string chosenChateauId;

    /// <summary>
    /// 保存拥有该酒庄
    /// </summary>
    public static ChateauController chosenChateauCtrl;

    /// <summary>
    /// 保存拥有该酒庄的性别
    /// </summary>
    public static string chosenPlayerSex;

    /// <summary>
    /// 酒庄能够被偷酒的百分比
    /// </summary>
    public static int wineLeftPerCanBeStolen = 60;

    /// <summary>
    /// 保存酒庄在地图的位置和信息
    /// </summary>
    public static Dictionary<string, ChateauInMap> chateausInMap;
    #endregion

    /// <summary>
    /// 商店物品信息
    /// </summary>
    public static ShopData shopData;

    /// <summary>
    /// 玩家信息，须在游戏开始时赋值
    /// </summary>
    public static ChateauData playerChateauData;

    public static object GetField(string fieldName)
    {
        Type[] types = new Type[] { typeof(GameData), typeof(ItemsData), typeof(ChateauData), typeof(PlayerData)};
        object[] objs = new object[] { null, itemsData, playerChateauData, playerData};

        for (int i = 0; i < types.Length; i++)
        {
            FieldInfo fieldInfo = types[i].GetField(fieldName);
            if (fieldInfo != null)
                return fieldInfo.GetValue(objs[i]);
        }

        return null;
    }

    public static void SetField(string fieldName, object value)
    {
        FieldInfo fieldInfo = typeof(GameData).GetField(fieldName);
        if(fieldInfo != null)
        {
            fieldInfo.SetValue(null, value);
        }
    }

    public static object GetPerperty(string propertyName)
    {
        Type[] types = new Type[] { typeof(ChateauData)};
        object[] objs = new object[] { playerChateauData };

        for (int i = 0; i < types.Length; i++)
        {
            PropertyInfo propertyInfo = types[i].GetProperty(propertyName);
            if (propertyInfo != null)
                return propertyInfo.GetValue(objs[i], null);
        }

        return null;
    }
}

/// <summary>
/// 
/// </summary>
[Serializable]
public struct PostData
{
    public string name;

    public string field;

    public PostData(string name, string field)
    {
        this.name = name;
        this.field = field;
    }
}

[Serializable]
public struct PostBinaryData
{
    public string name;

    public byte[] field;

    public PostBinaryData(string name, byte[] field)
    {
        this.name = name;
        this.field = field;
    }
}


public interface IData
{

}

[Serializable]
public class JsonWrapperArray<T>
{
    public string state;

    public string code;

    public string msg;

    public T[] items;

    private static Dictionary<string, string> dataMap = new Dictionary<string, string>();

    public static JsonWrapperArray<T> FromJson<T>(string json)
    {
        return JsonUtility.FromJson<JsonWrapperArray<T>>(json);
    }

    public static string ToJson<T>(T[] array, bool prettyPoint)
    {
        JsonWrapperArray<T> wrapper = new JsonWrapperArray<T>();
        wrapper.items = array;
        return JsonUtility.ToJson(wrapper, prettyPoint);
    }

    public static string ToJson<T>(T[] array)
    {
        JsonWrapperArray<T> wrapper = new JsonWrapperArray<T>();
        wrapper.items = array;
        return JsonUtility.ToJson(wrapper);
    }

    public static string ToJson<T>(T data)
    {
        return JsonUtility.ToJson(data);
    }

    public static PostData[] FromJsonStr(string json)
    {
        string[] dataStrList = StringUtil.StringToArray(json, "@");
        PostData[] postDataList = new PostData[dataStrList.Length];

        for (int i = 0; i < dataStrList.Length; i++)
        {
            string[] postDataStr = StringUtil.StringToArray(dataStrList[i], ":");
            PostData postData = new PostData(postDataStr[0], postDataStr[1]);

            postDataList[i] = postData;
        }

        return postDataList;
    }

    public static Dictionary<string, string> FromJsonMap(string json)
    {
        dataMap.Clear();
        string[] dataStrList = StringUtil.StringToArray(json, "@");

        for (int i = 0; i < dataStrList.Length; i++)
        {
            string[] postDataStr = StringUtil.StringToArray(dataStrList[i], ":");
            dataMap[postDataStr[0]] = postDataStr[1];
        }

        return dataMap;
    }
}