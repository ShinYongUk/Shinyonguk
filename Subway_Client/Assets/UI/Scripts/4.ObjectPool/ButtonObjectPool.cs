using UnityEngine;
using UnityEngine.UI;

public class ButtonObjectPool : ObjectPool<ButtonObjectPool>
{
    /// <summary>
    ///
    /// </summary>
    public override void SetObject(GameObject obj)
    {
        obj.name = obj.GetComponent<UnityButton>().buttonType.ToString();

        base.SetObject(obj);
    }

}
