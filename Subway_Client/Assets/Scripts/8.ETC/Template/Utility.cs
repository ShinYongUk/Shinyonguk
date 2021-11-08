using System;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Utility : MonoBehaviour
{

    /// <summary>
    /// 빈 오브젝트 생성
    /// </summary>
    /// <param name="name"></param>
    /// <param name="parent"></param>
    /// <returns></returns>
    static public GameObject NewGameObejct(string name, Transform parent = null)
    {
        GameObject obj = new GameObject(name);

        ResetTransform(obj);

        return obj;
    }

    /// <summary>
    /// 빈 오브젝트 생성
    /// </summary>
    /// <param name="name"></param>
    /// <param name="parent"></param>
    /// <returns></returns>
    static public GameObject ResetTransform(GameObject _gameObject)
    {
        _gameObject.transform.localScale = Vector3.one;
        _gameObject.transform.localPosition = Vector3.zero;
        _gameObject.transform.localRotation = Quaternion.identity;

        return _gameObject;
    }

    #region UI전용

    static public GameObject AddButton(buttonType buttonType, GameObject ListObj, string _name, string _text, UnityAction callback, bool isCheckForm = false)
    {
        GameObject buttonObj = ButtonObjectPool.Instance.GetObject(buttonType.ToString());
       
        if (buttonObj == null)
        {
            switch (buttonType)
            {
                case buttonType.DEFAULT:
                    buttonObj = Instantiate(Resources.Load<GameObject>("Button"));
                    break;
                case buttonType.MENU:
                    buttonObj = Instantiate(Resources.Load<GameObject>("MenuButton"));
                    break;
            }
        }

        switch (buttonType)
        {
            case buttonType.DEFAULT:
                buttonObj.GetComponent<Button>().onClick.RemoveAllListeners();
                if (isCheckForm)
                {
                    buttonObj.GetComponent<Button>().onClick.AddListener(() => { CheckOneMore(_text, () => callback()); });
                }
                else
                {
                    buttonObj.GetComponent<Button>().onClick.AddListener(() => callback());
                }
                break;
            case buttonType.MENU:
                buttonObj.GetComponent<Button>().onClick.RemoveAllListeners();

                //메뉴 버튼 그룹화
                int count = TopPanelManager.Instance.buttons.Count;
                buttonObj.GetComponent<Button>().onClick.AddListener(() => { TopPanelManager.Instance.PanelAnim(count); });
                TopPanelManager.Instance.buttons.Add(buttonObj);

                if (isCheckForm)
                {
                    buttonObj.GetComponent<Button>().onClick.AddListener(() => { CheckOneMore(_text, () => callback()); });
                }
                else
                {
                    buttonObj.GetComponent<Button>().onClick.AddListener(() => callback());
                }
                break;
        }

        buttonObj.name = _name;
        buttonObj.GetComponentInChildren<TextMeshProUGUI>().text = _text;
        buttonObj.transform.parent = ListObj.transform;
        buttonObj.transform.localScale = new Vector3(1, 1, 1);

        return buttonObj;
    }


    static public GameObject AddSlider(buttonType buttonType, GameObject ListObj, string _name, string _text, float current_value, float min_value, float max_value, UnityAction<float> callback)
    {
        GameObject buttonObj = ButtonObjectPool.Instance.GetObject(buttonType.ToString());

        if (buttonObj == null)
        {
            switch (buttonType)
            {
                case buttonType.SENSITIVITY:
                    buttonObj = Instantiate(Resources.Load<GameObject>("SliderItem"));
                    break;
            }
        }

        switch (buttonType)
        {
            case buttonType.SENSITIVITY:
                buttonObj.GetComponentInChildren<Slider>().value = current_value;
                buttonObj.GetComponentInChildren<Slider>().minValue = min_value;
                buttonObj.GetComponentInChildren<Slider>().maxValue = max_value;
                buttonObj.GetComponentInChildren<TextMeshProUGUI>().text = current_value.ToString();
                buttonObj.GetComponentInChildren<Slider>().onValueChanged.RemoveAllListeners();
                buttonObj.GetComponentInChildren<Slider>().onValueChanged.AddListener((value) => { callback(value); buttonObj.GetComponentInChildren<TextMeshProUGUI>().text = value.ToString(); });
                break;
        }

        buttonObj.name = _name;
        buttonObj.transform.Find("Title").GetComponentInChildren<TextMeshProUGUI>().text = _text;
        buttonObj.transform.parent = ListObj.transform;
        buttonObj.transform.localScale = new Vector3(1, 1, 1);

        return buttonObj;
    }

    public static void CheckOneMore(string _text, UnityAction ok_callback, UnityAction cancel_callback = null)
    {
        OnOff_Panel(UIProxy.Instance.UIManager.Check_Panel, true);

        UIProxy.Instance.UIManager.Check_Panel.GetComponentInChildren<TextMeshProUGUI>().text = _text + " ?";
        UIProxy.Instance.UIManager.OK_Btn.GetComponent<Button>().onClick.RemoveAllListeners();
        UIProxy.Instance.UIManager.Cancel_Btn.GetComponent<Button>().onClick.RemoveAllListeners();
        UIProxy.Instance.UIManager.OK_Btn.GetComponent<Button>().onClick.AddListener(ok_callback);
        UIProxy.Instance.UIManager.OK_Btn.GetComponent<Button>().onClick.AddListener(() => OnOff_Panel(UIProxy.Instance.UIManager.Check_Panel, false));
        if(cancel_callback != null)
        {
            UIProxy.Instance.UIManager.Cancel_Btn.GetComponent<Button>().onClick.AddListener(cancel_callback);
        }
        UIProxy.Instance.UIManager.Cancel_Btn.GetComponent<Button>().onClick.AddListener(() => OnOff_Panel(UIProxy.Instance.UIManager.Check_Panel, false));
    }

    static public GameObject AddText(GameObject ListObj, string _name)
    {
        GameObject textObj = TextObjectPool.Instance.GetObject(string.Empty);

        if (textObj == null)
        {
            textObj = Instantiate(Resources.Load<GameObject>("Text"));
        }

        textObj.name = _name;
        textObj.GetComponentInChildren<TextMeshProUGUI>().text = _name;
        textObj.transform.parent = ListObj.transform;
        textObj.transform.localScale = new Vector3(1, 1, 1);

        return textObj;
    }

    static public void SetChildUIObjects(RectTransform parent)
    {
        int count = parent.childCount;

        for (int i = 0; i < count; i++)
        {
            if(parent.GetChild(0).GetComponent<Button>() != null)
            {
                ButtonObjectPool.Instance.SetObject(parent.GetChild(0).gameObject);
            }
            else
            {
                TextObjectPool.Instance.SetObject(parent.GetChild(0).gameObject);
            }
        }
    }


    static public void UnSetMenuButtonGroup()
    {
        TopPanelManager.Instance.currentButtonlIndex = 0;
        TopPanelManager.Instance.buttons.Clear();
    }


    static public void OnOff_Panel(TopPanel topPanel,bool flag)
    {
        topPanel.PanelAnim(flag);
    }


    static public void OnOff_Panel(SubLeftPanel topPanel, bool flag)
    {
        topPanel.PanelAnim(flag);
    }


    static public void OnOff_Panel(SubRightPanel topPanel, bool flag)
    {
        topPanel.PanelAnim(flag);
    }

    static public void OnOff_Panel(CoordPanel topPanel, bool flag)
    {
        topPanel.PanelAnim(flag);
    }

    static public void OnOff_Panel(CheckPanel topPanel, bool flag)
    {
        topPanel.PanelAnim(flag);
    }

    static public List<string> GetFieldsToString<T>(T _target) where T : class
    {
        List<string> list = new List<string>();
        string temp = null;

        FieldInfo[] field = _target.GetType().GetFields();

        for (int i = 0; i < field.Length; i++)
        {
            temp = field[i].Name + " : " + field[i].GetValue(_target);
            list.Add(temp);
        }

        return list;
    }

    #endregion

}
