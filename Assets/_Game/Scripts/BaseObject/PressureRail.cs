using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PressureRail : IndestructibleRail
{
    [SerializeField] private MeshRenderer m_MeshRenderer;
    [SerializeField] private List<Material> m_MaterialPressureRail;

    private List<DynamicRail> m_DynamicRail;
    private bool m_IsReadyTrigger;
    private void Awake()
    {
        UpdateColorPressureRail();
    }
    public override void OnGroupIDChange()
    {
        base.OnGroupIDChange();
        UpdateColorPressureRail();
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
            TriggerDynamicRail();
        }
    }
    private void TriggerDynamicRail()
    {
        for (int i = 0; i < m_DynamicRail.Count; i++)
        {
            m_DynamicRail[i].TriggerEvent();
        }
    }
    public void GetDynamicRail()
    {
        m_DynamicRail = CellManager.Instance.CellList.Select(x => x.Entity).Where(e => e != null && e.EntityType == EntityType.DynamicRail && e.GroupID == m_GroupID).Cast<DynamicRail>().ToList();
    }
    public void UpdateColorPressureRail()
    {
        if (m_GroupID < 0 || m_GroupID >= m_MaterialPressureRail.Count) return;
        m_MeshRenderer.material = m_MaterialPressureRail[m_GroupID];
    }
}
