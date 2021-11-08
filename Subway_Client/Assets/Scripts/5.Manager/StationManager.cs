using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class StationManager : Manager<StationManager>
{
    //key - 스테이션코드
    public static Dictionary<string, Station> DicStation = new Dictionary<string, Station>();
    //key - 스테이션코드, value - true,false (열차기지인지 판단)
    public static Dictionary<string, bool> IsYardDIc = new Dictionary<string, bool>();

    public void CreateStation(MsgData msg)
    {
        StationInfo stationInfo = new StationInfo();
        string[] temp = msg.body.Split(',');

        stationInfo.stationcode = temp[0];
        stationInfo.line = temp[1];
        stationInfo.x = float.Parse(temp[2]);
        stationInfo.y = float.Parse(temp[3]);
        stationInfo.z = float.Parse(temp[4]);
        stationInfo.width = float.Parse(temp[5]);
        stationInfo.height = float.Parse(temp[6]);
        stationInfo.length = float.Parse(temp[7]);
        stationInfo.rotation = float.Parse(temp[8]);

        stationInfo.guestpermin = int.Parse(temp[9]);
        stationInfo.guestmax = int.Parse(temp[10]);

        stationInfo.yard = temp[11];
        stationInfo.nextstation = temp[12];
        stationInfo.prevstation = temp[13];

        StationFactory.Instance.CreateStation(stationInfo);
    }

    public void RemoveStation(MsgData msg)
    {
        StationInfo stationInfo = new StationInfo();
        string[] temp = msg.body.Split(',');


        Station station = DicStation[temp[0]];
        Debug.Log("역 삭제:" + station.stationInfo.stationcode);
        DicStation.Remove(temp[0]);
        IsYardDIc.Remove(temp[0]);

        GameObject.Destroy(station.gameObject);
    }

    public void ModifyStation(MsgData msg)
    {

        StationInfo stationInfo = new StationInfo();
        string[] temp = msg.body.Split(',');

        stationInfo.stationcode = temp[0];
        stationInfo.line = temp[1];
        stationInfo.x = float.Parse(temp[2]);
        stationInfo.y = float.Parse(temp[3]);
        stationInfo.z = float.Parse(temp[4]);
        stationInfo.width = float.Parse(temp[5]);
        stationInfo.height = float.Parse(temp[6]);
        stationInfo.length = float.Parse(temp[7]);
        stationInfo.rotation = float.Parse(temp[8]);

        stationInfo.guestpermin = int.Parse(temp[9]);
        stationInfo.guestmax = int.Parse(temp[10]);

        stationInfo.yard = temp[11];
        stationInfo.nextstation = temp[12];
        stationInfo.prevstation = temp[13];

        Station station = DicStation[temp[0]];
        Debug.Log("역 삭제:" + station.stationInfo.stationcode);
        DicStation.Remove(temp[0]);
        IsYardDIc.Remove(temp[0]);
        GameObject.Destroy(station.gameObject);

        StationFactory.Instance.CreateStation(stationInfo);

    }
}
