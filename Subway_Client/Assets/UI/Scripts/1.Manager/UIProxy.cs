using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIProxy : MonoSingleton<UIProxy>
{
    UIManager uIManager = null;

    public UIManager UIManager
    {
        get
        {
            if(uIManager == null)
            {
                uIManager = GameObject.Find("Manager").GetComponent<UIManager>();
            }
            return uIManager;
        }
    }
}
