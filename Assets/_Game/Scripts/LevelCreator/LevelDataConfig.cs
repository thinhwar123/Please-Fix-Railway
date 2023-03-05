using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif
[CreateAssetMenu(fileName = "LevelDataConfig", menuName = "ScriptableObjects/GlobalConfig/LevelDataConfig")]
[GlobalConfig("Assets/Resources/GlobalConfig/")]
public class LevelDataConfig : GlobalConfig<LevelDataConfig>
{
    [ReadOnly]
    [SerializeField] private TextAsset[] m_LevelDatas;


    public string GetLevelString(int chap, int level)
    {
#if UNITY_EDITOR
        UpdateLevelDataConfig();
#endif
        TextAsset textAsset = m_LevelDatas.FirstOrDefault(textAsset => textAsset.name == string.Format("Data_Chap{0}_Level{1}", chap, level));
        if (textAsset == null) return "";
        return textAsset.text;


    }

    #region Editor
#if UNITY_EDITOR
    [ReadOnly]
    [SerializeField] private LevelData m_CurrentEditLevelData;

    public LevelData CurrentEditLevelData
    {
        get
        {
            return m_CurrentEditLevelData ??= new LevelData();
        }
    }


    [Button(ButtonSizes.Medium), PropertyOrder(-1)]
    public void UpdateLevelDataConfig()
    {
        EditorUtility.SetDirty(this);
        this.m_LevelDatas = AssetDatabase.FindAssets("t:TextAsset ", new string[] { "Assets/_Game/Datas/LevelDatas" })
            .Select(guid => AssetDatabase.LoadAssetAtPath<TextAsset>(AssetDatabase.GUIDToAssetPath(guid)))
            .ToArray();
        this.m_LevelDatas.Sort((x, y) => x.name.CompareTo(y.name));

        AssetDatabase.Refresh();
        AssetDatabase.SaveAssets();
    }
#endif
    #endregion

}
