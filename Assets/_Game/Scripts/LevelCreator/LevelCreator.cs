using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class LevelCreator : Singleton<LevelCreator>
{

    public LevelData m_CurrentEditLevelData;

    #region UI
    public Transform m_MainCanvas;
    public Button m_ButtonPlay;
    public Button m_ButtonLoadStart;
    public Button m_ButtonLoadSolution;
    public Button m_ButtonSaveStart;
    public Button m_ButtonSaveSolution;
    public Button m_ButtonResize;
    public Button m_ButtonNew;
    public TMP_InputField m_InputChap;
    public TMP_InputField m_InputLevel;
    public TMP_InputField m_InputWidth;
    public TMP_InputField m_InputHeight;
    public TMP_InputField m_InputRailCount;
    public UIEditEntity m_UIEditBaseObject;
    public List<ButtonSelectEntity> m_ButtonSelectEntityList;
    #endregion

    #region Unity Functions
    private void Awake()
    {
        for (int i = 0; i < m_ButtonSelectEntityList.Count; i++)
        {
            m_ButtonSelectEntityList[i].Setup(EntityGlobalConfig.Instance.m_EntityConfigList[i], OnClickButtonChooseTile);
        }


        m_ButtonSaveStart.onClick.AddListener(OnClickButtonSaveStart);
        m_ButtonSaveSolution.onClick.AddListener(OnClickButtonSaveSolution);
        m_ButtonPlay.onClick.AddListener(OnClickButtonPlay);
        m_ButtonLoadStart.onClick.AddListener(OnClickButtonLoadStart);
        m_ButtonLoadSolution.onClick.AddListener(OnClickButtonLoadSolution);
        m_ButtonNew.onClick.AddListener(OnClickButtonNew);
        m_ButtonResize.onClick.AddListener(OnClickButtonResize);


        m_InputChap.onValueChanged.AddListener(value => SaveInputText(value, out m_CurrentEditLevelData.m_Chap));
        m_InputLevel.onValueChanged.AddListener(value => SaveInputText(value, out m_CurrentEditLevelData.m_Level));
        m_InputWidth.onValueChanged.AddListener(value => SaveInputText(value, out m_CurrentEditLevelData.m_Width));
        m_InputHeight.onValueChanged.AddListener(value => SaveInputText(value, out m_CurrentEditLevelData.m_Height));
        m_InputRailCount.onValueChanged.AddListener(value => SaveInputText(value, out m_CurrentEditLevelData.m_RailCount));
    }
    private void Start()
    {
        LoadCurrentEditLevel();
    }
    #endregion

    #region Save - Load - Create Functions

    public void LoadCurrentEditLevel()
    {
        m_CurrentEditLevelData = LevelDataGlobalConfig.Instance.CurrentEditLevelData;
        m_CurrentEditLevelData.OnLoadCurrentEditData();
        m_InputChap.text = m_CurrentEditLevelData.m_Chap.ToString();
        m_InputLevel.text = m_CurrentEditLevelData.m_Level.ToString();
        m_InputWidth.text = m_CurrentEditLevelData.m_Width.ToString();
        m_InputHeight.text = m_CurrentEditLevelData.m_Height.ToString();
        m_InputRailCount.text = m_CurrentEditLevelData.m_RailCount.ToString();

        LoadLevelStart();
    }
    public void LoadLevelStart()
    {
        GetValueFromInputField();
        RemoveEntity();
        CreateNewMap();
        LoadStartEntity();

        UpdateAllTunnel();
        UpdateAllPressureRail();
        UpdateLocolmotivePosition();
    }
    private void LoadStartEntity()
    {        
        for (int i = 0; i < m_CurrentEditLevelData.m_Height; i++)
        {
            for (int j = 0; j < m_CurrentEditLevelData.m_Width; j++)
            {
                EntitySaveData entitySaveData = m_CurrentEditLevelData.GetStartObjectInfor(j, i);
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
        for (int i = 0; i < m_CurrentEditLevelData.m_Height; i++)
        {
            for (int j = 0; j < m_CurrentEditLevelData.m_Width; j++)
            {
                EntitySaveData entitySaveData = m_CurrentEditLevelData.GetSolutionObjectInfor(j, i);
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
    public void SaveInputText(string inputText, out int value)
    {
        if (inputText == "") inputText = "1";
        int.TryParse(inputText, out value);

    }
    public void SaveStartEntity()
    {
        m_CurrentEditLevelData.m_StartEntityList.Clear();

        for (int i = 0; i < m_CurrentEditLevelData.m_Height; i++)
        {
            for (int j = 0; j < m_CurrentEditLevelData.m_Width; j++)
            {
                Entity entity = CellManager.Instance.GetCell(j, i).Entity;
                if (entity == null)
                {
                    m_CurrentEditLevelData.m_StartEntityList.Add(new EntitySaveData());
                }
                else
                {
                    m_CurrentEditLevelData.m_StartEntityList.Add(new EntitySaveData(entity));
                }
            }
        }

        m_CurrentEditLevelData.OnSaveData();
    }
    public void SaveSolutionEntity()
    {
        m_CurrentEditLevelData.m_SolutionEntityList.Clear();

        for (int i = 0; i < m_CurrentEditLevelData.m_Height; i++)
        {
            for (int j = 0; j < m_CurrentEditLevelData.m_Width; j++)
            {
                Entity entity = CellManager.Instance.GetCell(j, i).Entity;
                if (entity == null)
                {
                    m_CurrentEditLevelData.m_SolutionEntityList.Add(new EntitySaveData());
                }
                else
                {
                    m_CurrentEditLevelData.m_SolutionEntityList.Add(new EntitySaveData(entity));
                }
            }
        }

        m_CurrentEditLevelData.OnSaveData();
    }
    public void NewLevel()
    {
        GetValueFromInputField();
        RemoveEntity();
        CreateNewMap();
    }
    public void ResizeLevel()
    {
        GetValueFromInputField();
        CreateNewMap();        
    }
    #endregion

    #region UI Button Functions
    public void OnClickButtonSaveStart()
    {
        SaveStartEntity();
    }
    public void OnClickButtonSaveSolution()
    {
        SaveSolutionEntity();
    }
    public void OnClickButtonLoadStart()
    {
        GetValueFromInputField();
        LoadCurrentEditLevelData();
        RemoveEntity();
        CreateNewMap();
        LoadStartEntity();

        UpdateAllTunnel();
        UpdateAllPressureRail();
        UpdateLocolmotivePosition();
    }
    public void OnClickButtonLoadSolution()
    {
        GetValueFromInputField();
        LoadCurrentEditLevelData();
        RemoveEntity();
        CreateNewMap();
        LoadSolutionEntity();

        UpdateAllTunnel();
        UpdateAllPressureRail();
        UpdateLocolmotivePosition();
    }

    public void OnClickButtonPlay()
    {
        UpdateAllTunnel();
        UpdateAllPressureRail();
        UpdateLocolmotivePosition();

        for (int i = 0; i < CellManager.Instance.CurrentHeight; i++)
        {
            for (int j = 0; j < CellManager.Instance.CurrentWidth; j++)
            {
                if (CellManager.Instance.GetCell(j, i).Entity == null) continue;
                if (CellManager.Instance.GetCell(j, i).Entity is CarRail)
                {
                    (CellManager.Instance.GetCell(j, i).Entity as CarRail).m_Wagon.Run();
                }
            }
        }
    }
    public void OnClickButtonNew()
    {
        NewLevel();
    }
    public void OnClickButtonResize()
    {
        ResizeLevel();
    }
    public void OnClickButtonChooseTile(EntityType entityType)
    {
        LevelCreatorInputHandle.Instance.EntityType = LevelCreatorInputHandle.Instance.EntityType == entityType ? EntityType.Null : entityType;

        for (int i = 0; i < m_ButtonSelectEntityList.Count; i++)
        {
            m_ButtonSelectEntityList[i].SetImageOnChoose(LevelCreatorInputHandle.Instance.EntityType);
        }
    }
    public void UpdateAllButonPreviewField()
    {
        for (int i = 0; i < m_ButtonSelectEntityList.Count; i++)
        {
            m_ButtonSelectEntityList[i].UpdatePreviewField();
        }
    }
    #endregion

    #region Map Create Functions
    private void LoadCurrentEditLevelData()
    {
        m_CurrentEditLevelData.OnLoadData(LevelDataGlobalConfig.Instance.GetLevelString(m_CurrentEditLevelData.m_Chap, m_CurrentEditLevelData.m_Level));
        m_InputWidth.text = m_CurrentEditLevelData.m_Width.ToString();
        m_InputHeight.text = m_CurrentEditLevelData.m_Height.ToString();
        m_InputRailCount.text = m_CurrentEditLevelData.m_RailCount.ToString();
    }
    private void GetValueFromInputField()
    {
        if (m_InputChap.text == "") m_InputChap.text = "1";
        m_CurrentEditLevelData.m_Chap = int.Parse(m_InputChap.text);

        if (m_InputLevel.text == "") m_InputLevel.text = "1";
        m_CurrentEditLevelData.m_Level = int.Parse(m_InputLevel.text);

        if (m_InputWidth.text == "") m_InputWidth.text = "1";
        m_CurrentEditLevelData.m_Width = int.Parse(m_InputWidth.text);

        if (m_InputHeight.text == "") m_InputHeight.text = "1";
        m_CurrentEditLevelData.m_Height = int.Parse(m_InputHeight.text);

        if (m_InputRailCount.text == "") m_InputRailCount.text = "1";
        m_CurrentEditLevelData.m_RailCount = int.Parse(m_InputRailCount.text);
    }
    private void CreateNewMap()
    {
        CellManager.Instance.CreateMap(m_CurrentEditLevelData.m_Width, m_CurrentEditLevelData.m_Height);
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
    public void ChangeEntity(EntityType entityType)
    {
        EntityGlobalConfig.Instance.m_EntityConfigList.FirstOrDefault(e => e.EntityType == entityType).IncreaseIndex();

        UpdateAllButonPreviewField();
    }
    public void CreateEntity(Entity entity , Cell cell, Quaternion rotation)
    {
        if (entity == null) return;
        Entity tempEntity = Instantiate(entity);
        cell.AddEnity(tempEntity, rotation);
    }
    public void RemoveCurrentEntity(Cell cell)
    {
        cell.RemoveCurrentEnity();
    }
    public void EditSelectEntity(Cell cell)
    {
        cell.Entity.EditSelectEntity();
    }
    #endregion

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
}
[System.Serializable]
public class BaseObjectTile
{
    public EntityType m_BaseObjectTileType;
    public int m_CurrentIndex;

    public List<GameObject> m_Prefabs;
    public List<Sprite> m_Sprites;

    public GameObject GetCurrentPrefab()
    {
        if (m_Prefabs == null || m_Prefabs.Count == 0) return null;
        return m_Prefabs[m_CurrentIndex % m_Prefabs.Count];
    }
    public GameObject GetPrefab(int index)
    {
        if (m_Prefabs == null || m_Prefabs.Count == 0) return null;
        return m_Prefabs[index];
    }
    public Sprite GetCurrentSprite()
    {
        if (m_Sprites == null || m_Sprites.Count == 0) return null;
        return m_Sprites[m_CurrentIndex % m_Sprites.Count];
    }
    public void IncreaseIndex()
    {
        m_CurrentIndex++;
    }
}

