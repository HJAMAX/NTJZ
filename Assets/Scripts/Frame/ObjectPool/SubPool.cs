using System.Collections.Generic;
using UnityEngine;

public class SubPool
{
    /// <summary>
    /// 当池子中的对象数量达到这个数值时，读取关卡的时候会清除一些对象
    /// </summary>
    private int _clearPoint;

    private GameObject _prefab;

    private List<GameObject> _objects = new List<GameObject>();

    public string Name { get { return _prefab.name; } }

    public SubPool(GameObject prefab)
    {
        _prefab = prefab;
    }

    /// <summary>
    /// 取对象
    /// </summary>
    public GameObject Spawn(int clearPoint)
    {
        _clearPoint = clearPoint;
        GameObject gameObj = null;

        foreach(GameObject obj in _objects)
        {
            if(!obj.activeSelf)
            {
                gameObj = obj;
                break;
            }
        }

        if(gameObj == null)
        {
            gameObj = GameObject.Instantiate<GameObject>(_prefab);
            _objects.Add(gameObj);
        }
        
        gameObj.SetActive(true);
        gameObj.SendMessage("OnSpawn", SendMessageOptions.DontRequireReceiver);
        return gameObj;
    }

    /// <summary>
    /// 回收对象
    /// </summary>
    /// <param name="gameObj"></param>
    public void Unspawn(GameObject gameObj)
    {
        if(Contains(gameObj))
        {
            
            gameObj.SendMessage("OnUnspawn", SendMessageOptions.DontRequireReceiver);
            gameObj.SetActive(false);
        }
    }
    
    /// <summary>
    /// 回收所有对象
    /// </summary>
    public void UnpawnAll()
    {
        if(_objects.Count > 0 && _objects[0] == null)
        {
            _objects.Clear();
            return;
        }

        foreach(GameObject item in _objects)
        {
            if(item.activeSelf)
            {
                Unspawn(item);
            }
        }
    }

    public void Clear()
    {
        while (_objects.Count > _clearPoint)
        {
            GameObject obj = _objects[0];
            _objects.RemoveAt(0);
            Object.Destroy(obj);
        }
    }

    /// <summary>
    /// 是否包含对象
    /// </summary>
    /// <param name="gameObj"></param>
    /// <returns></returns>
    public bool Contains(GameObject gameObj)
    {
        return _objects.Contains(gameObj);
    }
}
