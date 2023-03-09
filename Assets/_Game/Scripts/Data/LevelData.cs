using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
#if UNITY_EDITOR
using UnityEditor;
#endif

[System.Serializable]
public class LevelData
{
    public int m_Chap;
    public int m_Level;

    public int m_Width;
    public int m_Height;
    public int m_RailCount;

    public List<EntitySaveData> m_StartEntityList;
    public List<EntitySaveData> m_SolutionEntityList;
    
    public LevelData()
    {

    }
    public LevelData(string json)
    {
        if (string.Compare(json, "") == 0)
        {
            OnInitNewData();
        }
        else
        {
            ConvertJsonToObject(json);
        }
    }

    public EntitySaveData GetStartObjectInfor(int x, int y)
    {
        return m_StartEntityList[x + y * m_Width];
    }
    public EntitySaveData GetSolutionObjectInfor(int x, int y)
    {
        return m_SolutionEntityList[x + y * m_Width];
    }
    public virtual void OnInitNewData()
    {
        m_Width = 5;
        m_Height = 5;
        m_RailCount = 10;

        m_StartEntityList = new List<EntitySaveData>();
        m_SolutionEntityList = new List<EntitySaveData>();
        for (int i = 0; i < 25 ; i++)
        {
            m_StartEntityList.Add(new EntitySaveData());
            m_SolutionEntityList.Add(new EntitySaveData());
        }
    }
    private string ConvertObjectToJson()
    {
        return JsonUtility.ToJson(this);
    }
    private void ConvertJsonToObject(string json)
    {
        JsonUtility.FromJsonOverwrite(json, this);
    }
    public void OnLoadData(string json)
    {
        if (string.Compare(json, "") == 0)
        {
            OnInitNewData();
        }
        else
        {
            ConvertJsonToObject(json);
        }
    }
    public void OnLoadCurrentEditData()
    {
        OnLoadData(m_Chap, m_Level);
    }
    public void OnLoadData(int chap, int level)
    {
        string json = EditorSaveSystem.ReadFromFile(string.Format($"Data_Chap{chap}_Level{level}.txt"));
        if (string.Compare(json, "") == 0)
        {
            m_Chap = 0;
            m_Level = 0;
            OnInitNewData();
        }
        else
        {
            ConvertJsonToObject(json);
        }
    }
    public void OnSaveData()
    {
        if (m_Chap != 0 && m_Level != 0)
        {
            EditorSaveSystem.WriteToFile(string.Format($"Data_Chap{m_Chap}_Level{m_Level}.txt"), ConvertObjectToJson());
        }
        EditorSaveSystem.WriteToFile(string.Format("Data_Chap{0}_Level{1}.txt", 0, 0), ConvertObjectToJson());

        AssetDatabase.Refresh();
        AssetDatabase.SaveAssets();
    }


}
[System.Serializable]
public class BaseObjectInfor
{
    public EntityType m_BaseObjectTileType;
    public int m_Index;
    public int m_GroupID;
    public Quaternion m_Rotation;
    public Vector3 m_Position;
    public BaseObjectInfor(EntityType baseObjectTileType = EntityType.Null, int index = 0, int groupID = 0, Quaternion rotation = default, Vector3 position = default)
    {
        m_BaseObjectTileType = baseObjectTileType;
        m_Index = index;
        m_GroupID = groupID;
        m_Rotation = rotation;
        m_Position = position;
    }
}