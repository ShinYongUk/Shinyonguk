using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using SimpleJSON;
using System.Net.Sockets;
using System.Net;

public class UnitySocket
{
    public string id = null;
    public string connType = null;
    public string type = null;
    public string ip = null;
    public string port = null;
    protected Thread thread = null;
    protected object obj = null;

    /// <summary>
    /// 해당 소켓(클라이언트)을 통해 전달받은 메세지 전송 처리
    /// 각 소켓 타입에 맞게 해당 클래스에서 구현
    /// </summary>
    /// <param name="msg"></param>
    public virtual void Send(MsgData msg) { }

    /// <summary>
    /// 해당 소켓(서버 또는 클라이언트)을 통해 수신하여 수신 큐에 저장된 메세지를 리턴
    /// </summary>
    /// <returns></returns>
    public virtual MsgData Receive() { return null; }

    public virtual void DisconnectThread() { }

    public virtual void Disconnect() { }

    public virtual void Run() { }
}


/// <summary>
/// VCCSocketIO
/// 소켓, bzMethod, Message 매핑 정보로 소켓 수신, 메소드 호출에 따른 절차 정의
/// </summary>
public class SocketIO
{
    public string msgId = string.Empty;
    public string bzMethod = string.Empty;

    public SocketIO(string msgid, string bzmethod)
    {
        this.msgId = msgid;
        this.bzMethod = bzmethod;
    }
}

/// <summary>
/// VCCSocketManager
/// </summary>
public class SocketManager
{
    private static SocketManager instance = null;
    private List<SocketIO> socketIOList = null;
    private Dictionary<string, UnitySocket> socketDic = null;

    private SocketManager()
    {
        CreateSocket();
        CreateSocketIO();
    }

    public static SocketManager Instance()
    {
        if (instance == null)
        {
            instance = new SocketManager();
        }
        return instance;
    }

    public void ResetThread()
    {
        for (int i = 0; i < socketDic.Count; i++)
        {
            socketDic.ElementAt(i).Value.DisconnectThread();
        }
    }

    public void Reset()
    {
        instance = null;
    }

    private void CreateSocket()
    {
        socketDic = new Dictionary<string, UnitySocket>();

        UnitySocket socket = null;

        socket = new UnityUDPServer();

        //수정 필요
        socket.id = XmlLoader.Instance.XmlLoad("ID");
        socket.ip = XmlLoader.Instance.XmlLoad("IP");
        socket.port = XmlLoader.Instance.XmlLoad("PORT");

        socketDic.Add(socket.id, socket);
    }

    public void Disconnect()
    {
        for (int i = 0; i < socketDic.Count; i++)
        {
            socketDic.ElementAt(i).Value.Disconnect();
        }
    }

    /// <summary>
    /// 소켓, bzMethod, Message 매핑 정보로 소켓 수신, 메소드 호출에 따른 절차 정의
    /// Key 가 없는 데이터이므로 리스트에 저장
    /// </summary>
    private void CreateSocketIO()
    {
        socketIOList = new List<SocketIO>();

        socketIOList.Add(new SocketIO("SUB1", "CreateSubway"));
        socketIOList.Add(new SocketIO("SUB2", "RemoveSubway"));
        socketIOList.Add(new SocketIO("SUB3", "ModifySubway"));
        socketIOList.Add(new SocketIO("STA1", "CreateStation"));
        socketIOList.Add(new SocketIO("STA2", "RemoveStation"));
        socketIOList.Add(new SocketIO("STA3", "ModifyStation"));
        socketIOList.Add(new SocketIO("LIN1", "LineStart"));
        socketIOList.Add(new SocketIO("LIN2", "LineStop"));
    }

    public void Run()
    {
        UnitySocket vccSock = null;

        for (int i = 0; i < socketDic.Count; i++)
        {
            vccSock = socketDic.ElementAt(i).Value;
            vccSock.Run();
        }

    }

    public void Receive()
    {
        {
            for (int i = 0; i < socketDic.Count; i++)
            {
                var socket = socketDic.ElementAt(i);

                MsgData msg = socket.Value.Receive();

                while (msg != null)
                {
                    List<SocketIO> sioList = socketIOList.FindAll(c => c.msgId.Equals(msg.msgId) && !string.IsNullOrEmpty(c.bzMethod));
                    foreach (SocketIO sio in sioList)
                    {
                        object obj = MainController.Instance.ExecMethod(sio.bzMethod, msg);
                    }
                    sioList.Clear();
                    msg = socket.Value.Receive();
                }
            }
        }
    }

}



/// <summary>
/// 기존 UDPServer 를 소켓으로 갖는 클래스
/// Send : 전달 받은 메시지를 송신 큐에 저장
/// Receive : 수신 큐로 부터 메세지를 꺼내 리턴
/// </summary>
public class UnityUDPServer : UnitySocket
{
    private UDPServer socket = null;

    public UnityUDPServer()
    {
        connType = "SERVER";
        type = "UDP";
    }

    public override void Run()
    {
        socket = new UDPServer(int.Parse(port));
        socket.StartServer();
        thread = new Thread(new ThreadStart(ThreadRun));
        thread.Start();
    }

    private void ThreadRun()
    {
        // UDPServer 의 송신큐 데이터를 송신, 데이터를 수신하여 수신큐 저장
        socket.Dispatch();
    }

