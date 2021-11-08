using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class LineManager : Manager<LineManager>
{
    //key - 라인코드, value - 가동상태
    public static Dictionary<string, bool> DicLineStat = new Dictionary<string, bool>();

    public delegate void LineEvent();
    public event LineEvent UpdateLineStat;

    public void LineStart(MsgData msg)
    {
        string[] temp = msg.body.Split(',');

        DicLineStat[temp[0]] = temp[1] == "Y" ? true : false;

        UpdateLineStat();
    }

    public void LineStop(MsgData msg)
    {
        string[] temp = msg.body.Split(',');

        DicLineStat[temp[0]] = temp[1] == "Y" ? true : false;

        UpdateLineStat();
    }
}
