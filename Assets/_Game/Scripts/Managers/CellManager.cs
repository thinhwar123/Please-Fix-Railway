using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using MyExtension;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class CellManager : Singleton<CellManager>
{
    private Transform m_Transform;
    public Transform Transform { get => m_Transform ??= transform; }
    [SerializeField] private Transform m_CellTransform;
    [SerializeField] private Transform m_EntityTransform;
    [SerializeField] private Cell m_CellPrefab;
    [SerializeField] private List<Cell> m_CellList = new List<Cell>();

    [SerializeField] private int m_CurrentWidth;
    [SerializeField] private int m_CurrentHeight;
    [SerializeField] private int m_OffsetWidth;
    [SerializeField] private int m_OffsetHeight;

    public Transform CellTransform { get => m_CellTransform; }
    public Transform EntityTransform { get => m_EntityTransform; }
    public int CurrentWidth { get => m_CurrentWidth; }
    public int CurrentHeight { get => m_CurrentHeight; }
    public List<Cell> CellList { get => m_CellList; }


    [Button]
    public void CreateMap(int width, int height)
    {
        m_CurrentWidth = width;
        m_CurrentHeight = height;
        m_OffsetWidth = (10 - width) / 2;
        m_OffsetHeight = (10 - height) / 2;
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                m_CellList[j + i * 10].SetCanDrawEntity(m_OffsetHeight <= i && i < height + m_OffsetHeight && m_OffsetWidth <= j && j < m_OffsetWidth + width);
                //m_CellList[j + i * 10].SetCanDrawEntity(i < height && j < width);
            }
        }
    }
    public Cell GetCell(int x, int y)
    {
        //if (x + m_OffsetWidth < 0 || y + m_OffsetHeight < 0 || x + m_OffsetWidth > 9 || y + m_OffsetHeight > 9) return null;
        //return m_CellList[(x + m_OffsetWidth) + (y + m_OffsetHeight) * 10];

        if (x < 0 || y < 0 || x > 9 || y > 9) return null;
        return m_CellList[x + y * 10];
    }
    public Cell GetCell(Coordinates coordinates)
    {
        return GetCell(coordinates.x, coordinates.y);
    }
    //public Cell GetCellIgnoreOffset(int x, int y)
    //{
    //    if (x  < 0 || y  < 0 || x  > 9 || y  > 9) return null;
    //    return m_CellList[x + y * 10];
    //}

    #region Editor

#if UNITY_EDITOR
    [Button]
    private void InitCell()
    {
        for (int i = 0; i < m_CellList.Count; i++)
        {
            if (m_CellList[i] != null)
            {
                DestroyImmediate(m_CellList[i].gameObject);
            }
        }
        m_CellList.Clear();

        m_CellTransform = transform.FindChildOrCreate("Cell");
        m_EntityTransform = transform.FindChildOrCreate("Enity");

        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                Cell tempCell = PrefabUtility.InstantiatePrefab(m_CellPrefab, m_CellTransform) as Cell;
                EditorUtility.SetDirty(tempCell);
                tempCell.name = $"Cell[{i},{j}]";
                tempCell.Coordinates.x = j;
                tempCell.Coordinates.y = i;

                tempCell.transform.localPosition = new Vector3(tempCell.Coordinates.x, 0, tempCell.Coordinates.y);
                tempCell.gameObject.SetActive(false);
                m_CellList.Add(tempCell);
            }
        }
    }
#endif
    #endregion

}
