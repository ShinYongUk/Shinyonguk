using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TopPanel : MonoBehaviour
{
    private static GameObject parentObj;
    public static bool isActive = false;
    public static Type ActivePanel = null;

    public virtual void OnClick()
    {
        UIProxy.Instance.UIManager.Close_SubLeftMenu();
        UIProxy.Instance.UIManager.Close_SubRightMenu();

        if (ActivePanel == null)
        {
            Utility.OnOff_Panel(this, true);
            Utility.SetChildUIObjects(this.GetParentObj().GetComponent<RectTransform>());
            Utility.UnSetMenuButtonGroup();
            return;
        }

        if (ActivePanel.Equals(this.GetType()))
        {
            Utility.OnOff_Panel(this, false);
        }
        else
        {
            Utility.OnOff_Panel(this, true);
            Utility.SetChildUIObjects(this.GetParentObj().GetComponent<RectTransform>());
            Utility.UnSetMenuButtonGroup();
        }
    }
    
    public void PanelAnim(bool flag)
    {
        if (flag)
        {
            UIProxy.Instance.UIManager.Top_Panel.GetComponent<Animator>().Play("Panel In");
            isActive = true;
            ActivePanel = this.GetType();
        }
        else
        {
            UIProxy.Instance.UIManager.Top_Panel.GetComponent<Animator>().Play("Panel Out");
            isActive = false;
            ActivePanel = null;
        }
    }

    public GameObject GetParentObj()
    {
        if(parentObj == null)
        {
            parentObj = UIProxy.Instance.UIManager.Top_Panel.transform.GetComponentInChildren<HorizontalLayoutGroup>().gameObject;
        }
        return parentObj;
    }
}