using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SubwayManager : Manager<SubwayManager>
{
    //(연결리스트 + DICTIONARY)
    public static SubwayPairCollection subwayCollection = new SubwayPairCollection();

    public MsgData CreateSubway(MsgData msg)
    {
        SubwayInfo subwayInfo = new SubwayInfo();
        string[] temp = msg.body.Split(',');

        Debug.Log("열차 생성:" + subwayInfo.subwaycode);

        subwayInfo.subwaycode = temp[0];
        subwayInfo.startstation = temp[1];
        subwayInfo.line = temp[2];
        subwayInfo.type = temp[3];
        subwayInfo.speed = float.Parse(temp[4]);
        subwayInfo.maxguestcount = int.Parse(temp[5]);

        if (!SubwayFactory.Instance.CreateSubway(subwayInfo))
        {
            Debug.Log("열차 생성 실패:" + subwayInfo.subwaycode);
        }

        return null;
    }


    public void RemoveSubway(MsgData msg)
    {
        string[] temp = msg.body.Split(',');

        Subway subway = subwayCollection.Get(temp[0]);
        Debug.Log("열차 삭제:" + subway.SubwayInfo.subwaycode);
        subwayCollection.DeleteMid(subway);

        subway.SubwayState = new RemoveState(subway);

        SubwayObjectPool.Instance.SetObject(subway.gameObject);
    }


    public void ModifySubway(MsgData msg)
    {
        string[] temp = msg.body.Split(',');

        Subway subway = subwayCollection.Get(temp[0]);
        Debug.Log("열차 수정:" + subway.SubwayInfo.subwaycode);

        subwayCollection.DeleteMid(subway);
        subway.SubwayState = new RemoveState(subway);
        SubwayObjectPool.Instance.SetObject(subway.gameObject);

        SubwayInfo subwayInfo = new SubwayInfo();

        subwayInfo.subwaycode = temp[0];
        subwayInfo.startstation = temp[1];
        subwayInfo.line = temp[2];
        subwayInfo.type = temp[3];
        subwayInfo.speed = float.Parse(temp[4]);
        subwayInfo.maxguestcount = int.Parse(temp[5]);

        if (SubwayFactory.Instance.CreateSubway(subwayInfo))
        {
            Debug.Log("열차 생성 실패:" + subwayInfo.subwaycode);
        }

    }
}