using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class ObjectInfoMenu : TopPanel
{
    public override void OnClick()
    {
        base.OnClick();

        if (isActive)
        {
            var objectInfoPanel = new ObjectInfo();
            objectInfoPanel.OnClick("ObjectInfo");
        }
    }

}
