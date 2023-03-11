using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

[System.Serializable]
public class ConnectionCode 
{
    [SerializeField] private List<int> m_Code;
    public ConnectionCode(params int[] code)
    {
        m_Code = new List<int>(code);
    }
    public ConnectionCode(List<int> code)
    {
        m_Code = new List<int>(code);
    }

    public List<int> GetBaseConnectionCode()
    {
        return m_Code;
    }
    public List<int> GetCurrentConnectionCode(Transform entity)
    {
        return m_Code.Select(x => Mathf.RoundToInt(x - 1 + entity.localEulerAngles.y / 90) % 4 + 1).ToList();
    }
    public List<int> GetRotateCode(int rotateTime)
    {
        return m_Code.Select(x => Mathf.RoundToInt(x - 1 + rotateTime) % 4 + 1).ToList();
    }
    public List<int> GetShiftCode(int shiftTime)
    {
        List<int> result = new List<int>();
        for (int i = 0; i < m_Code.Count; i++)
        {
            result.Add(m_Code[(i + shiftTime) % m_Code.Count]);
        }
        return result;
    }
    public ConnectionCode GetRotateConnectionCode(int rotateTime)
    {
        return new ConnectionCode(GetRotateCode(rotateTime));
    }
    public ConnectionCode GetShiftConnectionCode(int shiftTime)
    {
        return new ConnectionCode(GetShiftCode(shiftTime));
    }
    public int GetRotateTime(ConnectionCode connectionCode)
    {
        if (connectionCode.m_Code.Count != m_Code.Count)
        {
            return -1;
        }
        for (int i = 0; i < m_Code.Count; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                if (connectionCode.IsTheSame(GetShiftConnectionCode(i).GetRotateConnectionCode(j)))
                {
                    return j;
                }
            }

        }
        return -1;
    }
    public bool CanTransformTo(ConnectionCode connectionCode)
    {
        for (int i = 0; i < m_Code.Count; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                if (connectionCode.IsTheSame(GetShiftConnectionCode(i).GetRotateConnectionCode(j)))
                {
                    return true;
                }
            }

        }
        return false;
    }
    public bool IsTheSame(ConnectionCode connectionCode)
    {
        if (m_Code.Count != connectionCode.m_Code.Count) return false;
        for (int i = 0; i < m_Code.Count; i++)
        {
            if (m_Code[i] != connectionCode.m_Code[i])
            {
                return false;
            }
        }
        return true;
    }
    public override string ToString()
    {
        string s = "[ ";
        for (int i = 0; i < m_Code.Count; i++)
        {
            s += $"{m_Code[i]} ";
        }
        s += "]";

        return s;
    }
}
