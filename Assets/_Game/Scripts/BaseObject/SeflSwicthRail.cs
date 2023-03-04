using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeflSwicthRail : IndestructibleRail
{
    [SerializeField] private GameObject m_SwitchLeftRail;
    [SerializeField] private List<Connection> m_SwitchLeftConnections;
    [SerializeField] private GameObject m_SwitchRightRail;
    [SerializeField] private List<Connection> m_SwitchRightConnections;
    [SerializeField] private bool m_IsSwitchLeft;
    [SerializeField] private bool m_IsReadyTrigger;
    [SerializeField] private Transform m_Switch;

    private Tween m_RotateTween;
    public void TriggerEvent()
    {
        m_IsSwitchLeft = !m_IsSwitchLeft;

        m_SwitchLeftRail.SetActive(m_IsSwitchLeft);
        m_SwitchRightRail.SetActive(!m_IsSwitchLeft);

        m_Connections = m_IsSwitchLeft ? m_SwitchLeftConnections : m_SwitchRightConnections;


        Vector3 newAngle = m_IsSwitchLeft ? new Vector3(-160f, 0f, 90f) : new Vector3(-20f, 0f, 90f);
        m_RotateTween?.Kill();
        m_RotateTween = m_Switch.DOLocalRotate(newAngle, 0.7f).SetEase(Ease.OutBack);
    }
    public override void OnMoveUpdate(Wagon wagon, Connection currentConnection, float moveTime)
    {
        base.OnMoveUpdate(wagon, currentConnection, moveTime);

        if (moveTime < 0.5f && !m_IsReadyTrigger)
        {
            m_IsReadyTrigger = true;
        }
        if (moveTime >= 0.5f && m_IsReadyTrigger)
        {
            m_IsReadyTrigger = false;
            TriggerEvent();
        }
    }
}