    /// <summary>
    /// UDPServer 수신큐에서 수신데이터를 꺼내서 MsgData 로 변환하여 리턴
    /// </summary>
    /// <returns></returns>
    public override MsgData Receive()
    {
        MsgData msgData = null;

        byte[] buffer = new byte[1];
        int recvSize = socket.Receive(ref buffer, buffer.Length);

        if (recvSize > 0)
        {
            msgData = new MsgData();
            msgData.SetMsgData(buffer);
        }
        return msgData;
    }

    public override void Disconnect()
    {
        socket.StopServer();
    }
}



public class UDPServer
{
    private Socket m_socket = null;
    private Thread m_thread = null;
    private bool m_isStarted = false;
    // 서버. 
    private bool m_isServer = false;
    // 접속. 
    private bool m_isConnected = false;
    // 접속할 주소 정보.
    private IPEndPoint m_remoteEndPoint;
    // 송신 버퍼.
    private PacketQueue m_sendQueue;
    // 수신 버퍼.
    private PacketQueue m_recvQueue;
    // 송수신용 패킷의 최대 크기.
    private const int m_packetSize = 1400;
    // 타임아웃 시간.
    private const int m_timeOutSec = 5;
    private DateTime m_ticker;
    private int port;

    // Use this for initialization
    public UDPServer(int port)
    {
        this.port = port;

        // 송수신 버퍼를 작성합니다.
        m_sendQueue = new PacketQueue();
        m_recvQueue = new PacketQueue();
    }

    public bool StartServer()
    {
        // 리스닝 소켓을 생성합니다.
        try
        {
            if (m_socket == null)
            {
                m_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            }
            m_socket.Bind(new IPEndPoint(IPAddress.Any, port));

            m_isServer = true;
        }
        catch (Exception e)
        {
            m_isServer = false;
        }

        return m_isServer;
    }

    public void StopServer()
    {
        m_isStarted = false;
        if (m_thread != null)
        {
            m_thread.Join();
            m_thread = null;
        }

        Disconnect();

        if (m_socket != null)
        {
            m_socket.Close();
            m_socket = null;
        }

        m_isServer = false;
        m_isStarted = false;

    }


    public bool Disconnect()
    {
        if (m_socket != null)
        {
            // 소켓 닫기.
            m_socket.Close();
            m_socket = null;
        }

        m_isStarted = false;
        m_isConnected = false;

        return true;
    }

    public int Send(byte[] data, int size)
    {
        return m_sendQueue.Enqueue(data, size);
    }

    public int Receive(ref byte[] buffer, int size)
    {
        return m_recvQueue.Dequeue(ref buffer, size);
    }

    public void Dispatch()
    {
        m_isStarted = true;

        while (m_isStarted == true)
        {
            // 클라이언트의 접속을 기다립니다.
            AcceptClient();

            // 클라이언트와의 송수신을 처리합니다..
            if (m_socket != null)
            {
                // 송신 처리.
                DispatchSend();

                // 수신 처리.
                DispatchReceive();

                // 타임아웃 처리.
                CheckTimeout();
            }

            Thread.Sleep(3);
        }
    }

    void AcceptClient()
    {
        if (m_isConnected == false &&
            m_socket != null &&
            m_socket.Poll(0, SelectMode.SelectRead))
        {
            // 클라이언트로부터 접속되었습니다.
            m_isConnected = true;
            // 통신 시작 시각을 기록.
            m_ticker = DateTime.Now;
        }
    }

    void DispatchSend()
    {
        if (m_socket == null)
        {
            return;
        }

        try
        {
            if (m_socket.Poll(0, SelectMode.SelectWrite))
            {
                byte[] buffer = new byte[m_packetSize];

                int sendSize = m_sendQueue.Dequeue(ref buffer, buffer.Length);
                while (sendSize > 0)
                {
                    m_socket.Send(buffer, sendSize, SocketFlags.None);
                    sendSize = m_sendQueue.Dequeue(ref buffer, buffer.Length);
                }
            }
        }
        catch
        {
            return;
        }
    }

    void DispatchReceive()
    {
        if (m_socket == null)
        {
            return;
        }

        try
        {
            while (m_socket.Poll(0, SelectMode.SelectRead))
            {
                byte[] buffer = new byte[m_packetSize];

                int recvSize = m_socket.Receive(buffer, buffer.Length, SocketFlags.None);

                if (recvSize == 0)
                {
                    // 끊기.
                    Disconnect();
                }
                else if (recvSize > 0)
                {
                    m_recvQueue.Enqueue(buffer, recvSize);
                    // 수신 시각을 갱신.
                    m_ticker = DateTime.Now;
                }
            }
        }
        catch
        {
            return;
        }
    }

    void CheckTimeout()
    {
        TimeSpan ts = DateTime.Now - m_ticker;

        if (m_isConnected && ts.Seconds > m_timeOutSec)
        {
            m_isConnected = false;

        }
    }

    public bool IsConnected
    {
        get
        {
            return m_isConnected;
        }

        set
        {
            m_isConnected = value;
        }
    }


    /// <summary>
    /// 리시브 큐 카운트 가져오기
    /// </summary>
    /// <returns></returns>
    public int getRecvQueueCount()
    {
        return m_recvQueue.getOffsetListCount();
    }
}
