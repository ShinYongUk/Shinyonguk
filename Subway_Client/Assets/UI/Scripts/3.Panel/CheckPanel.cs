using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckPanel : MonoBehaviour
{
    public static bool isActive = false;

    public void PanelAnim(bool flag)
    {
        if (flag)
        {
            UIProxy.Instance.UIManager.Check_Panel.GetComponent<Animator>().Play("Panel In");
            isActive = true;
        }
        else
        {
            UIProxy.Instance.UIManager.Check_Panel.GetComponent<Animator>().Play("Panel Out");
            isActive = false;
        }
    }
}