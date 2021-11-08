using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class RenderingObject : SubLeftPanel
{
    Camera camera = UIProxy.Instance.UIManager.MainCamera;
    GameObject[] buttons;
    bool isOn = true;

    public override void OnClick(string value)
    {
        base.OnClick(value);

        //전체 ON/OFF
        Utility.AddButton(buttonType.DEFAULT, this.GetParentObj(), "All Objcet", "All Objcet", () => { Click_AllObject(); });

        buttons = new GameObject[32];

        for (int i = 0; i < 32; i++)
        {
            int count = i;

            if (!string.IsNullOrEmpty(LayerMask.LayerToName(i)))
            {
                buttons[i] = Utility.AddButton(buttonType.DEFAULT, 
                    this.GetParentObj(), 
                    LayerMask.LayerToName(i), 
                    LayerMask.LayerToName(i) + ", Status: " + ((camera.cullingMask & 1 << i) > 0 ? "ON" : "OFF"), 
                    () => { camera.cullingMask ^= 1 << count; 
                        buttons[count].GetComponentInChildren<TextMeshProUGUI>().text = LayerMask.LayerToName(count) + ", Status: " + ((camera.cullingMask & 1 << count) > 0 ? "ON" : "OFF"); 
                    });
            }

        }
    }

    void Click_AllObject()
    {
        if (isOn)
            camera.cullingMask = -1;
        else
            camera.cullingMask = 0;
        isOn = !isOn;

        for (int i = 0; i < buttons.Length; i++)
        {
            if (string.IsNullOrEmpty(LayerMask.LayerToName(i))) continue;
            
            buttons[i].GetComponentInChildren<TextMeshProUGUI>().text = LayerMask.LayerToName(i) + ", Status: " + ((camera.cullingMask & 1 << i) > 0 ? "ON" : "OFF");
        }
    }

}