using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class InitManager : MonoBehaviour
{
    bool isQuit = false;

    void Awake()
    {
        Application.wantsToQuit += () => { Utility.CheckOneMore("종료", ExecQuit); return isQuit; };
        StationFactory.Instance.CreateObject();
        LineFactory.Instance.CreateObject();
        SubwayModelFactory.Instance.CreateObject();
        SubwayFactory.Instance.CreateObject();

        MainController.Instance.Initailize();
    }

    public void ExecQuit()
    {
        isQuit = true;
        Logger.Instance.CloseFile();
        Application.Quit();
    }


}