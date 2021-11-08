using SimpleJSON;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class SubwayModelFactory : Factory<SubwayModelFactory>
{
    Dictionary<string, SubwayModelInfo> DicSubModel = new Dictionary<string, SubwayModelInfo>();

    public override void CreateObject()
    {
        string selectdata = DBManager.Instance.SelectQuery("SubwayModelSelect");
        var selectjson = JSON.Parse(selectdata);

        JSONNode select = selectjson["data"]["SubwayModelSelect"];

        IEnumerable<JSONNode> e = select.Children;
        IEnumerator<JSONNode> b = e.GetEnumerator();

        while (b.MoveNext())
        {
            SubwayModelInfo subwayModelInfo = new SubwayModelInfo();

            subwayModelInfo.subwaymodelcode = b.Current["SUBWAYMODELCODE"];
            subwayModelInfo.width = b.Current["WIDTH"].AsFloat;
            subwayModelInfo.height = b.Current["HEIGHT"].AsFloat;
            subwayModelInfo.length = b.Current["LENGTH"].AsFloat;

            DicSubModel.Add(subwayModelInfo.subwaymodelcode, subwayModelInfo);
        }
    }

    /// <summary>
    /// 모델명 받아서 차량 생성(오버로딩)
    /// </summary>
    /// <param name="subwayType"></param>
    /// <returns></returns>
    public void GetModelObject(string subwayType, ref GameObject _obj)
    {
        //디비에서 차종별 정보 받아와서 차량 만들기
        _obj = GameObject.Instantiate(Resources.Load("Subway") as GameObject);

        SubwayModelInfo subwayModelInfo = new SubwayModelInfo();
        subwayModelInfo = DicSubModel[subwayType];
        _obj.transform.localScale = new Vector3(subwayModelInfo.width, subwayModelInfo.height, subwayModelInfo.length);
    }


}

