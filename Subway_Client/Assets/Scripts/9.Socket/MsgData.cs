using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
public class MsgData
{
	//헤더 4 + 4 (전체길이 + msgid)
	public static int HEADER_LENGTH = 4;
	// 20181213, fansung, packetsize 추가
	public static int PACKET_SIZE = 1400;
	public int totallength;
	public string msgId;
	public string body;

	public void SetMsgData(byte[] data)
	{
		byte[] bytesHeader = new byte[4];
		byte[] bytesBody = new byte[data.Length - MsgData.HEADER_LENGTH];

		for (int i = 0; i < MsgData.HEADER_LENGTH; i++)
		{
			bytesHeader[i] = data[i];
		}

		msgId = Encoding.Default.GetString(bytesHeader);


		for (int i = 0; i < bytesBody.Length; i++)
		{
			bytesBody[i] = data[MsgData.HEADER_LENGTH + i];
		}

		body = Encoding.Default.GetString(bytesBody);
	}
}

