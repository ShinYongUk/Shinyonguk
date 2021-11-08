using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class SettingMenu : TopPanel
{
    public override void OnClick()
    {
        base.OnClick();

        if (isActive)
        {
            Utility.AddButton(buttonType.MENU, this.GetParentObj(), "Rendering Object", "Rendering Object", () =>
            {
                var renderpanel = new RenderingObject();
                renderpanel.OnClick("Rendering Object");
            });

            Utility.AddButton(buttonType.MENU, this.GetParentObj(), "Sensitivity", "Sensitivity", () =>
            {
                var renderpanel = new SensitivityObject();
                renderpanel.OnClick("Sensitivity");
            });
        }
    }
}