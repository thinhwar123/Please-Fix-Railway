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
        // TODO: Add animation
        if (m_Entity == null) return;
        Entity entity = m_Entity;
        Unregister(entity);
        Destroy(entity.gameObject);
    }
    public void AddEnity(Entity entity, Quaternion rotation)
    {
        if (entity == null) return;
        Register(entity);
        entity.Transform.SetParent(CellManager.Instance.EntityTransform);
        entity.Transform.position = Transform.position;
        entity.Transform.rotation = rotation;
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
