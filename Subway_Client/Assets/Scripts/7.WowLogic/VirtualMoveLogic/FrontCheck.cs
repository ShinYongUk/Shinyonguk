using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class FrontCheck : WowLogicDecorator<Subway, FrontCheck>
{
    public FrontCheck(IWowLogicComponent component, Subway car) : base(component, car)
    {
    }

    protected override bool CheckLocalLogic(Vector3 v3)
    {
        if (WowObject.prev_subway == null)
            return false;

        float distance = (WowObject.prev_subway.transform.position - v3).sqrMagnitude;

        if (distance < 400f)
        {
            ErrLog = "앞차 겹침 오류" + v3;
            return true;
        }

        return false;
    }
}