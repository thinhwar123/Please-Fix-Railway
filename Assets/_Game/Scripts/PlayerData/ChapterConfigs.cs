using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "ChapterConfig_", menuName = "ScriptableObjects/ChapterConfig", order = 1)]
public class ChapterConfigs : ScriptableObject {
    public List<ChapterConfig> chapterConfigs = new List<ChapterConfig>();
    public ChapterConfig GetChapterConfig(int chapterID) {
        for(int i = 0; i < chapterConfigs.Count; i++) {
            ChapterConfig chapterConfig = chapterConfigs[i];
            if(chapterConfig.chapterID == chapterID) {
                return chapterConfig;
            }
        }
        return null;
    }
}
[System.Serializable]
public class ChapterConfig {
    public int chapterID;
    public int maxLevel;
    public int superMaxLevel;
}