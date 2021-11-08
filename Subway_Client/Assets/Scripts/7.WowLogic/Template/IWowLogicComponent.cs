using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWowLogicComponent
{
    bool CheckCoordinate(Vector3 v3);

    void CreateSettingBtn();
    
    void CreateErrSettingBtn();
}
