using System.Collections.Generic;
using System.Reflection;
using UnityEngine;


public class MainController : MonoSingleton<MainController>
{
    SocketManager socketManager;
    
    List<IManager> ManagerList = new List<IManager>();

    /// <summary>
    /// 
    /// </summary>
    public void Initailize()
    {
        ManagerList.Add(SubwayManager.Instance);
        ManagerList.Add(StationManager.Instance);
        ManagerList.Add(LineManager.Instance);

        socketManager = SocketManager.Instance();
        socketManager.Run();

    }

    /// <summary>
    /// 
    /// </summary>
    private void Update()
    {
        socketManager.Receive();
    }

    /// <summary>
    /// 프로그램 종료
    /// </summary>
    private void OnDisable()
    {
        if (socketManager != null)
        {
            socketManager.Disconnect();
            socketManager.ResetThread();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public object ExecMethod(string bzMethod, MsgData msg)
    {
        object temp = null;

        for (int i = 0; i < ManagerList.Count; i++)
        {
            ManagerList[i].ExecMethod(bzMethod, new object[] { msg });
        }

        return temp;
    }
}
