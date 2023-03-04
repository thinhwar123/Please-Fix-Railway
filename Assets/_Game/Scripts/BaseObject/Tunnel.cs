using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class Tunnel : BasicRail
{
    [SerializeField] private MeshRenderer m_MeshRenderer;
    [SerializeField] private List<Material> m_MaterialTunnel;

    public Tunnel m_OtherTunnel;
    public Transform m_RaycastTransform;
    private bool m_IsReadyTrigger;

    private void Awake()
    {
        UpdateColorTunnel();
    }
    public override void OnGroupIDChange()
    {
        base.OnGroupIDChange();
        UpdateColorTunnel();
    }
    public override BasicRail GetNextRail(Connection nextConnection)
    {
        if (nextConnection == m_Connections[3])
        {
            return m_OtherTunnel;
        }
        return base.GetNextRail(nextConnection);

    }
    public override Connection GetConnection(Connection otherConnection)
    {
        if (m_OtherTunnel.IsInConnection(otherConnection)) return m_Connections[3];
        return m_Connections[1];
    }

    public override void OnMoveUpdate(Wagon wagon, Connection currentConnection, float moveTime)
    {
        base.OnMoveUpdate(wagon, currentConnection, moveTime);
        float fixTime = IsInConnection(currentConnection) ? 0.1f : -0.1f;
        if (moveTime < 0.5f + fixTime && !m_IsReadyTrigger)
        {
            m_IsReadyTrigger = true;
        }
        if (moveTime >= 0.5f + fixTime && m_IsReadyTrigger)
        {
            m_IsReadyTrigger = false;
            wagon.SetHideFromMask(!IsInConnection(currentConnection));
        }
    }
    private void UpdateColorTunnel()
    {
        if (m_GroupID < 0 || m_GroupID >= m_MaterialTunnel.Count) return;
        m_MeshRenderer.material = m_MaterialTunnel[m_GroupID];
    }
    private bool IsInConnection(Connection otherConnection)
    {
        return otherConnection == m_Connections[3];
    }
}
