using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 삭제 상태
/// </summary>
public class RemoveState : State
{
    float timer = 0f;

    /// <summary>
    /// 초기화
    /// </summary>
    /// <param name="subway"></param>
    public RemoveState(Subway subway) : base(subway)
    {
        subway.Pause();
    }

    public override void Move()
    {
    }

    public override void Pause()
    {
    }

    /// <summary>
    /// 좌표 이동
    /// </summary>
    public override void PointToDestination(Vector3 v3)
    {
    }
}
