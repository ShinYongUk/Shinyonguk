using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataBaseConnect;

public class DBManager : MonoSingleton<DBManager>
{
    private DBConnect conn = null;
    private bool isExit = true;

    // Start is called before the first frame update
    void Awake()
    {
        conn = new DBConnect();
        DontDestroyOnLoad(this.gameObject);
    }

    private bool Connect()
    {
        bool result = true;

        if (isExit == true)
        {
            result = conn.Connect();

            if (result == true)
            {
                isExit = false;
                Debug.Log("연결");
            }
            else
            {
                Debug.Log("연결실패");
            }
        }
        return result;
    }

    private bool Clear()
    {
        bool result = true;

        if (isExit == false)
        {
            result = conn.Disconnect();

            if (result == true)
            {
                isExit = true;
                Debug.Log("연결종료");
            }
            else
            {
                Debug.Log("연결종료실패");
            }
        }
        return result;
    }

    public string SelectQuery(string queryID)
    {
        string temp = string.Empty;

        if (Connect())
        {
            temp = conn.SelectQuery(queryID);

            Clear();
        }
        return temp;
    }

}
