using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum CreateEntityState
{
    Null = -1,
    Empty = 0,
    SelectObject = 1,
    HasObject = 2,
}

public class LevelCreatorInputHandle : Singleton<LevelCreatorInputHandle>
{
    private Camera m_MainCamera;
    public Camera MainCamera { get { return m_MainCamera ??= Camera.main; } }
    private Transform m_MainCameraTransform;
    public Transform MainCameraTransform { get { return m_MainCameraTransform ??= MainCamera.transform; } }

    [Header("Camera Controller")]
    [SerializeField] private Transform m_CameraPivot;
    [SerializeField] private float m_SmoothTime;
    [SerializeField] private float m_ZoomAmount;
    [SerializeField] private float m_ZoomMinAmout;
    [SerializeField] private float m_ZoomMaxAmout;
    private Vector3 m_DragStartPosition;
    private Vector3 m_DragCurrentPosition;
    private Vector3 m_NewPosition;
    private Vector3 m_NewZoom;
    private float m_ZoomValue;
    private Plane m_Plane;

    [Header("Interact Controller")]
    [SerializeField] private float m_Threshold;
    [SerializeField] private EntityType m_EntityType;
    [SerializeField] private CreateEntityState m_CreateEntityState;
    [SerializeField] private LayerMask m_WhatIsCell;
    [SerializeField] private LayerMask m_WhatIsEntity;

    [SerializeField] private Vector3 m_CurrentRotate;
    [SerializeField] private Entity m_DemoSpawnObject;
    [SerializeField] private GameObject m_StateTileObject;

    public EntityType EntityType { get => m_EntityType; set => m_EntityType = value; }
    #region Unity Functions
    private void Awake()
    {
        m_Plane = new Plane(Vector3.up, Vector3.zero);
        m_NewPosition = m_CameraPivot.position;
        m_NewZoom = MainCameraTransform.localPosition;
        m_ZoomValue = 0;

        m_EntityType = EntityType.Null;
        m_CreateEntityState = CreateEntityState.Null;
    }
    private void Update()
    {
        HandleMovement();
        HandleCreateObject();
    }
    #endregion

    #region Handle Input Functions
    public void HandleCreateObject()
    {
        if (EventSystem.current.IsPointerOverGameObject(-1))    // is the touch on the GUI
        {
            CreateDemoSpawnObject(EntityType.Null);
            CreateStateObject(CreateEntityState.Null);
            return;
        }

        Ray interactRay = MainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitCell;
        RaycastHit hitEntity;
        if (Physics.Raycast(interactRay, out hitCell, 100, m_WhatIsCell)) // check hit default tile
        {
            if (Physics.Raycast(interactRay, out hitEntity, 100, m_WhatIsEntity))  
            {
                if (m_EntityType != EntityType.Null && m_CreateEntityState != CreateEntityState.HasObject) // check state tile is has object 
                {
                    CreateStateObject(CreateEntityState.HasObject);
                    CreateDemoSpawnObject(EntityType.Null);
                }

                if (m_EntityType == EntityType.Null && m_CreateEntityState != CreateEntityState.SelectObject) // check state tile is select object
                {
                    CreateStateObject(CreateEntityState.SelectObject);
                }

                if (Input.GetKeyUp(KeyCode.Mouse1))
                {
                    RemoveSelectedTile(hitCell.collider.GetComponent<Cell>());
                }
                if (Input.GetKeyUp(KeyCode.Mouse0) && m_Threshold < 0.1f)
                {
                    EditSelectTile(hitCell.collider.GetComponent<Cell>());
                }
            }
            else
            {
                if (m_CreateEntityState != CreateEntityState.Empty) // check state tile is empty
                {
                    CreateStateObject(CreateEntityState.Empty);
                }

                if (m_EntityType != EntityType.Null) // check select object is not null
                {
                    CreateDemoSpawnObject(m_EntityType);

                    m_DemoSpawnObject.transform.position = hitCell.collider.transform.position + Vector3.up * 0.25f;

                    if (Input.GetKeyDown(KeyCode.Q))
                    {
                        RotateDemoEntity();
                    }
                    if (Input.GetKeyDown(KeyCode.W))
                    {
                        ChangeDemoEntity();
                    }
                }

                if (Input.GetKeyUp(KeyCode.Mouse0) && m_Threshold < 0.1f)
                {
                    CreateSelectedTile(hitCell.collider.GetComponent<Cell>());

                }
            }

            m_StateTileObject.transform.position = hitCell.collider.transform.position + Vector3.up * 0.25f;
        }
        else
        {
  
            CreateDemoSpawnObject(EntityType.Null);
            CreateStateObject(CreateEntityState.Null);
        }
    }
    public void HandleMovement()
    {
        if (Input.mouseScrollDelta.y != 0)
        {
            m_ZoomValue += Input.mouseScrollDelta.y * m_ZoomAmount;
            if (m_ZoomValue <= m_ZoomMaxAmout && m_ZoomValue >= -m_ZoomMinAmout)
            {
                m_NewZoom += Input.mouseScrollDelta.y * m_ZoomAmount * MainCameraTransform.forward;
            }
            else
            {
                m_ZoomValue -= Input.mouseScrollDelta.y * m_ZoomAmount;
            }
           
        }
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Ray ray = MainCamera.ScreenPointToRay(Input.mousePosition);
            float entry;
            if (m_Plane.Raycast(ray, out entry))
            {
                m_DragStartPosition = ray.GetPoint(entry);
            }
        }
        if (Input.GetKey(KeyCode.Mouse0))
        {
            Ray ray = MainCamera.ScreenPointToRay(Input.mousePosition);
            float entry;
            if (m_Plane.Raycast(ray, out entry))
            {
                m_DragCurrentPosition = ray.GetPoint(entry);
                m_Threshold = Vector3.Distance(m_DragStartPosition, m_DragCurrentPosition);
                m_NewPosition = m_CameraPivot.position + m_DragStartPosition - m_DragCurrentPosition;
                m_NewPosition.y = 0;
            }
        }

