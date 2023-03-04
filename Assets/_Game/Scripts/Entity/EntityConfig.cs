using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EntityConfig
{
    [SerializeField] private EntityType m_EntityType;
    [SerializeField] private int m_CurrentIndex;

    [SerializeField] private List<Entity> m_EnityPrefabList;
    [SerializeField] private List<Sprite> m_EnitySpriteList;

    public EntityType EntityType { get => m_EntityType; }
    public List<Entity> EnityPrefabList { get => m_EnityPrefabList; set => m_EnityPrefabList = value; }
    public List<Sprite> EnitySpriteList { get => m_EnitySpriteList; set => m_EnitySpriteList = value; }

    public int GetPrefabCount()
    {
        return m_EnitySpriteList.Count;
    }
    public Entity GetCurrentPrefab()
    {
        if (m_EnityPrefabList == null || m_EnityPrefabList.Count == 0) return null;
        return m_EnityPrefabList[m_CurrentIndex % m_EnityPrefabList.Count];
    }
    public Entity GetPrefab(int index)
    {
        if (m_EnityPrefabList == null || m_EnityPrefabList.Count == 0) return null;
        return m_EnityPrefabList[index];
    }
    public Sprite GetCurrentSprite()
    {
        if (m_EnitySpriteList == null || m_EnitySpriteList.Count == 0) return null;
        return m_EnitySpriteList[m_CurrentIndex % m_EnitySpriteList.Count];
    }
    public void IncreaseIndex()
    {
        m_CurrentIndex = (m_CurrentIndex + 1) % m_EnityPrefabList.Count;
    }
}
