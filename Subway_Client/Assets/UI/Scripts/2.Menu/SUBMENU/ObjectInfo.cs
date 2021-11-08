using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ObjectInfo : SubLeftPanel
{
    public override void OnClick(string value)
    {
        base.OnClick(value);

        var list = UIProxy.Instance.UIManager.infoList;

        for (int i = 0; i < list.Count; i++)
        {
            Utility.AddText(GetParentObj(), list[i]);
        }
    }

}