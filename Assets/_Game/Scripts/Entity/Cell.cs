using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Cell : MonoBehaviour
{
    private Transform m_Transform;
    public Transform Transform { get => m_Transform ??= transform; }
    [SerializeField] private BoxCollider m_BoxCollider;
    [SerializeField] private Vector3 m_EntityOffset;

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
        entity.Transform.position = Transform.position + m_EntityOffset;
        entity.Transform.rotation = rotation;
        entity.Coordinates = Coordinates;
    }
    public void AddEnity(Entity entity, int rotateTime)
    {
        if (entity == null) return;
        Register(entity);
        entity.OnSpawn();
        entity.Transform.SetParent(CellManager.Instance.EntityTransform);
        entity.Transform.position = Transform.position + m_EntityOffset;
        entity.Transform.eulerAngles = new Vector3(0, 90 * rotateTime, 0);
        entity.Coordinates = Coordinates;
    }
    public void ReplaceEntity(Entity entity, int rotateTime)
    {
        Entity currentEntity = m_Entity;
        Unregister(currentEntity);
        Destroy(currentEntity.gameObject);

        if (entity == null) return;
        Register(entity);
        entity.OnReplace();
        entity.Transform.SetParent(CellManager.Instance.EntityTransform);
        entity.Transform.position = Transform.position + m_EntityOffset;
        entity.Transform.eulerAngles = new Vector3(0, 90 * rotateTime, 0);
        entity.Coordinates = Coordinates;
    }

    public Material[] material;
    public Renderer renderer;
    //Link
    public void OnStart()
    {
        renderer.material = material[0];
    }

    public void OnChange()
    {
        renderer.material = material[1];
    }


    //private void FixedUpdate()
    //{
    //    if (time >= 0f)
    //    {
    //        time += Time.fixedDeltaTime * speed;
    //        transform.position = new Vector3(transform.position.x, curveY.Evaluate(time), transform.position.z);
    //    }
    //}
}


