using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplineMove : MonoBehaviour
{
    bool isMove = false;

    public float speed;

    public WowObject vitualObject;

    public List<Vector3> DestinationList = new List<Vector3>();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="wowObject"></param>
    public void Initialize(WowObject wowObject) {
        vitualObject = wowObject;
    }


    /// <summary>
    /// Starts movement. Can be called from other scripts to allow start delay.
    /// <summary>
    public void SetDestination(Vector3 destination)
    {
        DestinationList.Add(destination);
    }

    public void ChangeSpeed(float value)
    {
        speed = value;
    }

    public void Resume()
    {
        isMove = true;
    }

    public void Pause()
    {
        isMove = false;
    }

    /// <summary>
    /// 
    /// </summary>
    void FixedUpdate()
    {
        if (isMove)
        {
            if(DestinationList.Count == 0)
            {
                return;
            }

            vitualObject.CheckCoordLogic(transform.position);

            if (vitualObject.isError)
            {
                return;
            }

            if((transform.position - DestinationList[0]).sqrMagnitude < 0.4)
            {
                DestinationList.RemoveAt(0);
            }

            transform.LookAt(DestinationList[0]);

            transform.localPosition += transform.forward * speed * Construct.FIXED_RATE;
        }
    }

}
