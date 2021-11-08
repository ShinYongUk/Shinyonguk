using UnityEngine;

public class WowObject
{
    #region 전역변수
    public bool isError = false;
    
    public IWowLogicComponent coordComponent;
    #endregion

    /// <summary>
    /// 좌표로 이동
    /// </summary>
    public virtual void CheckCoordLogic(Vector3 v3)
    {
        isError = coordComponent.CheckCoordinate(v3);
    }
    
}
