using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoordPanel : MonoBehaviour
{
    public static bool isActive = false;
    
    public void PanelAnim(bool flag)
    {
        if (flag)
        {
            UIProxy.Instance.UIManager.Coordinate_Panel.GetComponent<Animator>().Play("Panel In");
            isActive = true;
        }
        else
        {
            UIProxy.Instance.UIManager.Coordinate_Panel.GetComponent<Animator>().Play("Panel Out");
            isActive = false;
        }
    }
}