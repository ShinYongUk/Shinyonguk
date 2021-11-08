using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ErrSubwayListMenu : SubLeftPanel
{
    public override void OnClick(string value)
    {
        base.OnClick(value);

        if (isActive)
        {
            foreach (var item in SubwayManager.subwayCollection.Dictionary)
            {
                if (item.Value.moveLogicObj.isError)
                {
                    Utility.AddButton(buttonType.DEFAULT, this.GetParentObj(), item.Key, item.Key, () => Click_ErrListCar(item.Key));
                }
            }
        }
    }

    void Click_ErrListCar(string key)
    {
        var subBottomPanel = new SubRightPanel();
        subBottomPanel.OnClick(key);

        SubwayManager.subwayCollection.Get(key).moveLogicObj.coordComponent.CreateErrSettingBtn();
    }
    
}
