using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Dictionary와 List를 합친 컬렉션, Key 와 Index로 조회 가능
/// </summary>
/// <typeparam name="string">Key</typeparam>
/// <typeparam name="TValue">Value</typeparam>
public class SubwayPairCollection
{
	private Dictionary<string, Subway> dictionary;
	public Subway HEAD;
	public Subway END;

	public SubwayPairCollection()
	{
		dictionary = new Dictionary<string, Subway>();
		HEAD = null;
		END = null;
	}

	public bool IsFirst(Subway subway)
    {
		if(subway == HEAD)
        {
			return true;
        }
		return false;
    }

	public bool IsEnd(Subway subway)
	{
		if (subway == END)
		{
			return true;
		}
		return false;
	}

	public void InsertHead(Subway subway)
	{
        if (!dictionary.ContainsKey(subway.SubwayInfo.subwaycode))
        {
			dictionary.Add(subway.SubwayInfo.subwaycode, subway);
		}

		if (HEAD == null)
		{
			HEAD = END = subway;
			return;
		}

		Subway temp = HEAD;
		HEAD = subway;
		HEAD.next_subway = temp;
		temp.prev_subway = subway;
	}


	public void InsertMid(Subway subway)
	{
		if (!dictionary.ContainsKey(subway.SubwayInfo.subwaycode))
		{
			dictionary.Add(subway.SubwayInfo.subwaycode, subway);
		}

		if (HEAD == null)
		{
			END = HEAD = subway;
			return;
		}

		InsertSubway(HEAD, subway);
	}

	private void InsertSubway(Subway compareSubway,Subway subway)
    {
		if (compareSubway == END)
		{
			InsertEnd(subway);
			return;
		}

		if (compareSubway.SubwayInfo.subwaycode.CompareTo(subway.SubwayInfo.subwaycode) > 0)
		{
			if (compareSubway == HEAD)
			{
				InsertHead(subway);
				return;
			}

			Subway temp = compareSubway.prev_subway;
			compareSubway.prev_subway = subway;
			subway.next_subway = compareSubway;
			subway.prev_subway = temp;
			temp.next_subway = subway;
			return;
		}
		else
		{
			InsertSubway(compareSubway.next_subway, subway);
		}
    }


	public void InsertEnd(Subway subway)
	{
		if (!dictionary.ContainsKey(subway.SubwayInfo.subwaycode))
		{
			dictionary.Add(subway.SubwayInfo.subwaycode, subway);
		}

		if (END == null)
		{
			HEAD = END = subway;
			return;
		}

		Subway temp = END;
		END = subway;
		END.prev_subway = temp;
		temp.next_subway = subway;
	}


	public void DeleteHead()
    {
		if (dictionary.ContainsKey(HEAD.SubwayInfo.subwaycode))
		{
			dictionary.Remove(HEAD.SubwayInfo.subwaycode);
		}

		if(END == HEAD)
        {
			END = HEAD = null;
			return;
        }

		HEAD = HEAD.next_subway;
    }

	/// <summary>
	/// key - Mes_Prod_Seq
	/// </summary>
	/// <param name="key"></param>
	/// <param name="subway"></param>

	public void DeleteMid(Subway subway)
	{
		if (dictionary.ContainsKey(subway.SubwayInfo.subwaycode))
		{
			dictionary.Remove(subway.SubwayInfo.subwaycode);
		}

		if (END == HEAD)
		{
			END = HEAD = null;
			return;
		}

		DeleteSubway(subway);
	}

	private void DeleteSubway(Subway subway)
    {
		if(subway == END)
        {
			DeleteEnd();
			return;
        }

		if(subway == HEAD)
        {
			DeleteHead();
			return;
        }

		subway.prev_subway.next_subway = subway.next_subway;
		subway.next_subway.prev_subway = subway.prev_subway;
	}

	/// <summary>
	/// key - Mes_Prod_Seq
	/// </summary>
	/// <param name="key"></param>
	/// <param name="car"></param>

	public void DeleteEnd()
	{
		if (dictionary.ContainsKey(END.SubwayInfo.subwaycode))
		{
			dictionary.Remove(END.SubwayInfo.subwaycode);
		}

		if (END == HEAD)
		{
			END = HEAD = null;
			return;
		}

		END = END.prev_subway;
	}

	/// <summary>
	/// 존재 여부 검색(키)
	/// </summary>
	/// <param name="key">검색할 Key</param>
	/// <returns></returns>
	public bool Contains(string key)
	{
		return dictionary.ContainsKey(key);
	}

	/// <summary>
	/// Key로 조회
	/// </summary>
	/// <param name="key">Key</param>
	/// <returns></returns>
	public Subway Get(string key)
	{
		return dictionary[key];
	}

	/// <summary>
	/// 총 개수 조회
	/// </summary>
	/// <returns></returns>
	public int Count()
	{
		return dictionary.Count;
	}

    public Dictionary<string,Subway> Dictionary
    {
        get
        {
            return dictionary;
        }
    }
}