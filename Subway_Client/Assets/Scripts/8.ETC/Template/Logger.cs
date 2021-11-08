using System;
using System.IO;
using UnityEngine;

public class Logger : Singleton<Logger>
{
    string dataPath = Application.dataPath;
    StreamWriter mFile = null;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="_log"></param>
    public void WriteLog(string _log, logType _type = logType.None)
    {
        string dt = DateTime.Now.ToString("yyyyMMdd");
        string path = dataPath + "/" + dt;
        switch (_type)
        {
            case logType.TcpIP:
                {
                    path += "." + logType.TcpIP.ToString();
                }
                break;
        }

        path += ".txt";

        OpenFile(path, _log);
    }

    /// <summary>
    /// 
    /// </summary>
    private void OpenFile(string path, string log)
    {
        mFile = new StreamWriter(path, true);
        mFile.WriteLine(DateTime.Now.ToString("yyyy/MM/dd/HH:mm:ss") + ": " + log);
        mFile.Flush();
        mFile.Close();
        mFile = null;
    }

    /// <summary>
    /// 
    /// </summary>
    public void CloseFile()
    {
        if (mFile != null)
        {
            mFile.Close();
            mFile = null;
        }
    }

}




