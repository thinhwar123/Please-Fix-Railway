using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyExtension;
using Sirenix.OdinInspector;

public class GameInputHandler : Singleton<GameInputHandler>
{
    public enum EntityModifyMode
    {
        Disable = 0,
        Add = 1,
        Remove = 2,
    }

    private Camera m_MainCamera;
    public Camera MainCamera { get { return m_MainCamera ??= Camera.main; } }
    private Transform m_MainCameraTransform;
    public Transform MainCameraTransform { get { return m_MainCameraTransform ??= MainCamera.transform; } }
    
    [SerializeField] private EntityModifyMode m_EnityModifyMode;
    [SerializeField] private LayerMask m_WhatIsCell;

    [DictionaryDrawerSettings(), SerializeField]
    private Dictionary<Cell, List<Cell>> m_CellConnectDictionary = new Dictionary<Cell, List<Cell>>();
    private Ray m_InteractRay;
    private RaycastHit m_RaycastHit;
    private Cell m_HitCell;
    private Cell m_LastHitCell;

    private DestructibleRail m_DestructibleRailPrefab;
    private DestructibleRail m_SpawnDestructibleRail;
    private int m_RotateTime;



    #region Unity Functions

    private void Update()
    {
        HandleSwapMode();
        HandleAddEntity();
        HandleRemoveEntity();
    }
    #endregion

    public void ActiveModifyMode()
    {
        m_EnityModifyMode = EntityModifyMode.Add;
        
        InitConnectionCell();
    }
    public void DeactiveModifyMode()
    {
        m_EnityModifyMode = EntityModifyMode.Disable;
    }
    public void ChangeEntityModifyMode(EntityModifyMode entityModifyMode)
    {
        m_EnityModifyMode = entityModifyMode;
    }

