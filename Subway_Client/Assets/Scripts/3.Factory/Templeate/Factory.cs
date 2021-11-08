using UnityEngine;

public class Factory<T> : MonoSingleton<T>, IFactory where T: MonoBehaviour
{
    public virtual void CreateObject()
    {
    }

    public virtual void DeleteObject()
    {
    }

    public virtual void UpdateObject()
    {
    }
}

