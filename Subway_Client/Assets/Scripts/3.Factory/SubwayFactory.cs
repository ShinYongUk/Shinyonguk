using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubwayFactory : Factory<SubwayFactory>
{
    public override void CreateObject()
    {
        string selectdata = DBManager.Instance.SelectQuery("SubwaySelect");
        var selectjson = JSON.Parse(selectdata);

        JSONNode select = selectjson["data"]["SubwaySelect"];

        IEnumerable<JSONNode> e = select.Children;
        IEnumerator<JSONNode> b = e.GetEnumerator();

        while (b.MoveNext())
        {
            SubwayInfo subwayInfo = new SubwayInfo();

            subwayInfo.subwaycode = b.Current["SUBWAYCODE"];
            subwayInfo.speed = b.Current["SPEED"].AsFloat;
            subwayInfo.type = b.Current["TYPE"];
            subwayInfo.line = b.Current["LINE"];
            subwayInfo.maxguestcount = b.Current["MAXGUESTCOUNT"].AsInt;
            subwayInfo.startstation = b.Current["STARTSTATION"];

            CreateSubway(subwayInfo);
        }
    }

    public bool CreateSubway(SubwayInfo subwayInfo)
    {
        if (SubwayManager.subwayCollection.Contains(subwayInfo.subwaycode))
        {
            Debug.Log("이미 생성된 열차, subwaycode : " + subwayInfo.subwaycode);
            return false;
        }

        Debug.Log("열차 생성 시작:" + subwayInfo.subwaycode);

        if (!StationManager.IsYardDIc[subwayInfo.startstation])
        {
            Debug.Log("잘못된 시작역 입력:" + subwayInfo.startstation);
            return false;
        }

        //차종
        string subwayType = subwayInfo.type;

        //오브젝트 풀 사용해서 재사용
        GameObject obj = SubwayObjectPool.Instance.GetObject(subwayType);
        //재사용할 객체가 없을 시
        if (obj == null)
        {
            SubwayModelFactory.Instance.GetModelObject(subwayType, ref obj);
        }

        Subway subway = obj.GetComponent<Subway>();
        //초기 맵핑시 ActualState로 설정
        subway.SetProperty(subwayInfo);
        subway.Initialize();

        obj.transform.parent = Instance.transform;

        SubwayManager.subwayCollection.InsertMid(subway);
        Station startstation = StationManager.DicStation[subwayInfo.startstation];
        subway.transform.localPosition = startstation.transform.localPosition;
        if (StationManager.DicStation.ContainsKey(startstation.stationInfo.nextstation))
        {
            subway.transform.rotation = startstation.transform.rotation;
            subway.SubwayState.Move();
        }
        return true;
    }
}
