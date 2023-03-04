using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Linq;

/// <summary>
/// Load Level From Level Creator Scene, Need Setup First
/// </summary>
public class LevelManager : Singleton<LevelManager>
{
    private LevelData m_CurrentEditLevelData;
    private GameObject[,] m_AllDefaultTile;
    public GameObject[,] m_AllBaseObject;
    private List<GameObject> m_AllLine;

    [Header("Level Data")]
    public Transform m_RootMap;
    public int m_Chap;
    public int m_Level;
    public int m_Width;
    public int m_Height;
    public int m_RailCount;

    [Header("BaseObjects")]
    public GameObject m_DefauleTile;
    public GameObject m_Line;
    public List<BaseObjectTile> m_BaseObjectTileList;
    public List<GameObject> m_StateTileObjectList;

    public MapData m_MapData;

    private void Awake()
    {
        m_CurrentEditLevelData = new LevelData();
    }
    [Button]
    public void StartWagon()
    {
        // TODO: Optimize later 
        Wagon[] AllWagon = FindObjectsOfType<Wagon>();
        foreach (Wagon wagon in AllWagon)
        {
            if (wagon.m_WagonID != -1)
            {
                wagon.Run();
            }
        }
    }
    [Button]
    public void LoadLevelStart(int chapter, int level)
    {
        m_CurrentEditLevelData.OnLoadData(LevelDataConfig.Instance.GetLevelString(chapter, level));

        LoadLevelData();
        RemoveDefaultTile();
        RemoveAllBaseObject();
        CreateDefaultTile();
        CreateStartBaseObject();
        UpdateAllTunnel();
        UpdateAllPressureRail();
        UpdateLocolmotivePosition();
    }
    [Button]
    public void LoadLevelSolution(int chapter, int level)
    {
        m_CurrentEditLevelData.OnLoadData(LevelDataConfig.Instance.GetLevelString(chapter, level));

        LoadLevelData();
        RemoveDefaultTile();
        RemoveAllBaseObject();
        CreateDefaultTile();
        CreateSolutionBaseObject();
        UpdateAllTunnel();
        UpdateAllPressureRail();
        UpdateLocolmotivePosition();
    }
    private void LoadLevelData()
    {
        m_Chap = m_CurrentEditLevelData.m_Chap;
        m_Level = m_CurrentEditLevelData.m_Level;
        m_Width = m_CurrentEditLevelData.m_Width;
        m_Height = m_CurrentEditLevelData.m_Height;
        m_RailCount = m_CurrentEditLevelData.m_RailCount;

        m_MapData = new MapData();
        m_MapData.Init(m_Level, m_RailCount);
    }
    private void CreateDefaultTile()
    {
        m_AllDefaultTile = new GameObject[m_Height, m_Width];
        if (m_AllLine == null) m_AllLine = new List<GameObject>();

        for (int i = 0; i < m_Height; i++)
        {
            for (int j = 0; j < m_Width; j++)
            {
                GameObject go = Instantiate(m_DefauleTile, new Vector3(i, 0, j), Quaternion.identity, m_RootMap);
                m_AllDefaultTile[i, j] = go;
                DefaultTile defaultTile = go.GetComponent<DefaultTile>();
                defaultTile.UpdateTileOwner();
                defaultTile.m_Tile.Setup(j, i);
            }
        }

        for (int i = -1; i < m_Height; i++)
        {
            GameObject line = Instantiate(m_Line, new Vector3(i + 0.5f, 0, m_Width / 2.0f - 0.5f), Quaternion.identity, m_RootMap);
            line.transform.localScale = new Vector3(1, 1, m_Width * 20);
            m_AllLine.Add(line);
        }
        for (int i = -1; i < m_Width; i++)
        {
            GameObject line = Instantiate(m_Line, new Vector3(m_Height / 2.0f - 0.5f, 0, i + 0.5f), Quaternion.identity, m_RootMap);
            line.transform.localScale = new Vector3(m_Height * 20, 1, 1);
            m_AllLine.Add(line);
        }
    }
    private void CreateStartBaseObject()
    {
        //m_AllBaseObject = new GameObject[m_Height, m_Width];

        //for (int i = 0; i < m_CurrentEditLevelData.m_Height; i++)
        //{
        //    for (int j = 0; j < m_CurrentEditLevelData.m_Width; j++)
        //    {
        //        BaseObjectInfor baseObjectInfor = m_CurrentEditLevelData.m_StartEntityList[i * m_CurrentEditLevelData.m_Width + j];
        //        if (baseObjectInfor.m_BaseObjectTileType != EntityType.Null)
        //        {
        //            GameObject prefab = GetBaseObject(baseObjectInfor.m_BaseObjectTileType, baseObjectInfor.m_Index);
        //            GameObject go = Instantiate(prefab, new Vector3(i, 0, j), baseObjectInfor.m_Rotation, m_RootMap);
        //            go.name = prefab.name + "_" + j + "_" + i;
                    
        //            BaseObject baseObject = go.GetComponent<BaseObject>();
        //            baseObject.m_ObjectID = baseObjectInfor.m_GroupID;
        //            baseObject.m_Position = baseObjectInfor.m_Position;
        //            baseObject.OnObjectIDChange();
        //            TileDirection tileDirection = Static.GetTileDirection( baseObject.m_BaseObjectTileType, baseObject.DesRailType, baseObjectInfor.m_Rotation.eulerAngles.y);
        //            Debug.Log("Create " + go.name + " " + baseObjectInfor.m_Rotation.eulerAngles + " direction " + tileDirection + " type " + baseObject.DesRailType);
        //            baseObject.UpdateTileOwner();
        //            baseObject.TileDirection = tileDirection;
        //            baseObject.m_Tile.Setup(j, i);
        //            baseObject.UpdateOutput();
        //            m_AllBaseObject[i, j] = go;
        //            if(baseObject.m_BaseObjectTileType == EntityType.DestructionRail) {
        //                IngameManager.Instance.AddRailToList(go);
        //            }
        //        }
        //    }
        //}
        //for(int i = 0; i < m_AllBaseObject.GetLength(0); i++) {
        //    for(int j = 0; j < m_AllBaseObject.GetLength(1); j++) {
        //        GameObject go = m_AllBaseObject[i, j];
        //        if(go == null) continue;
        //        BaseObject bo = go.GetComponent<BaseObject>();
        //        if (bo.m_BaseObjectTileType == EntityType.DestructionRail) {
        //            bo.AutoMatching();
        //        }
        //    }

        //}
    }
    private void CreateSolutionBaseObject()
    {
        //m_AllBaseObject = new GameObject[m_Height, m_Width];

        //for (int i = 0; i < m_CurrentEditLevelData.m_Height; i++)
        //{
        //    for (int j = 0; j < m_CurrentEditLevelData.m_Width; j++)
        //    {
        //        BaseObjectInfor baseObjectInfor = m_CurrentEditLevelData.m_SolutionEntityList[i * m_CurrentEditLevelData.m_Width + j];
        //        if (baseObjectInfor.m_BaseObjectTileType != EntityType.Null)
        //        {
        //            GameObject go = Instantiate(GetBaseObject(baseObjectInfor.m_BaseObjectTileType, baseObjectInfor.m_Index), new Vector3(i, 0, j), baseObjectInfor.m_Rotation, m_RootMap);
        //            BaseObject baseObject = go.GetComponent<BaseObject>();
        //            baseObject.m_ObjectID = baseObjectInfor.m_GroupID;
        //            baseObject.m_Position = baseObjectInfor.m_Position;
        //            baseObject.OnObjectIDChange();
        //            m_AllBaseObject[i, j] = go;
        //        }
        //    }
        //}
    }
    private void RemoveDefaultTile()
    {
        if (m_AllDefaultTile == null || m_AllLine == null) return;
        foreach (GameObject go in m_AllDefaultTile)
        {
            if (go != null) SimplePool.Despawn(go);
        }
        m_AllDefaultTile = null;

        foreach (GameObject go in m_AllLine)
        {
            if (go != null) SimplePool.Despawn(go);
        }
        m_AllLine.Clear();
    }
    private void RemoveAllBaseObject()
    {
        if (m_AllBaseObject == null) return;
        foreach (GameObject go in m_AllBaseObject)
        {
            if (go != null) {
                SimplePool.Despawn(go);
            }
        }
        m_AllBaseObject = null;
    }
    private GameObject GetBaseObject(EntityType baseObjectTileType, int index)
    {
        for (int i = 0; i < m_BaseObjectTileList.Count; i++)
        {
            if (m_BaseObjectTileList[i].m_BaseObjectTileType == baseObjectTileType)
            {
                return m_BaseObjectTileList[i].GetPrefab(index);
            }
        }
        return null;
    }
    private void UpdateAllTunnel()
    {
        // TODO: Optimize later
        Tunnel[] allTunnel = FindObjectsOfType<Tunnel>();
        List<Tunnel> tunnels = allTunnel.OrderBy(value => value.GroupID).ToList();
        for (int i = 0; i < tunnels.Count; i += 2)
        {
            tunnels[i].m_OtherTunnel = tunnels[i + 1];
            tunnels[i + 1].m_OtherTunnel = tunnels[i];
        }
    }
    private void UpdateAllPressureRail()
    {
        // TODO: Optimize later
        PressureRail[] allPressureRail = FindObjectsOfType<PressureRail>();
        for (int i = 0; i < allPressureRail.Length; i++)
        {
            allPressureRail[i].GetDynamicRail();
        }
    }
    private void UpdateLocolmotivePosition() {
        LocolmotiveRail locolmotiveRail = null;
        int wagonCount = 0;
        for (int i = 0; i < m_AllBaseObject.GetLength(0); i++) {
            for (int j = 0; j < m_AllBaseObject.GetLength(1); j++) {
                if (m_AllBaseObject[i, j] != null && m_AllBaseObject[i, j].GetComponent<BaseObject>() is LocolmotiveRail) {
                    locolmotiveRail = m_AllBaseObject[i, j].GetComponent<LocolmotiveRail>();
                } else if (m_AllBaseObject[i, j] != null && m_AllBaseObject[i, j].GetComponent<BaseObject>() is CarRail && m_AllBaseObject[i, j].GetComponent<CarRail>().GroupID > 0) {
                    wagonCount++;
                }
            }
        }

        locolmotiveRail?.UpdateLocolmotivePosition(wagonCount);
    }
    public void AddBaseObject(BaseObject go, int x, int y) {
        //Debug.Log("ADD " + go.name);
        go.m_Position = new Vector3 (y, 0, x);
        m_AllBaseObject[y, x] = go.gameObject;
    }
    public void RemoveBaseObject(int width, int height) {
        if (width >= 0 && width < m_Width && height >= 0 && height < m_Height) {
            m_AllBaseObject[height, width] = null;
        }
    }
    public BaseObject GetBaseObject(int x, int y) {
        int width = x;
        int height = y;
        if (width >= 0 && width < m_Width && height >= 0 && height < m_Height) {
            return m_AllBaseObject[y, x].GetComponent<BaseObject>();
        }
        return null;
    }
    public List<BaseObject> GetAllAround(int x, int y) {
        List<BaseObject> list = new List<BaseObject>();
        {
            //Left;
            int width = x - 1;
            int height = y;
            if (width >= 0 && width < m_Width && height >= 0 && height < m_Height) {
                if (m_AllBaseObject[height,width] != null) {
                    list.Add(m_AllBaseObject[height, width].GetComponent<BaseObject>());
                } else {
                    list.Add(null);
                }
            } else {
                list.Add(null);
            }
        }
        {
            //Right
            int width = x + 1;
            int height = y;
            if (width >= 0 && width < m_Width && height >= 0 && height < m_Height) {
                if (m_AllBaseObject[height, width] != null) {
                    list.Add(m_AllBaseObject[height, width].GetComponent<BaseObject>());
                } else {
                    list.Add(null);
                }
            } else {
                list.Add(null);
            }
        }
        {
            // Up
            int width = x;
            int height = y-1;
            if (width >= 0 && width < m_Width && height >= 0 && height < m_Height) {
                if (m_AllBaseObject[height, width] != null) {
                    list.Add(m_AllBaseObject[height, width].GetComponent<BaseObject>());
                } else {
                    list.Add(null);
                }
            } else {
                list.Add(null);
            }
        }
        {
            //Down
            int width = x;
            int height = y+1;
            if (width >= 0 && width < m_Width && height >= 0 && height < m_Height) {
                if (m_AllBaseObject[height, width] != null) {
                    list.Add(m_AllBaseObject[height, width].GetComponent<BaseObject>());
                } else {
                    list.Add(null);
                }
            } else {
                list.Add(null);
            }
            
        }
        return list;
    }
    public BaseObject GetNearBaseObject(int x, int y, TileDirection position) {
        switch (position) {
            case TileDirection.Left: {
                    int width = x + 1;
                    int height = y;
                    if (width >= 0 && width < m_Width && height >= 0 && height < m_Height) {
                        if (m_AllBaseObject[height, width] != null) {
                            return m_AllBaseObject[height, width].GetComponent<BaseObject>();
                        } else {
                            return null;
                        }
                    } else {
                        return null;
                    }
                }
            case TileDirection.Right: {
                    int width = x - 1;
                    int height = y;
                    if (width >= 0 && width < m_Width && height >= 0 && height < m_Height) {
                        if (m_AllBaseObject[height, width] != null) {
                            return m_AllBaseObject[height, width].GetComponent<BaseObject>();
                        } else {
                            return null;
                        }
                    } else {
                        return null;
                    }
                }
            case TileDirection.Up: {
                    int width = x;
                    int height = y + 1;
                    if (width >= 0 && width < m_Width && height >= 0 && height < m_Height) {
                        if (m_AllBaseObject[height, width] != null) {
                            return m_AllBaseObject[height, width].GetComponent<BaseObject>();
                        } else {
                            return null;
                        }
                    } else {
                        return null;
                    }
                }
            case TileDirection.Down: {
                    int width = x;
                    int height = y - 1;
                    if (width >= 0 && width < m_Width && height >= 0 && height < m_Height) {
                        if (m_AllBaseObject[height, width] != null) {
                            return m_AllBaseObject[height, width].GetComponent<BaseObject>();
                        } else {
                            return null;
                        }
                    } else {
                        return null;
                    }
                }
        }
        return null;
    }
    public void MarkUpdateAround(bool isUpdate) {
        for(int i = 0; i < m_Width; i++) {
            for(int j = 0; j < m_Height; j++) {
                GameObject go = m_AllBaseObject[j, i];
                if (go != null) {
                    BaseObject bo = go.GetComponent<BaseObject>();
                    if (bo != null) {
                        bo.IsUpdatedAround = isUpdate;
                    }
                }
            }
        }
    }
    public void UpdateAround() {
        for (int i = 0; i < m_Width; i++) {
            for (int j = 0; j < m_Height; j++) {
                GameObject go = m_AllBaseObject[j, i];
                if (go != null) {
                    BaseObject bo = go.GetComponent<BaseObject>();
                    if (bo != null && !bo.IsUpdatedAround) {
                        bo.UpdateAround();
                    }
                }
            }
        }
    }

#if UNITY_EDITOR
    ///// <summary>
    ///// Only use in Level Creator Scene
    ///// </summary>
    //[ShowIf("IsOnLevelCreatorScene")]
    //[Button]
    //private void Setup()
    //{
    //    m_DefauleTile = LevelCreator.Instance.m_DefauleTile;
    //    m_Line = LevelCreator.Instance.m_Line;
    //    m_BaseObjectTileList = LevelCreator.Instance.m_BaseObjectTileList;
    //    m_StateTileObjectList = LevelCreator.Instance.m_StateTileObjectList;
    //}
    public bool IsOnLevelCreatorScene()
    {
        return UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "LevelCreatorScene";
    }
#endif

}
