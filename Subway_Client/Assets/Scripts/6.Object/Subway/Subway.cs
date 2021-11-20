using System.Collections.Generic;
using UnityEngine;

public class Subway : MonoBehaviour, IPopObject
{
    #region 전역변수
    public Subway prev_subway = null;
    public Subway next_subway = null;
    
    public State SubwayState;
    SubwayInfo subwayInfo = new SubwayInfo();

    List<Guest> guestlist = new List<Guest>();

    float speed = 0f;

    public WowObject moveLogicObj;

    SplineMove spMove;
    #endregion

    #region 프로퍼티

    public int currentguestcount
    {
        get
        {
            return guestlist.Count;
        }
    }

    public SubwayInfo SubwayInfo
    {
        set
        {
            subwayInfo = value;
        }
        get
        {
            return subwayInfo;
        }
    }


    public string LineCD
    {
        set
        {
            subwayInfo.line = value;
        }
        get
        {
            return subwayInfo.line;
        }
    }

    public SplineMove SpMove
    {
        get
        {
            if(spMove == null)
                spMove = this.gameObject.AddComponent<SplineMove>();
            return spMove;
        }
    }

    #endregion

    public void SetProperty(SubwayInfo subwayInfo)
    {
        SubwayInfo = subwayInfo;
    }

    /// <summary>
    /// 임시 함수
    /// </summary>
    public void Initialize()
    {
        //기본 정보 셋팅
        name = subwayInfo.subwaycode;
        speed = subwayInfo.speed;

        moveLogicObj = new WowObject();
        moveLogicObj.coordComponent =  new FrontCheck(
                                       new BehindCheck(
                                       new BasicWowCheckLogic(), this),this);
        SpMove.Initialize(moveLogicObj);

        LineManager.Instance.UpdateLineStat += UpdateCarStat;
        SubwayState = new ActualState(this);
        transform.localPosition = Vector3.zero;

    }

    private void OnDestroy()
    {
        LineManager.Instance.UpdateLineStat -= UpdateCarStat;
    }
    
    public void UpdateCarStat()
    {
        Resume();
    }

    /// <summary>
    /// 목적지 설정
    /// </summary>
    public void PointToDestination(Vector3 v3)
    {
        if (this.gameObject.activeSelf == false)
        {
            gameObject.SetActive(true);
        }

        spMove.SetDestination(v3);
    }

    public void SetSpeed(float speed)
    {
        spMove.ChangeSpeed(speed);
    }

    public void Resume()
    {
        if (LineManager.DicLineStat[LineCD])
        {
            SetSpeed(speed);
            spMove.Resume();
            this.GetComponent<Animator>().SetBool("isRun", true);
        }
        else
        {
            Pause();
        }
    }

    public void Pause()
    {
        spMove.Pause();
        this.GetComponent<Animator>().SetBool("isRun", false);
    }

    public bool AddGuest(Guest guest)
    {
        if(subwayInfo.maxguestcount > currentguestcount)
        {
            guestlist.Add(guest);

            if (currentguestcount == subwayInfo.maxguestcount)
                onPopUp();

            return true;
        }
        return false;
    }

    public void RemoveGuest(string stationcode)
    {
        for (int i = 0; i < guestlist.Count; i++)
        {
            if (guestlist[i].destination.Equals(stationcode))
            {
                guestlist.RemoveAt(i);
            }
        }
    }

    #region UI

    /// <summary>
    /// 팝업창 생성
    /// </summary>
    public void onPopUp()
    {
        UIProxy.Instance.UIManager.OnClick_PopUp();
        Utility.AddText(UIProxy.Instance.UIManager.PopUp_Panel.GetParentObj(), "지하철 코드: "+subwayInfo.subwaycode + ", 손님 초과");
    }

    #endregion
}
