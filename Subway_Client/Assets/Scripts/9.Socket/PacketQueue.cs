using System;
using System.Collections.Generic;
using System.IO;

public class PacketQueue
{

    // 패킷 저장 정보.
    struct PacketInfo
    {
        public int offset;
        public int size;
    };

    private MemoryStream streamBuffer;

    private List<PacketInfo> offsetList;

    private int offset = 0;


    private Object lockObj = new Object();

    public PacketQueue()
    {
        streamBuffer = new MemoryStream();
        offsetList = new List<PacketInfo>();
    }

    public int Enqueue(byte[] data, int size)
    {
        PacketInfo info = new PacketInfo();

        lock (lockObj)
        {
            info.offset = offset;
            info.size = size;

            // 패킷 저장 정보를 보존.
            offsetList.Add(info);

            // 패킷 데이터를 보존.
            streamBuffer.Position = offset;
            streamBuffer.Write(data, 0, size);
            streamBuffer.Flush();

            offset += size;
        }

        return size;
    }

    public int Dequeue(ref byte[] buffer, int size)
    {
        // send 100
        if (offsetList.Count <= 0)
        {
            return -1;
        }

        int recvSize = 0;
        lock (lockObj)
        {
            PacketInfo info = offsetList[0];

            // 버퍼로부터 해당하는 패킷 데이터를 가져옵니다.
            int dataSize = Math.Max(size, info.size);
            streamBuffer.Position = info.offset;

            if (buffer.Length < dataSize)
            {
                buffer = new byte[dataSize];

            }

            recvSize = streamBuffer.Read(buffer, 0, dataSize);

            offsetList.RemoveAt(0);

            // 모든 큐 데이터를 꺼냈을 때는 스트림을 클리어해서 메모리를 절약합니다.
            if (offsetList.Count == 0)
            {
                Clear();
                offset = 0;
            }
        }


        return recvSize;
    }

    public void Clear()
    {
        byte[] buffer = streamBuffer.GetBuffer();
        Array.Clear(buffer, 0, buffer.Length);
        streamBuffer.Position = 0;
        streamBuffer.SetLength(0);
    }

    public int getOffsetListCount()
    {
        return offsetList.Count;
    }

}

