using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

static class Construct{
    public static float FIXED_RATE = Time.fixedDeltaTime;
}

public class StationInfo
{
    public string stationcode;
    public float x;
    public float y;
    public float z;
    public float width;
    public float height;
    public float length;
    public float rotation;
    public int guestpermin;
    public int guestmax;
    public string line;
    public string yard;
    public string nextstation;
    public string prevstation;

    public object Clone()
    {
        return this.MemberwiseClone();
    }
}

public class SubwayInfo
{
    public string subwaycode;
    public float speed;
    public string type;
    public string line;
    public int maxguestcount; 
    public string startstation;

    public object Clone()
    {
        return this.MemberwiseClone();
    }
}

public class SubwayModelInfo
{
    public string subwaymodelcode;
    public float width;
    public float height;
    public float length;

    public object Clone()
    {
        return this.MemberwiseClone();
    }
}


public enum buttonType
{
    DEFAULT = 0,
    MENU,
    SENSITIVITY
}

public enum logType
{
    None = 0,
    TcpIP
}

public enum ByteLinecd
{
    LIN01,
    LIN02
}
