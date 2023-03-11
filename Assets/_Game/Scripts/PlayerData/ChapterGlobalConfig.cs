using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Sirenix.Utilities;

[CreateAssetMenu(fileName = "ChapterGlobalConfig", menuName = "ScriptableObjects/GlobalConfig/ChapterGlobalConfig")]
[GlobalConfig("Assets/Resources/GlobalConfig/")]
public class ChapterGlobalConfig : GlobalConfig<ChapterGlobalConfig>
{
    public List<ChapterConfig> m_ChapterConfigs = new List<ChapterConfig>();
    public ChapterConfig GetChapterConfig(int chap)
    {
        for (int i = 0; i < m_ChapterConfigs.Count; i++)
        {
            ChapterConfig chapterConfig = m_ChapterConfigs[i];
            if (chapterConfig.chap == Mathf.Abs(chap))
            {
                return chapterConfig;
            }
        }
        return null;
    }
}
[System.Serializable]
public class ChapterConfig
{
    public int chap;
    public int normalLevel;
    public int hardLevel;

    public string chapterName;
}