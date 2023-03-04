using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MapData {
    public int m_Level;
    public int m_MaxRail;
    public int m_CurrentRail;

    public void Init(int level, int maxRail) {
        m_Level = level;
        m_MaxRail = maxRail;
    }
    public void AddRail(int amount = 1) {
        m_CurrentRail += amount;
    }
    public void ConsumeRail(int amount = 1) {
        m_CurrentRail -= amount;
    }
    public void SetCurrentRail(int amount) {
        m_CurrentRail = amount;
    }
    public void SetMaxRail(int amount) {
        m_MaxRail = amount;
    }
}
