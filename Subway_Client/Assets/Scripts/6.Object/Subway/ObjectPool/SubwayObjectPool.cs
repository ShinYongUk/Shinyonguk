using UnityEngine;

public class SubwayObjectPool : ObjectPool<SubwayObjectPool>
{
    /// <summary>
    ///
    /// </summary>
    public override void SetObject(GameObject obj)
    {
        base.SetObject(obj);

        obj.transform.localPosition = Vector3.zero;
        obj.transform.eulerAngles = Vector3.zero;
    }

}
