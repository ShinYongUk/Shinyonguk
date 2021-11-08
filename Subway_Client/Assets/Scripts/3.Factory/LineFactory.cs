using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class LineFactory : Factory<LineFactory>
{
    public override void CreateObject()
    {
        string selectdata = DBManager.Instance.SelectQuery("LineSelect");
        var selectjson = JSON.Parse(selectdata);

        JSONNode select = selectjson["data"]["LineSelect"];

        IEnumerable<JSONNode> e = select.Children;
        IEnumerator<JSONNode> b = e.GetEnumerator();

        while (b.MoveNext())
        {
            string linecd = b.Current["LINECODE"];
            bool active = b.Current["ACTIVE"].Equals("Y") ? true : false;

            LineManager.DicLineStat.Add(linecd, active);
        }
    }

}