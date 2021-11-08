using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ObjectListMenu : TopPanel
{
    public override void OnClick()
    {
        base.OnClick();

        if (isActive)
        {
            Utility.AddButton(buttonType.MENU, this.GetParentObj(), "SubwayList", "SubwayList", () => {
                var subwayList = new SubwayListMenu();
                subwayList.OnClick("SubwayList");
            });

            Utility.AddButton(buttonType.MENU, this.GetParentObj(), "ErrSubwayList", "ErrSubwayList", () => {
                var ErrSubwayList = new ErrSubwayListMenu();
                ErrSubwayList.OnClick("ErrSubwayList");
            });

        }
    }
}
