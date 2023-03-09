using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
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
[RequireComponent(typeof(BoxCollider))]
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
    [SerializeField] private BoxCollider m_BoxCollider;
    [SerializeField] private Transform m_Model;

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
        if (m_CurrentUIEditEntity != null) return;
        if (!m_EntityCanEditList.Contains(EntityType)) return;
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
    public void OnDespawn()
    {
        m_Model.localScale = Vector3.one;
        m_Model.DOScale(Vector3.zero, 0.3f).SetEase(Ease.InBack);

    }
    public void OnSpawn()
    {
        m_Model.localScale = Vector3.zero;
        m_Model.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutBack);
    }
    //[Sirenix.OdinInspector.Button]
    //public void Setup()
    //{
    //    m_Model = MyExtension.EditorExtension.FindChildOrCreate(transform, "Model");
    //    List<Transform> tempList = new List<Transform>();
    //    for (int i = 0; i < transform.childCount; i++)
    //    {
    //        Debug.Log(transform.GetChild(i).name);
    //        if (transform.GetChild(i).name != "Connections" && transform.GetChild(i).name != "Model")
    //        {
    //            tempList.Add(transform.GetChild(i));
    //        }
    //    }
    //    for (int i = 0; i < tempList.Count; i++)
    //    {
    //        tempList[i].SetParent(m_Model);
    //    }

    //}
}
