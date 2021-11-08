using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class SensitivityObject : SubLeftPanel
{
    public override void OnClick(string key)
    {
        base.OnClick(key);
        Utility.AddSlider(buttonType.SENSITIVITY, this.GetParentObj(), "Scroll Speed", "Scroll Speed", UIProxy.Instance.UIManager.MainCamera.GetComponent<CameraOperate>().scrollSpeed, 0, 5, (float value) => { UIProxy.Instance.UIManager.MainCamera.GetComponent<CameraOperate>().scrollSpeed = value; });
        Utility.AddSlider(buttonType.SENSITIVITY, this.GetParentObj(), "Rotate X Speed", "Rotate X Speed", UIProxy.Instance.UIManager.MainCamera.GetComponent<CameraOperate>().rotateXSpeed, 0, 3, (float value) => { UIProxy.Instance.UIManager.MainCamera.GetComponent<CameraOperate>().rotateXSpeed = value; });
        Utility.AddSlider(buttonType.SENSITIVITY, this.GetParentObj(), "Rotate Y Speed", "Rotate Y Speed", UIProxy.Instance.UIManager.MainCamera.GetComponent<CameraOperate>().rotateYSpeed, 0, 4, (float value) => { UIProxy.Instance.UIManager.MainCamera.GetComponent<CameraOperate>().rotateYSpeed = value; });
        Utility.AddSlider(buttonType.SENSITIVITY, this.GetParentObj(), "Move Speed", "Move Speed", UIProxy.Instance.UIManager.MainCamera.GetComponent<CameraOperate>().moveSpeed, 0, 7, (float value) => { UIProxy.Instance.UIManager.MainCamera.GetComponent<CameraOperate>().moveSpeed = value; });
    }
}

