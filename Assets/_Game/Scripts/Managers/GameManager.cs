using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Linq;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private int m_CurrentChap;
    [SerializeField] private int m_CurrentLevel;
    [SerializeField] private int m_CurrentRailCount;
    [SerializeField] private ChapterConfig m_CurrentChapterConfig;
    [SerializeField] private LevelData m_CurrentLevelData;

    public int CurrentRailCount { get => m_CurrentRailCount; set => m_CurrentRailCount = value; }

    #region Unity Functions
    private void Start()
    {
        LoadLevel(1, 10);
    } 
    #endregion

    #region Load Functions
    [Button]
    public void LoadLevel(int chap, int level)
    {
        m_CurrentChapterConfig = ChapterGlobalConfig.Instance.GetChapterConfig(chap);
        m_CurrentChap = chap;
        m_CurrentLevel = level;

        m_CurrentLevelData = new LevelData(LevelDataGlobalConfig.Instance.GetLevelString(chap, level));
        m_CurrentRailCount = m_CurrentLevelData.m_RailCount;
        RemoveEntity();
        CreateNewMap();
        LoadStartEntity();

        UpdateAllTunnel();
        UpdateAllPressureRail();
        UpdateLocolmotivePosition();
    }
    [Button]
    public void LoadNextLevel()
    {
        if (m_CurrentChap > 0 && m_CurrentLevel < m_CurrentChapterConfig.normalLevel)
        {
            LoadLevel(m_CurrentChap, m_CurrentLevel + 1);
        }
        else if (m_CurrentChap > 0 && m_CurrentLevel == m_CurrentChapterConfig.normalLevel)
        {
            LoadLevel(-m_CurrentChap, 1);
        }
        else if (m_CurrentChap < 0 && m_CurrentLevel < m_CurrentChapterConfig.hardLevel)
        {
            LoadLevel(m_CurrentChap, m_CurrentLevel + 1);
        }
        else if (ChapterGlobalConfig.Instance.GetChapterConfig(Mathf.Abs(m_CurrentChap) + 1) != null)
        {
            LoadLevel(Mathf.Abs(m_CurrentChap) + 1, 1);
        }
    } 
    #endregion

    #region Map Create Functions
    private void CreateNewMap()
    {
        CellManager.Instance.CreateMap(m_CurrentLevelData.m_Width, m_CurrentLevelData.m_Height);
    }
    public void RemoveEntity()
    {
        for (int i = 0; i < CellManager.Instance.CurrentHeight; i++)
        {
            for (int j = 0; j < CellManager.Instance.CurrentWidth; j++)
            {
                CellManager.Instance.GetCell(j, i).RemoveCurrentEnity();
            }
        }
    }
    private void LoadStartEntity()
    {
        for (int i = 0; i < m_CurrentLevelData.m_Height; i++)
        {
            for (int j = 0; j < m_CurrentLevelData.m_Width; j++)
            {
                EntitySaveData entitySaveData = m_CurrentLevelData.GetStartObjectInfor(j, i);
                if (entitySaveData.m_EntityType != EntityType.Null)
                {
                    Cell cell = CellManager.Instance.GetCell(entitySaveData.m_Coordinates);
                    Entity entity = Instantiate(EntityGlobalConfig.Instance.GetEntity(entitySaveData.m_EntityType, entitySaveData.m_Index));
                    entity.GroupID = entitySaveData.m_GroupID;
                    cell.AddEnity(entity, entitySaveData.m_Rotation);
                }
            }
        }
    }
    public void LoadSolutionEntity()
    {
        for (int i = 0; i < m_CurrentLevelData.m_Height; i++)
        {
            for (int j = 0; j < m_CurrentLevelData.m_Width; j++)
            {
                EntitySaveData entitySaveData = m_CurrentLevelData.GetSolutionObjectInfor(j, i);
                if (entitySaveData.m_EntityType != EntityType.Null)
                {
                    Cell cell = CellManager.Instance.GetCell(entitySaveData.m_Coordinates);
                    Entity entity = Instantiate(EntityGlobalConfig.Instance.GetEntity(entitySaveData.m_EntityType, entitySaveData.m_Index));
                    entity.GroupID = entitySaveData.m_GroupID;
                    cell.AddEnity(entity, entitySaveData.m_Rotation);
                }
            }
        }
    }
    #endregion

    #region Update Entity Functions

    public void UpdateAllTunnel()
    {
        List<Tunnel> tunnelList = new List<Tunnel>();

        for (int i = 0; i < CellManager.Instance.CurrentHeight; i++)
        {
            for (int j = 0; j < CellManager.Instance.CurrentWidth; j++)
            {
                if (CellManager.Instance.GetCell(j, i).Entity == null) continue;
                if (CellManager.Instance.GetCell(j, i).Entity is Tunnel)
                {
                    tunnelList.Add(CellManager.Instance.GetCell(j, i).Entity as Tunnel);

                }
            }
        }

        List<Tunnel> tunnels = tunnelList.OrderBy(value => value.GroupID).ToList();
        for (int i = 0; i < tunnels.Count; i += 2)
        {
            tunnels[i].m_OtherTunnel = tunnels[Mathf.Clamp(i + 1, 0, tunnels.Count - 1)];
            tunnels[Mathf.Clamp(i + 1, 0, tunnels.Count - 1)].m_OtherTunnel = tunnels[i];
        }
    }
    public void UpdateAllPressureRail()
    {
        for (int i = 0; i < CellManager.Instance.CurrentHeight; i++)
        {
            for (int j = 0; j < CellManager.Instance.CurrentWidth; j++)
            {
                if (CellManager.Instance.GetCell(j, i).Entity == null) continue;
                if (CellManager.Instance.GetCell(j, i).Entity is PressureRail)
                {
                    (CellManager.Instance.GetCell(j, i).Entity as PressureRail).GetDynamicRail();
                }
            }
        }
    }
    public void UpdateLocolmotivePosition()
    {
        LocolmotiveRail locolmotiveRail = null;
        int wagonCount = 0;
        for (int i = 0; i < CellManager.Instance.CurrentHeight; i++)
        {
            for (int j = 0; j < CellManager.Instance.CurrentWidth; j++)
            {
                if (CellManager.Instance.GetCell(j, i).Entity == null) continue;
                if (CellManager.Instance.GetCell(j, i).Entity is LocolmotiveRail)
                {
                    locolmotiveRail = CellManager.Instance.GetCell(j, i).Entity as LocolmotiveRail;
                }
                else if (CellManager.Instance.GetCell(j, i).Entity is CarRail && CellManager.Instance.GetCell(j, i).Entity.GroupID > 0)
                {
                    wagonCount++;
                }
            }
        }

        locolmotiveRail?.UpdateLocolmotivePosition(wagonCount);
    } 
    #endregion

}
