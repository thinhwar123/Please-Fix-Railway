using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : Singleton<GameData> {
    public ChapterGlobalConfig m_ChapterConfigs;
    private void Awake() {
        DontDestroyOnLoad(gameObject);
        if(m_Instance != null) {
            DestroyImmediate(gameObject);
        }
    }
    public ChapterConfig GetChapterConfig(int chapterID) {
        return m_ChapterConfigs.GetChapterConfig(chapterID);
    }
}