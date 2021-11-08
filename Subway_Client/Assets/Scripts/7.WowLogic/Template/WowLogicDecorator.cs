using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class WowLogicDecorator<T,U> : IWowLogicComponent where T: MonoBehaviour
{
    protected IWowLogicComponent component;
    protected T WowObject;
    //flag
    public static bool isGlobalflag = true;
    public bool isLocalflag = true;
    public bool isError = false;

    //ErrLog
    protected string ErrLog = string.Empty;

    //Cache
    static GameObject SubRightPanel_obj = UIProxy.Instance.UIManager.Sub_Right_Panel.transform.GetComponentInChildren<VerticalLayoutGroup>().gameObject;

    public WowLogicDecorator(IWowLogicComponent coordComponent, T wowObject)
    {
        component = coordComponent;
        WowObject = wowObject;
    }

    public bool CheckCoordinate(Vector3 v3)
    {
        if (isGlobalflag && isLocalflag)
        {
            isError = CheckLocalLogic(v3);
            if (isError) Debug.Log(ErrLog);
            return component.CheckCoordinate(v3) || isError;
        }
        else
        {
            return component.CheckCoordinate(v3);
        }
    }

    protected abstract bool CheckLocalLogic(Vector3 v3);

    #region UI 버튼 이벤트 관련
    public void CreateSettingBtn()
    {
        component.CreateSettingBtn();
        Utility.AddButton(buttonType.DEFAULT, SubRightPanel_obj, this.GetType().ToString()+"Local", "Local: "+ this.GetType().ToString() + "Flag : " + isLocalflag, ChangeLocalFlag, true);
        Utility.AddButton(buttonType.DEFAULT, SubRightPanel_obj, this.GetType().ToString()+"Global", "Global: " + this.GetType().ToString() + "Flag : " + isGlobalflag, ChangeGlobalFlag, true);
    }
    
    public void CreateErrSettingBtn()
    {
        component.CreateErrSettingBtn();
        if (isError)
        {
            Utility.AddButton(buttonType.DEFAULT, SubRightPanel_obj, this.GetType().ToString()+"ErrSet", this.GetType().ToString(), CreateErrContentBtn);
        }
    }
    
    void ChangeLocalFlag()
    {
        isLocalflag = !isLocalflag;
        SubRightPanel_obj.transform.Find(this.GetType().ToString()+"Local").GetComponentInChildren<TextMeshProUGUI>().text = "Local: " + this.GetType().ToString() + "Flag : " + isLocalflag;

    }

    void ChangeGlobalFlag()
    {
        isGlobalflag = !isGlobalflag;
        SubRightPanel_obj.transform.Find(this.GetType().ToString()+"Global").GetComponentInChildren<TextMeshProUGUI>().text = "Global: " + this.GetType().ToString() + "Flag : " + isGlobalflag;
    }

    void CreateErrContentBtn()
    {
        Utility.SetChildUIObjects(SubRightPanel_obj.GetComponent<RectTransform>());

        Utility.AddButton(buttonType.DEFAULT, SubRightPanel_obj, this.GetType().ToString()+"Local", "Local: " + this.GetType().ToString() + "Flag : " + isLocalflag, ChangeLocalFlag, true);
        Utility.AddText(SubRightPanel_obj, ErrLog);
    }
    
    void Empty()
    {

    }
    #endregion
}
