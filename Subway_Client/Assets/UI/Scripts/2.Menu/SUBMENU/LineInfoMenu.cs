using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class LineInfoMenu : SubLeftPanel
{
    public override void OnClick(string value)
    {
        base.OnClick(value);

        if (isActive)
        {
            foreach (var item in LineManager.DicLineStat)
            {
                Utility.AddText(this.GetParentObj(), item.Key + " LineStat :" + item.Value);
            }
        }
    }
}
