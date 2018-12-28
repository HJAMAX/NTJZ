using UnityEngine;

public class ReusableObject : MonoBehaviour, IReusable
{
    public IData data;

    public virtual void OnSpawn()
    {

    }

    public virtual void OnUnspawn()
    {
        
    }

    protected virtual void Awake()
    {
        DontDestroyOnLoad(this);
    }
}
