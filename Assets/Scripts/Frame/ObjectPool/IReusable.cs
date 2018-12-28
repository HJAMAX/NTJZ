using System;
using System.Collections.Generic;

public interface IReusable
{
    /// <summary>
    /// 取出时调用
    /// </summary>
    void OnSpawn();

    /// <summary>
    /// 回收时调用
    /// </summary>
    void OnUnspawn();
}
