using System;

class PacketParser
{
    private struct PacketInfo
    {
        public int size;
        public int offset;
    }

    private int headerLength;

    private PacketInfo packet;
    private byte[] paketBuffer;
    public delegate void SuccessCallback(byte[] buffer, int size);

    public PacketParser(int length)
    {
        headerLength = length;
        // 패킷 초기화 (헤더길이 만큼 설정)
        packet = new PacketInfo();
        packet.size = headerLength;
        paketBuffer = new byte[1024];
    }

    /// <summary>
    /// 패킷 나누기
    /// </summary>
    /// <param name="buffer">원본 버퍼</param>
    /// <param name="recvSize">받은 버퍼 크기</param>
    public void Parse(byte[] buffer, int recvSize, SuccessCallback callback)
    {
        byte[] result = new byte[recvSize];
        Array.Copy(buffer, 0, result, 0, recvSize);
        callback(result, recvSize);
        Reset();
    }


    /// <summary>
    /// 원하는 크기만큼 복사한 경우에는 TRUE 아닐 경우 FALSE 리턴
    /// </summary>
    /// <param name="buffer">원본 버퍼</param>
    /// <param name="packet">패킷 정보</param>
    /// <param name="offset">패킷 위치정보</param>
    /// <param name="recvSize">받은 버퍼 크기</param>
    /// <returns></returns>
    bool CopyBuffer(ref PacketInfo packet, byte[] buffer, int recvSize, ref int offset)
    {

        // 현재 읽을 수 있는 남은 버퍼 크기
        int remainBufferSize = recvSize - offset;
        // 현재 패킷을 넣을 수 있는 크기
        int remainPacketSize = packet.size - packet.offset;

        // 남은 버퍼와 넣을 수 있는 크기 비교 후 복사 크기 설정
        int copySize = (remainBufferSize > remainPacketSize) ? remainPacketSize : remainBufferSize;

        //읽을게 없는 경우
        if (copySize < 0)
        {
            return false;
        }

        // 해당 패킷만큼 복사
        Array.Copy(buffer, offset, paketBuffer, packet.offset, copySize);
        offset += copySize;
        packet.offset += copySize;

        // 패킷이 다 차지 않은경우 리턴
        if (packet.offset < packet.size)
        {
            return false;
        }
        return true;
    }

    /// <summary>
    /// 헤더에서 바디크기 정보 가져오기
    /// </summary>
    /// <returns></returns>
    int GetBodySize()
    {
        byte[] temp = new byte[4];

        Array.Copy(paketBuffer, 5, temp, 0, 4);
        Array.Reverse(temp);

        return BitConverter.ToInt32(temp, 0);
    }

    /// <summary>
    /// 패킷 정보 및 패킷 버퍼 초기화
    /// </summary>
    public void Reset()
    {
        // 패킷 초기화 (헤더길이 만큼 설정)
        packet.offset = 0;
        packet.size = headerLength;

        Array.Clear(paketBuffer, 0, paketBuffer.Length);
    }

}