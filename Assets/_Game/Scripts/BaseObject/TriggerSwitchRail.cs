using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerSwitchRail : DynamicRail
{
    [SerializeField] private GameObject m_SwitchLeftRail;
    [SerializeField] private List<Connection> m_SwitchLeftConnections;
    [SerializeField] private GameObject m_SwitchRightRail;
    [SerializeField] private List<Connection> m_SwitchRightConnections;
    [SerializeField] private bool m_IsSwitchLeft;

    public override void TriggerEvent()
    {
        base.TriggerEvent();
        m_IsSwitchLeft = !m_IsSwitchLeft;

        m_SwitchLeftRail.SetActive(m_IsSwitchLeft);
        m_SwitchRightRail.SetActive(!m_IsSwitchLeft);

        m_Connections = m_IsSwitchLeft ? m_SwitchLeftConnections : m_SwitchRightConnections;
    }
}
