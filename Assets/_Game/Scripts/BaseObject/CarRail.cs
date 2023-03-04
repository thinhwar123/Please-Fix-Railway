using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CarRail : IndestructibleRail
{
    public Wagon m_Wagon;
    
    private void Awake()
    {
        m_Wagon.SetWagonID(m_GroupID);
    }
    public override void OnGroupIDChange()
    {
        base.OnGroupIDChange();
        m_Wagon.SetWagonID(m_GroupID);
    }
}
