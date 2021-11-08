using UnityEngine;

public abstract class ObjectPool<T> : MonoSingleton<T> where T : MonoBehaviour
{
    protected GameObject ObjectPoolObj = null;
    /// <summary>
    ///
    /// </summary>
    public virtual void SetObject(GameObject obj)
    {
        if (ObjectPoolObj == null)
        {
            ObjectPoolObj = Instance.gameObject;
        }

        obj.transform.parent = ObjectPoolObj.transform;
        obj.SetActive(false);
    }

    /// <summary>
    ///
    /// </summary>
    public virtual GameObject GetObject(string value = "")
    {
        if (ObjectPoolObj == null)
        {
            ObjectPoolObj = Instance.gameObject;
        }

        if (ObjectPoolObj.transform.childCount <= 0)
        {
            return null;
        }

        GameObject target = FindObjByString(value);

        if (target != null)
        {
            target.SetActive(true);
            return target;
        }

        return null;
    }

    public virtual GameObject FindObjByString(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return ObjectPoolObj.transform.GetChild(0).gameObject;
        }

        if (ObjectPoolObj.transform.Find(value) == null)
        {
            return null;
        }

        return ObjectPoolObj.transform.Find(value).gameObject;
    }

}
