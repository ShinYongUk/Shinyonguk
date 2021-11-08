using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SubLeftPanel : MonoBehaviour
{
    private static GameObject parentObj;
    public static bool isActive = false;
    public static string ActivePanel = string.Empty;

    public virtual void OnClick(string key)
    {
        UIProxy.Instance.UIManager.Close_SubRightMenu();

        if (ActivePanel.Equals(key))
        {
            Utility.OnOff_Panel(this, false);
        }
        else
        {
            ActivePanel = key;
            Utility.OnOff_Panel(this, true);
            Utility.SetChildUIObjects(this.GetParentObj().GetComponent<RectTransform>());
        }
    }

    public void PanelAnim(bool flag)
    {
        if (flag)
        {
            UIProxy.Instance.UIManager.Sub_Left_Panel.GetComponent<Animator>().Play("Panel In");
            isActive = true;
        }
        else
        {
            UIProxy.Instance.UIManager.Sub_Left_Panel.GetComponent<Animator>().Play("Panel Out");
            isActive = false;
            ActivePanel = string.Empty;
        }
    }

    public GameObject GetParentObj()
    {
        if (parentObj == null)
        {
            parentObj = UIProxy.Instance.UIManager.Sub_Left_Panel.transform.GetComponentInChildren<VerticalLayoutGroup>().gameObject;
        }
        return parentObj;
    }
}