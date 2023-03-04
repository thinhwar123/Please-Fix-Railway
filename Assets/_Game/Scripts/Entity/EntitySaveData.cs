using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EntitySaveData 
{
    public EntityType m_EntityType;
    public int m_Index;
    public int m_GroupID;
    public Coordinates m_Coordinates;
    public Quaternion m_Rotation;
    public EntitySaveData()
    {
        m_EntityType = EntityType.Null;
        m_Index = 0;
        m_GroupID = 0;
        m_Coordinates = new Coordinates(0, 0);
        m_Rotation = Quaternion.identity;
    }
    public EntitySaveData(EntityType entityType, int index, int groupID, Coordinates coordinates, Quaternion rotation)
    {
        m_EntityType = entityType;
        m_Index = index;
        m_GroupID = groupID;
        m_Coordinates = coordinates;
        m_Rotation = rotation;
    }
    public EntitySaveData(Entity entity)
    {
        m_EntityType = entity.EntityType;
        m_Index = entity.Index;
        m_GroupID = entity.GroupID;
        m_Coordinates = entity.Coordinates;
        m_Rotation = entity.Transform.rotation;
    }
}
