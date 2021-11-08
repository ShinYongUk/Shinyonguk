using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class StationFactory : Factory<StationFactory>
{
    public override void CreateObject()
    {
        string selectdata = DBManager.Instance.SelectQuery("StationSelect");
        var selectjson = JSON.Parse(selectdata);

        JSONNode select = selectjson["data"]["StationSelect"];

        IEnumerable<JSONNode> e = select.Children;
        IEnumerator<JSONNode> b = e.GetEnumerator();

        while (b.MoveNext())
        {
            StationInfo stationInfo = new StationInfo();

            stationInfo.stationcode = b.Current["STATIONCODE"];
            stationInfo.x = b.Current["X"].AsFloat;
            stationInfo.y = b.Current["Y"].AsFloat;
            stationInfo.z = b.Current["Z"].AsFloat;
            stationInfo.width = b.Current["WIDTH"].AsFloat;
            stationInfo.height = b.Current["HEIGHT"].AsFloat;
            stationInfo.length = b.Current["LENGTH"].AsFloat;
            stationInfo.rotation = b.Current["ROTATION"].AsFloat;
            stationInfo.guestpermin = b.Current["GUESTPERMIN"].AsInt;
            stationInfo.guestmax = b.Current["GUESTMAX"].AsInt;
            stationInfo.line = b.Current["LINE"];
            stationInfo.yard = b.Current["YARD"];
            stationInfo.nextstation = b.Current["NEXTSTATION"];
            stationInfo.prevstation = b.Current["PREVSTATION"];

            CreateStation(stationInfo);
        }

    }

    public bool CreateStation(StationInfo stationInfo)
    {
        GameObject stationObj = null;
        stationObj = Instantiate(Resources.Load("Station") as GameObject);
        Station station = stationObj.AddComponent(Type.GetType("Station")) as Station;

        station.stationInfo = stationInfo;
        stationObj.name = stationInfo.stationcode;

        Utility.ResetTransform(stationObj);

        stationObj.transform.localScale = new Vector3(stationInfo.width, stationInfo.length, stationInfo.height);
        stationObj.transform.localPosition = new Vector3(stationInfo.x, stationInfo.y, stationInfo.z);
        stationObj.transform.localRotation = Quaternion.Euler(0, stationInfo.rotation, 0);
        stationObj.transform.parent = Instance.transform;

        if (StationManager.DicStation.ContainsKey(stationInfo.stationcode))
        {
            Debug.Log("이미 생성된 역 코드:" + stationInfo.stationcode);
            return false;
        }

        Debug.Log("역 생성:" + stationInfo.stationcode);
        StationManager.DicStation.Add(stationInfo.stationcode, station);
        StationManager.IsYardDIc.Add(stationInfo.stationcode, stationInfo.yard == "Y" ? true : false);
        

        return true;
    }

}