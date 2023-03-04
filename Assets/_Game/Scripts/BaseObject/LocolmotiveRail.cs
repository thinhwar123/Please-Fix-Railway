using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocolmotiveRail : IndestructibleRail
{
    public Transform m_Locolmotive;

    public void UpdateLocolmotivePosition(int wagonCount)
    {
        m_Locolmotive.transform.localPosition = Vector3.right * wagonCount;
    }
}
