using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class MonitoringMenu : TopPanel
{
    public override void OnClick()
    {
        base.OnClick();

        if (isActive)
        {
            Utility.AddButton(buttonType.MENU, this.GetParentObj(), "LineInfo", "LineInfo", () => {
                var lineInfo = new LineInfoMenu();
                lineInfo.OnClick("LineInfo");
            });
        }
    }
}
