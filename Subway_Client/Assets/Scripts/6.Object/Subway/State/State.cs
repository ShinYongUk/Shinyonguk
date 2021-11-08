using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State {

    protected Subway subway;

    public State(Subway subway)
    {
        this.subway = subway;
        Init();
    }

    // 상태 초기화
    public virtual void Init()
    {

    }

    // 이동
    public virtual void Move()
    {
        subway.Resume();
    }

    // 정지
    public virtual void Pause()
    {
        subway.Pause();
    }

    /// <summary>
    /// 좌표 이동
    /// </summary>
    public virtual void PointToDestination(Vector3 v3)
    {
        subway.PointToDestination(v3);
    }
    
}
