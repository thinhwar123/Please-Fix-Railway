using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    private Transform m_Transform;
    public Transform Transform { get => m_Transform ??= transform; }

    [SerializeField] private Coordinates m_Coordinates;
    [SerializeField] private Entity m_Entity;

    public Coordinates Coordinates { get => m_Coordinates ??= new Coordinates(); }
    public Entity Entity { get => m_Entity; }


    public void Register(Entity entity)
    {
        if (m_Entity == null)
        {
            m_Entity = entity;
        }
    }
    public void Unregister(Entity entity)
    {
        if (m_Entity == entity)
        {
            m_Entity = null;
        }
    }
    public void RemoveCurrentEnity()
    {        
        if (m_Entity == null) return;
        Entity entity = m_Entity;
        Unregister(entity);
        entity.OnDespawn();
        Destroy(entity.gameObject, 0.5f);
    }
    public void AddEnity(Entity entity, Quaternion rotation)
    {
        if (entity == null) return;
        Register(entity);
        entity.OnSpawn();
        entity.Transform.SetParent(CellManager.Instance.EntityTransform);
        entity.Transform.position = Transform.position;
        entity.Transform.rotation = rotation;
        entity.Coordinates = Coordinates;
    }
    public void AddEnity(Entity entity)
    {
        if (entity == null) return;
        Register(entity);
        entity.OnSpawn();
        entity.Transform.SetParent(CellManager.Instance.EntityTransform);
        entity.Transform.position = Transform.position;
        entity.Coordinates = Coordinates;
    }
    public void ReplaceEntity(Entity entity)
    {
        Entity currentEntity = m_Entity;
        Unregister(currentEntity);
        Destroy(currentEntity.gameObject);

        AddEnity(entity);
    }

}
