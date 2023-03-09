using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyExtension;

public class GameInputHandler : Singleton<GameInputHandler>
{
    enum EnityModifyMode
    {
        Disable = 0,
        Add = 1,
        Remove = 2,
    }

    private Camera m_MainCamera;
    public Camera MainCamera { get { return m_MainCamera ??= Camera.main; } }
    private Transform m_MainCameraTransform;
    public Transform MainCameraTransform { get { return m_MainCameraTransform ??= MainCamera.transform; } }
    
    [SerializeField] private EnityModifyMode m_EnityModifyMode;
    [SerializeField] private LayerMask m_WhatIsCell;

    [SerializeField] private DestructibleRail m_StraightRail;
    [SerializeField] private DestructibleRail m_SwitchLeftRail;
    [SerializeField] private DestructibleRail m_SwitchRightRail;
    [SerializeField] private DestructibleRail m_TurnRail;

    private Dictionary<Cell, List<Cell>> m_CellConnectDictionary = new Dictionary<Cell, List<Cell>>();
    private Ray m_InteractRay;
    private RaycastHit m_RaycastHit;
    [SerializeField] private Cell m_HitCell;
    [SerializeField] private Cell m_LastHitCell;


    #region Unity Functions
    private void Awake()
    {

    }
    private void Update()
    {
        HandleAddEntity();
        HandleRemoveEntity();
    }
    #endregion

    #region Handle Input Functions
    public void HandleAddEntity()
    {
        if (m_EnityModifyMode != EnityModifyMode.Add) return;
        if (Extension.IsPointerOverUIGameObject()) return;

        if (Input.GetKey(KeyCode.Mouse0))
        {
            m_InteractRay = MainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(m_InteractRay, out m_RaycastHit, 100, m_WhatIsCell))
            {
                m_HitCell = m_RaycastHit.collider.GetComponent<Cell>();
                if (m_LastHitCell != m_HitCell)
                {
                    if (m_LastHitCell == null && m_HitCell.Entity == null)
                    {
                        CreateDefaultRail(m_HitCell);
                    }
                    else if (m_LastHitCell == null && m_HitCell.Entity != null)
                    {
                        UpdateCorrectRail(m_HitCell);
                    }
                    else if (m_LastHitCell != null && m_HitCell.Entity == null)
                    {
                        CreateCorrectRail(m_HitCell, m_LastHitCell);
                    }
                    else if (m_LastHitCell != null && m_HitCell.Entity != null)
                    {
                        UpdateCorrectRail(m_HitCell, m_LastHitCell);
                    }


                    m_LastHitCell = m_HitCell;
                }
            }
        }
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            m_LastHitCell = null;
            m_HitCell = null;
        }
    }
    public void HandleRemoveEntity()
    {
        if (m_EnityModifyMode != EnityModifyMode.Remove) return;
        if (Extension.IsPointerOverUIGameObject()) return;

    }

    #endregion

    #region Other Functions
    private void CreateDefaultRail(Cell cell)
    {
        cell.AddEnity(GetDefaultRail());
    }
    private void UpdateCorrectRail(Cell cell)
    {
        if (cell.Entity.EntityType != EntityType.DestructionRail || GetCellConnections(cell).Count == 3) return;
        
    }
    private void CreateCorrectRail(Cell cell, Cell lastCell)
    {
        if (lastCell.Entity is DestructibleRail)
        {

        }
        else
        {

        }
    }
    private void UpdateCorrectRail(Cell cell, Cell lastCell)
    {

    }
    private void AddConnectionCell(Cell cell, Cell connectCell) 
    {
        GetCellConnections(cell).Add(connectCell);

        if (GetCellConnections(cell).Count > 3)
        {
            GetCellConnections(cell).RemoveAt(0);
        }
    }
    private List<Cell> GetCellConnections(Cell cell)
    {
        if (!m_CellConnectDictionary.ContainsKey(cell))
        {
            m_CellConnectDictionary.Add(cell, new List<Cell>());
        }
        return m_CellConnectDictionary[cell];
    }
    private DestructibleRail FindCorrectRail(Cell cell)
    {
        if (GetCellConnections(cell).Count == 1)
        {

            return Instantiate(m_StraightRail);
        }
        return null;
    }
    private DestructibleRail GetDefaultRail()
    {
        return Instantiate(m_StraightRail);
    }
    [Sirenix.OdinInspector.Button]
    private List<int> GetEntityOpenConnection(Entity entity)
    {
        List<int> openConnections = new List<int>();
        if (entity is not BasicRail) return openConnections;

        BasicRail basicRail = entity as BasicRail;
        for (int i = 0; i < basicRail.m_Connections.Count; i++)
        {
            openConnections.Add(GetDirectionIndex(basicRail.m_Connections[i]));
        }
        return openConnections;
    }
    private int GetDirectionIndex(Connection connection)
    {
        Debug.Log(connection.GetConnectDirection());
        if (connection.m_NexConnection == null) return 0;

        if (Vector3.Distance(connection.GetConnectDirection(), Vector3.left) < 0.1f)
        {
            return 1;
        }
        else if (Vector3.Distance(connection.GetConnectDirection(), Vector3.forward) < 0.1f)
        {
            return 2;
        }
        else if (Vector3.Distance(connection.GetConnectDirection(), Vector3.right) < 0.1f)
        {
            return 3;
        }
        else if (Vector3.Distance(connection.GetConnectDirection(), Vector3.back) < 0.1f)
        {
            return 4;
        }
        return 0;
    }
    #endregion
}