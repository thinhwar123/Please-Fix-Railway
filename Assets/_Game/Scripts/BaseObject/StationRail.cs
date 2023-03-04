using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
public class StationRail : IndestructibleRail
{
    public TextMeshPro m_TextStationID;
    public Transform m_Passenger;
    public bool m_HasPassenger;
    private void Awake()
    {
        m_TextStationID.text = m_GroupID.ToString();
    }
    public override void OnGroupIDChange()
    {
        base.OnGroupIDChange();
        m_TextStationID.text = m_GroupID.ToString();
    }
    public override void OnMoveUpdate(Wagon wagon, Connection currentConnection, float moveTime)
    {
        base.OnMoveUpdate(wagon, currentConnection, moveTime);
        if (m_HasPassenger && wagon.m_WagonID == m_GroupID && moveTime >= 0.5f)
        {
            m_HasPassenger = false;
            wagon.ChangeWagonState(WagonState.WaitPassenger);

            m_Passenger.DOMove(wagon.Transform.position, wagon.WaitPassengerDuration).SetEase(Ease.OutCubic).OnComplete(() => Destroy(m_Passenger.gameObject));
        }
    }
}
