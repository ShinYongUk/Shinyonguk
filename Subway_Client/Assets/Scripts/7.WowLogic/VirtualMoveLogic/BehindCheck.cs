using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class BehindCheck : WowLogicDecorator<Subway, BehindCheck>
{
    public BehindCheck(IWowLogicComponent component, Subway car) : base(component, car)
    {
    }

    protected override bool CheckLocalLogic(Vector3 v3)
    {
        if (WowObject.next_subway == null)
            return false;

        float distance = (WowObject.next_subway.transform.position - v3).sqrMagnitude;

        if (distance < 800f)
        {
            ErrLog = "뒷차 겹침 오류" + v3;
        }

        return false;
    }
}