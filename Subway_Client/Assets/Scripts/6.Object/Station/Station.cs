using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Station : MonoBehaviour, IPopObject
{
    Queue<Guest> guestQueue = new Queue<Guest>();
    public StationInfo stationInfo = new StationInfo();
    public Vector3 inner;
    public Vector3 outer;

    int currentGuestCount {
        get
        {
            return guestQueue.Count;
        }
    }

    public string Destination
    {
        get
        {
            var list = StationManager.DicStation.OrderBy(g => Guid.NewGuid());
            var ordereditem = list.Take(2);

            foreach (var temp in ordereditem)
            {
                var item = temp.Value;

                if (item.stationInfo.stationcode.Equals(this.stationInfo.stationcode))
                    continue;

                return item.stationInfo.stationcode;
            }
            return string.Empty;
        }
    }

    void Start()
    {
        StartCoroutine("ComeGuest");
        StartCoroutine("CheckGuest");
        inner = transform.Find("INNER").position;
        outer = transform.Find("OUTER").position;
    }

    IEnumerator ComeGuest()
    {
        while (true)
        {
            for (int i = 0; i < stationInfo.guestpermin; i++)
            {
                guestQueue.Enqueue(new Guest() { destination = Destination });
            }
            yield return new WaitForSeconds(60f);
        }
    } 


    IEnumerator CheckGuest()
    {
        while (true)
        {
            if(stationInfo.guestmax < currentGuestCount)
            {
                onPopUp();
            }
            yield return new WaitForSeconds(30f);
        }
    }


    void OnTriggerEnter(Collider coll)
    {
        StationIn(coll);
    }

    void OnTriggerExit(Collider coll)
    {
        StationOut(coll);
    }

    protected virtual void StationIn(Collider other)
    {
        string layer = LayerMask.LayerToName(other.gameObject.layer);

        if (layer.Equals("Subway"))
        {
            Subway subway = other.transform.GetComponent<Subway>();

            if (subway != null)
            {
                subway.SubwayState = new PauseState(subway, 15f);
                subway.SubwayState.Pause();
                subway.SubwayState.PointToDestination(StationManager.DicStation[stationInfo.nextstation].inner);
                subway.SubwayState.PointToDestination(StationManager.DicStation[stationInfo.nextstation].outer);

                SubwayInfo subwayinfo = subway.SubwayInfo;

                //손님 하차
                subway.RemoveGuest(this.stationInfo.stationcode);

                //손님 탑승
                int count = 0;

                for (int i = 0; i < currentGuestCount; i++)
                {
                    if (subway.AddGuest(guestQueue.Dequeue()))
                    {
                        count++;
                    }
                    else
                    {
                        break;
                    }
                }

                Debug.Log("열차 진입 :" + subwayinfo.subwaycode + ", 역 코드 :" + this.stationInfo.stationcode + ", 손님 탑승 수 :" + count);
            }
        }
    }

    protected virtual void StationOut(Collider other)
    {
        string layer = LayerMask.LayerToName(other.gameObject.layer);

        if (layer.Equals("Subway"))
        {
            Subway subway = other.transform.GetComponent<Subway>();

            if (subway != null)
            {
                SubwayInfo info = subway.SubwayInfo.Clone() as SubwayInfo;

                Debug.Log("열차 진출 :" + info.subwaycode + ", 역 코드 :" + this.stationInfo.stationcode);
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
        Utility.AddText(UIProxy.Instance.UIManager.PopUp_Panel.GetParentObj(), stationInfo.stationcode + "역의 인원수 초과");
    }

    #endregion
}
