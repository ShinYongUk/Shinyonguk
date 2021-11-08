using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class SubwayListMenu : SubLeftPanel
{
    public override void OnClick(string value)
    {
        base.OnClick(value);

        if (isActive)
        {
            foreach (var key in SubwayManager.subwayCollection.Dictionary)
            {
                Utility.AddButton(buttonType.DEFAULT, this.GetParentObj(), key.Key, key.Key, () => Click_LogicListCar(key.Key));
            }
        }
    }


    void Click_LogicListCar(string key)
    {
        var subBottomPanel = new SubRightPanel();
        subBottomPanel.OnClick(key);
        
        SubwayManager.subwayCollection.Get(key).moveLogicObj.coordComponent.CreateSettingBtn();
    }
}
