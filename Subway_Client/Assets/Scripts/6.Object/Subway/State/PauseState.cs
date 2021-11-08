using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// 대기 상태
/// </summary>
public class PauseState : State
{
    float timer = 0f;

    /// <summary>
    /// 대기 상태 초기화
    /// </summary>
    /// <param name="subway"></param>
    public PauseState(Subway subway, float timer) : base(subway) {
        this.timer = timer;
    }
    
    public override void Pause()
    {
        Timer();
    }
    
    async void Timer()
    {
        subway.Pause();
        await Task.Delay((int)timer*1000);
        subway.Resume();
    }
}
