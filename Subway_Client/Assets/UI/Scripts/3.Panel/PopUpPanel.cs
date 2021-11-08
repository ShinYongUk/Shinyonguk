using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUpPanel : MonoBehaviour
{
    private static GameObject parentObj;
    public static bool isActive = false;

    public void OnClick()
    {
        Utility.SetChildUIObjects(this.GetParentObj().GetComponent<RectTransform>());
        PanelAnim(false);
        PanelAnim(true);
    }

    public void PanelAnim(bool flag)
    {
        if (flag)
        {
            UIProxy.Instance.UIManager.PopUp_Panel.GetComponent<Animator>().Play("Panel In");
            isActive = true;
        }
        else
        {
            UIProxy.Instance.UIManager.PopUp_Panel.GetComponent<Animator>().Play("Panel Out");
            isActive = false;
        }
    }

    public GameObject GetParentObj()
    {
        if (parentObj == null)
        {
            parentObj = UIProxy.Instance.UIManager.PopUp_Panel.transform.GetComponentInChildren<VerticalLayoutGroup>().gameObject;
        }
        return parentObj;
    }
}