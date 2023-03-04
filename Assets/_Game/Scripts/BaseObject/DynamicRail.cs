using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicRail : IndestructibleRail
{
    public List<MeshRenderer> m_MeshRenderers;
    public List<Material> m_MaterialDynamicRail;
    private void Awake()
    {
        UpdateColorDynamicRail();
    }
    public override void OnGroupIDChange()
    {
        base.OnGroupIDChange();
        UpdateColorDynamicRail();
    }

    public virtual void TriggerEvent()
    {

    }
    public void UpdateColorDynamicRail()
    {
        if (m_GroupID < 0 || m_GroupID >= m_MaterialDynamicRail.Count) return;
        for (int i = 0; i < m_MeshRenderers.Count; i++)
        {
            m_MeshRenderers[i].material = m_MaterialDynamicRail[m_GroupID];
        }        
    }
}