        Vector3 velocity = Vector3.zero;
        if (Vector3.Distance(m_CameraPivot.position, m_NewPosition) > 0.1f)
        {
            m_CameraPivot.position = Vector3.SmoothDamp(m_CameraPivot.position, m_NewPosition, ref velocity, m_SmoothTime);
        }
        if (Vector3.Distance(MainCameraTransform.localPosition, m_NewZoom) > 0.1f)
        {
            MainCameraTransform.localPosition = Vector3.SmoothDamp(MainCameraTransform.localPosition, m_NewZoom, ref velocity, m_SmoothTime);
        }

    }
    #endregion

    #region Other Functions
    public void RemoveDemoEntity()
    {
        if (m_DemoSpawnObject != null)
        {
            Destroy(m_DemoSpawnObject.gameObject);
            m_DemoSpawnObject = null;
        }
    }
    public void RotateDemoEntity()
    {
        m_CurrentRotate += Vector3.up * 90;
        m_DemoSpawnObject.transform.localEulerAngles = m_CurrentRotate;
    }
    public void ChangeDemoEntity()
    {
        RemoveDemoEntity();
        LevelCreator.Instance.ChangeEntity(m_EntityType);
    }
    public void CreateStateObject(CreateEntityState createEntityState)
    {
        if (m_StateTileObject != null)
        {
            Destroy(m_StateTileObject);
            m_StateTileObject = null;
        }
        m_CreateEntityState = createEntityState;

        if (createEntityState == CreateEntityState.Null) return;
        m_StateTileObject = Instantiate(EntityGlobalConfig.Instance.GetEntityState(m_CreateEntityState));
    }
    public void CreateDemoSpawnObject(EntityType entityType)
    {
        if (entityType == EntityType.Null)
        {
            if (m_DemoSpawnObject != null)
            {
                Destroy(m_DemoSpawnObject.gameObject);
                m_DemoSpawnObject = null;
            }
        }
        else
        {
            if (m_DemoSpawnObject == null)
            {
                if (EntityGlobalConfig.Instance.GetCurrentEntity(m_EntityType) == null)
                {
                    // TODO: check null later
                    return;
                }
                m_DemoSpawnObject = Instantiate(EntityGlobalConfig.Instance.GetCurrentEntity(m_EntityType));

                m_DemoSpawnObject.Transform.localEulerAngles = m_CurrentRotate;
                m_DemoSpawnObject.gameObject.layer = LayerMask.NameToLayer("Default");
                Transform[] transformList = m_DemoSpawnObject.GetComponentsInChildren<Transform>();
                for (int i = 0; i < transformList.Length; i++)
                {
                    transformList[i].gameObject.layer = LayerMask.NameToLayer("Default");
                }
            }
        }

    }
    public void CreateSelectedTile(Cell cell)
    {
        LevelCreator.Instance.CreateEntity(EntityGlobalConfig.Instance.GetCurrentEntity(m_EntityType), cell, m_DemoSpawnObject.transform.rotation);
    }
    public void RemoveSelectedTile(Cell cell)
    {
        LevelCreator.Instance.RemoveCurrentEntity(cell);
    }
    public void EditSelectTile(Cell cell)
    {
        LevelCreator.Instance.EditSelectEntity(cell);
    }
    #endregion

}
