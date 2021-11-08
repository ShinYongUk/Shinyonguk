using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class CameraOnOff : SubLeftPanel
{
    public override void OnClick(string value)
    {
        base.OnClick(value);

        if (isActive)
        {
            Utility.AddButton(buttonType.DEFAULT, this.GetParentObj(), "Camera ON", "Camera ON", () => Click_CameraOnOff(true),true);
            Utility.AddButton(buttonType.DEFAULT, this.GetParentObj(), "Camera OFF", "Camera OFF", () => Click_CameraOnOff(false),true);
        }
    }

    public void Click_CameraOnOff(bool isOn)
    {
        UIProxy.Instance.UIManager.MainCamera.gameObject.SetActive(isOn);
    }

}