using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System.Linq;

[CreateAssetMenu(fileName = "TileGlobalConfig", menuName = "ScriptableObjects/GlobalConfig/EntityGlobalConfig")]
[GlobalConfig("Assets/Resources/GlobalConfig/")]
public class EntityGlobalConfig : GlobalConfig<EntityGlobalConfig>
{
    public UIEditEntity m_UIEditEntity;
    public List<EntityConfig> m_EntityConfigList;
    public List<GameObject> m_EntityStateList;
    public Entity GetCurrentEntity(EntityType entityType)
    {
        return m_EntityConfigList.FirstOrDefault(e => e.EntityType == entityType).GetCurrentPrefab();
    }
    public Entity GetEntity(EntityType entityType, int index)
    {
        return m_EntityConfigList.FirstOrDefault(e => e.EntityType == entityType).GetPrefab(index);
    }
    public GameObject GetEntityState(CreateEntityState createEntityState)
    {
        return m_EntityStateList[(int)createEntityState];
    }
}
