using UnityEngine;
using System;

public class UIUpdateGameData : MonoBehaviour
{
    public void Click(UIGameFieldInfo gameFieldInfo)
    {
        //把字符串转换成相应的类型
        Type t = Type.GetType(gameFieldInfo.fieldType);
        //把值转换成相应类型的值
        object changedValue = Convert.ChangeType(gameFieldInfo.value, t);
        GameData.SetField(gameFieldInfo.fieldName, changedValue);
    }
}
