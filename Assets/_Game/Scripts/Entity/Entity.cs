using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum EntityType
{
    Null = 0,
    IndestructibleRail = 1,
    DestructionRail = 2,
    Tunnel = 3,
    PressureRail = 4,
    DynamicRail = 5,
    Obstacle = 6,
    Locomotive = 7,
    CarRail = 8,
    SpecialRail = 9,
}

public class Entity : MonoBehaviour
{
    private static List<EntityType> m_EntityCanEditList
    {
        get
        {
            return new List<EntityType>()
            {
                EntityType.Tunnel,
                EntityType.DynamicRail,
                EntityType.CarRail,
                EntityType.PressureRail,
                EntityType.SpecialRail,
            };
        }
    }

    private Transform m_Transform;
    public Transform Transform { get => m_Transform ??= transform; }

    [SerializeField] private EntityType m_EntityType;

    [SerializeField] protected int m_Index;
    [SerializeField] protected int m_GroupID;
    [SerializeField] protected Coordinates m_Coordinates;

    private UIEditEntity m_CurrentUIEditEntity;


    public EntityType EntityType { get => m_EntityType; }
    public int Index { get => m_Index; }
    public int GroupID
    {
        get
        {
            return m_GroupID;
        }
        set
        {
            m_GroupID = value;
            OnGroupIDChange();
        }
    }
    public Coordinates Coordinates { get => m_Coordinates; set => m_Coordinates = value; }

    public void EditSelectEntity()
    {
        Debug.Log("1");
        if (m_CurrentUIEditEntity != null) return;
        Debug.Log("2");
        if (!m_EntityCanEditList.Contains(EntityType)) return;
        Debug.Log("3");
        m_CurrentUIEditEntity = Instantiate<UIEditEntity>(EntityGlobalConfig.Instance.m_UIEditEntity, LevelCreator.Instance.m_MainCanvas);
        m_CurrentUIEditEntity.Setup(this, OnClickSaveCallback);
    }
    private void OnClickSaveCallback()
    {
        Destroy(m_CurrentUIEditEntity.gameObject);
        m_CurrentUIEditEntity = null;

    }
    public virtual void OnGroupIDChange()
    {

    }
}