    #region Handle Input Functions
    public void HandleSwapMode()
    {
        if (Input.GetKeyDown(KeyCode.Q) && m_EnityModifyMode == EntityModifyMode.Remove)
        {
            m_EnityModifyMode = EntityModifyMode.Add;
        }
        if (Input.GetKeyDown(KeyCode.W) && m_EnityModifyMode == EntityModifyMode.Add)
        {
            m_EnityModifyMode = EntityModifyMode.Remove;
        }
    }
    public void HandleAddEntity()
    {
        if (m_EnityModifyMode != EntityModifyMode.Add) return;
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
        if (m_EnityModifyMode != EntityModifyMode.Remove) return;
        if (Extension.IsPointerOverUIGameObject()) return;
        if (Input.GetKey(KeyCode.Mouse0))
        {
            m_InteractRay = MainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(m_InteractRay, out m_RaycastHit, 100, m_WhatIsCell))
            {
                m_HitCell = m_RaycastHit.collider.GetComponent<Cell>();
                if (m_LastHitCell != m_HitCell )
                {
                    RemoveEnity(m_HitCell);
                    m_LastHitCell = m_HitCell;
                }
            }
        }
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            m_HitCell = null;
            m_LastHitCell = null;
        }
    }

    #endregion

    #region Other Functions
    private void CreateDefaultRail(Cell cell)
    {
        (DestructibleRail railPrefab, int rotate) = DestructibleRailMananger.Instance.DestructilbeRail(new ConnectionCode(2, 4));
        DestructibleRail rail = Instantiate(railPrefab);
        cell.AddEnity(rail, rotate);
    }
    private void UpdateCorrectRail(Cell cell)
    {
        if (cell.Entity is not DestructibleRail || GetCellConnections(cell).Count != 3) return;
        SwapCellConnection(cell);

        (m_DestructibleRailPrefab, m_RotateTime) = DestructibleRailMananger.Instance.DestructilbeRail(GetConnectionCode(cell));
        m_SpawnDestructibleRail = Instantiate(m_DestructibleRailPrefab);
        cell.ReplaceEntity(m_SpawnDestructibleRail, m_RotateTime);

    }
    private void CreateCorrectRail(Cell cell, Cell lastCell)
    {
        if (lastCell.Entity is DestructibleRail )
        {
            AddConnectionCell(lastCell, cell);
            AddConnectionCell(cell, lastCell);

            (m_DestructibleRailPrefab, m_RotateTime) = DestructibleRailMananger.Instance.DestructilbeRail(GetConnectionCode(cell));
            m_SpawnDestructibleRail = Instantiate(m_DestructibleRailPrefab);
            cell.AddEnity(m_SpawnDestructibleRail, m_RotateTime);

            (m_DestructibleRailPrefab, m_RotateTime) = DestructibleRailMananger.Instance.DestructilbeRail(GetConnectionCode(lastCell));

            m_SpawnDestructibleRail = Instantiate(m_DestructibleRailPrefab);
            lastCell.ReplaceEntity(m_SpawnDestructibleRail, m_RotateTime);
        }
        else if (lastCell.Entity is IndestructibleRail)
        {
            IndestructibleRail indestructibleRail = lastCell.Entity as IndestructibleRail;
            if (indestructibleRail.CanConnectToCell(cell))
            {
                AddConnectionCell(cell, lastCell);
            }

            (m_DestructibleRailPrefab, m_RotateTime) = DestructibleRailMananger.Instance.DestructilbeRail(GetConnectionCode(cell));
            m_SpawnDestructibleRail = Instantiate(m_DestructibleRailPrefab);
            cell.AddEnity(m_SpawnDestructibleRail, m_RotateTime);
        }

    }
    private void UpdateCorrectRail(Cell cell, Cell lastCell)
    {
        if (lastCell.Entity is DestructibleRail && cell.Entity is DestructibleRail)
        {
            AddConnectionCell(lastCell, cell);
            AddConnectionCell(cell, lastCell);

            (m_DestructibleRailPrefab, m_RotateTime) = DestructibleRailMananger.Instance.DestructilbeRail(GetConnectionCode(cell));
            m_SpawnDestructibleRail = Instantiate(m_DestructibleRailPrefab);
            cell.ReplaceEntity(m_SpawnDestructibleRail, m_RotateTime);

            (m_DestructibleRailPrefab, m_RotateTime) = DestructibleRailMananger.Instance.DestructilbeRail(GetConnectionCode(lastCell));
            m_SpawnDestructibleRail = Instantiate(m_DestructibleRailPrefab);
            lastCell.ReplaceEntity(m_SpawnDestructibleRail, m_RotateTime);
        }
        else if (lastCell.Entity is IndestructibleRail && cell.Entity is DestructibleRail)
        {
            IndestructibleRail indestructibleRail = lastCell.Entity as IndestructibleRail;
            if (indestructibleRail.CanConnectToCell(cell))
            {
                AddConnectionCell(cell, lastCell);
            }

            (m_DestructibleRailPrefab, m_RotateTime) = DestructibleRailMananger.Instance.DestructilbeRail(GetConnectionCode(cell));
            m_SpawnDestructibleRail = Instantiate(m_DestructibleRailPrefab);
            cell.ReplaceEntity(m_SpawnDestructibleRail, m_RotateTime);
        }
        else if (lastCell.Entity is DestructibleRail && cell.Entity is IndestructibleRail)
        {
            IndestructibleRail indestructibleRail = cell.Entity as IndestructibleRail;
            if (indestructibleRail.CanConnectToCell(lastCell))
            {
                AddConnectionCell(lastCell, cell);
            }

            (m_DestructibleRailPrefab, m_RotateTime) = DestructibleRailMananger.Instance.DestructilbeRail(GetConnectionCode(lastCell));
            m_SpawnDestructibleRail = Instantiate(m_DestructibleRailPrefab);
            lastCell.ReplaceEntity(m_SpawnDestructibleRail, m_RotateTime);
        }
    }
    public void RemoveEnity(Cell cell)
    {
        if (cell.Entity != null && cell.Entity is DestructibleRail)
        {
            ClearConnectionCell(cell);
            cell.RemoveCurrentEnity();
        }
    }
    private void AddConnectionCell(Cell cell, Cell connectCell) 
    {
        if (GetCellConnections(cell).Contains(connectCell)) return;
        
        GetCellConnections(cell).Add(connectCell);

        if (GetCellConnections(cell).Count > 3)
        {
            GetCellConnections(cell).RemoveAt(0);
        }
    }
    private void SwapCellConnection(Cell cell)
    {
        if (GetCellConnections(cell).Count != 3) return;

        Cell temp = GetCellConnections(cell)[0];
        GetCellConnections(cell)[0] = GetCellConnections(cell)[1];
        GetCellConnections(cell)[1] = temp;
    }
    private void ClearConnectionCell(Cell cell)
    {
        GetCellConnections(cell).Clear();
    }
    private List<Cell> GetCellConnections(Cell cell)
    {
        if (!m_CellConnectDictionary.ContainsKey(cell))
        {
            m_CellConnectDictionary.Add(cell, new List<Cell>());
        }
        return m_CellConnectDictionary[cell];
    }
    private ConnectionCode GetConnectionCode(Cell cell)
    {
        List<int> indexList = new List<int>();
        for (int i = 0; i < GetCellConnections(cell).Count; i++)
        {
            if (GetCellConnections(cell)[i].Coordinates.x < cell.Coordinates.x)
            {
                indexList.Add(1);
            }
            else if (GetCellConnections(cell)[i].Coordinates.y > cell.Coordinates.y)
            {
                indexList.Add(2);
            }
            if (GetCellConnections(cell)[i].Coordinates.x > cell.Coordinates.x)
            {
                indexList.Add(3);
            }
            if (GetCellConnections(cell)[i].Coordinates.y < cell.Coordinates.y)
            {
                indexList.Add(4);
            }
        }
        return new ConnectionCode(indexList);
    }
    private void InitConnectionCell()
    {
        m_CellConnectDictionary.Clear();


        for (int i = 0; i < CellManager.Instance.CurrentHeight; i++)
        {
            for (int j = 0; j < CellManager.Instance.CurrentWidth; j++)
            {
                if (CellManager.Instance.GetCell(j, i).Entity == null) continue;
                if (CellManager.Instance.GetCell(j, i).Entity is DestructibleRail)
                {
                    DestructibleRail destructibleRail = CellManager.Instance.GetCell(j, i).Entity as DestructibleRail;
                    List<Cell> cellList = destructibleRail.GetAllCellCanConnect();
                    for (int k = 0; k < cellList.Count; k++)
                    {
                        AddConnectionCell(CellManager.Instance.GetCell(j, i), cellList[k]);
                    }
                }
            }
        }
    }

    #endregion
}