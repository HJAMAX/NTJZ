using System.Collections.Generic;
using UnityEngine;

public class ObjPool : MonoBehaviour
{
    [SerializeField]
    private int _clearPoint;

    public string ResourceDir = "";

    Dictionary<string, SubPool> _poolMap = new Dictionary<string, SubPool>();

    public GameObject Spawn(string name)
    {
        if(!_poolMap.ContainsKey(name))
        {
            RegisterNew(name);
        }
        return _poolMap[name].Spawn(_clearPoint);
    }

    /// <summary>
    /// 回收对象
    /// </summary>
    /// <param name="gameObj"></param>
    public void Unspawn(GameObject gameObj)
    {
        SubPool pool = null;

        foreach(SubPool p in _poolMap.Values)
        {
            if(p.Contains(gameObj))
            {
                pool = p;
                break;
            }
        }

        pool.Unspawn(gameObj);
    }

    /// <summary>
    /// 回收所有
    /// </summary>
    public void UnspawnAll()
    {
        foreach (SubPool p in _poolMap.Values)
        {
            p.UnpawnAll();
        }
    }

    public void ClearAll()
    {
        foreach (SubPool p in _poolMap.Values)
        {
            p.Clear();
        }
    }

    /// <summary>
    /// 产生新对象池
    /// </summary>
    /// <param name="name"></param>
    private void RegisterNew(string name)
    {
        string path = string.IsNullOrEmpty(ResourceDir) ? name : ResourceDir + "/" + name;
        GameObject prefab = Resources.Load<GameObject>(path);

        SubPool pool = new SubPool(prefab);
        _poolMap.Add(prefab.name, pool);
    }
}
