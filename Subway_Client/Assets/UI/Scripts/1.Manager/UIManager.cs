using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    //INFO
    [HideInInspector]
    public List<string> infoList;


    [Header("StartAnim")]
    [Space(24)]
    public Animator Main_Panel_Animator;
    
    [Header("PanelObject")]
    [Header("LeftUI")]
    [Space(10)]
    //VerticalLayoutGroup 게임오브젝트로 지정해야 함.
    public GameObject Top_Panel;
    public GameObject Sub_Left_Panel;
    public GameObject Sub_Right_Panel;

    [Header("Main")]
    [Space(10)]
    public Camera MainCamera;
    public GameObject Main_Canvas;
    
    [Header("Coordinate")]
    [Space(10)]
    public CoordPanel Coordinate_Panel;
    public Text Coordinate_Text;

    [Header("CheckPanel")]
    [Space(10)]
    public CheckPanel Check_Panel;
    public Button OK_Btn;
    public Button Cancel_Btn;

    [Header("PopUp")]
    [Space(10)]
    public PopUpPanel PopUp_Panel;

    private void Start()
    {
        Start_MainPanel();
    }

    void Start_MainPanel()
    {
        Main_Panel_Animator.gameObject.SetActive(true);
        Main_Panel_Animator.Play("Panel Start");
    }

    #region 왼쪽 탑패널 리스트 생성

    //오브젝트 정보
    public void Click_Info()
    {
        var objectinfo = new ObjectInfoMenu();
        objectinfo.OnClick();
    }

    public void Click_ObjectList()
    {
        var menu = new ObjectListMenu();
        menu.OnClick();
    }

    public void Click_Monitoring()
    {
        var menu = new MonitoringMenu();
        menu.OnClick();
    }

    public void Click_SettingMenu()
    {
        var menu = new SettingMenu();
        menu.OnClick();
    }

    #endregion

    public void Close_SubLeftMenu()
    {
        Utility.OnOff_Panel(Sub_Left_Panel.GetComponent<SubLeftPanel>(), false);
    }

    public void Close_SubRightMenu()
    {
        Utility.OnOff_Panel(Sub_Right_Panel.GetComponent<SubRightPanel>(), false);
    }

    #region 팝업메뉴

    public void OnClick_PopUp()
    {
        PopUp_Panel.OnClick();
    }

    #endregion

    #region 마우스 좌표 표시

    public void Click_Coordinate()
    {
        if (CoordPanel.isActive)
        {
            Utility.OnOff_Panel(Coordinate_Panel, false);
            StopCoroutine(MousePosition());
            return;
        }

        StartCoroutine(MousePosition());

        Utility.OnOff_Panel(Coordinate_Panel, true);
    }

    IEnumerator MousePosition()
    {
        bool isClick = false;

        while (true)
        {
            yield return null;

            if (Input.GetMouseButtonDown(2))
            {
                isClick = !isClick;
            }

            if (!isClick)
            {
                RaycastHit hit;
                Ray ray = MainCamera.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit, float.MaxValue, 1 << LayerMask.NameToLayer("background")))
                {
                    if (hit.transform != null)
                    {
                        Coordinate_Text.text = (hit.point).ToString();
                    }
                }
            }
        }
    }
    #endregion

}

