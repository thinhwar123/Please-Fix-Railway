using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Sirenix.Utilities;

[CreateAssetMenu(fileName = "ChapterGlobalConfig", menuName = "ScriptableObjects/GlobalConfig/ChapterGlobalConfig")]
[GlobalConfig("Assets/Resources/GlobalConfig/")]
public class ChapterGlobalConfig : GlobalConfig<ChapterGlobalConfig>
{
    public List<ChapterConfig> chapterConfigs = new List<ChapterConfig>();
    public ChapterConfig GetChapterConfig(int chap)
    {
        for (int i = 0; i < chapterConfigs.Count; i++)
        {
            ChapterConfig chapterConfig = chapterConfigs[i];
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
}