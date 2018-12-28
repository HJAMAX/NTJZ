using System;
using System.Reflection;

[Serializable]
public struct ShopData : IData
{
    public string arch;

    public string gun;

    public string cannon;

    public string rocket_gun;

    public string missile_launcher;

    public string arrow;

    public string bullet;

    public string cannonball;
    
    public string rocket;
        
    public string missile;

    public string lv1;

    public string lv2;

    public string lv3;

    public string lv4;

    public string lv5;

    public object GetField(string fieldName)
    {
        FieldInfo fieldInfo = typeof(ShopData).GetField(fieldName);
        return fieldInfo != null ? fieldInfo.GetValue(this) : null;
    }
}

[Serializable]
public struct WechatPayData : IData
{
    public string appid;

    public string partnerid;

    public string prepayid;

    public string noncestr;

    public string timestamp;

    public string sign;
}