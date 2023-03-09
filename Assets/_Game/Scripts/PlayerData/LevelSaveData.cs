using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LevelSaveDatas {
    public string key;
    public List<ChapterSaveData> chapterSaveDatas = new List<ChapterSaveData>();
    public int selectedLevel;
    public int selectedChapterID = 1;
    public LevelSaveDatas(string key) {
        this.key = key;
    }
    
    public void Save() {
        string s = "";
        for(int i = 0; i < chapterSaveDatas.Count; i++) {
            ChapterSaveData chapterSaveData = chapterSaveDatas[i];
            s += chapterSaveData.chapterID + "," + chapterSaveData.passedLevel + "," + chapterSaveData.superLevelPassed + "|";
        }
        s = s.Substring(0,s.Length-1);
        PlayerPrefs.SetString(key, s);
    }
    public void Load() {
        selectedChapterID = 1;
        string s = PlayerPrefs.GetString(key, "");
        if (string.IsNullOrEmpty(s)) {
            ChapterSaveData chapterSaveData = new ChapterSaveData {
                chapterID = 1,
                passedLevel = 0,
                superLevelPassed = 0,
            };
            chapterSaveDatas.Add(chapterSaveData);
            Save();
        } else {
            string[] s1 = s.Split('|');
            for(int i = 0; i < s1.Length; i++) {
                string[] s2 = s1[i].Split(",");
                ChapterSaveData chapterSaveData = new ChapterSaveData {
                    chapterID = int.Parse(s2[0]),
                    passedLevel = int.Parse(s2[1]),
                    superLevelPassed = int.Parse(s2[2]),
                };
                chapterSaveDatas.Add(chapterSaveData);
            }
        }
    }
    public void SelectLevel(int level, int chapterID) {
        selectedChapterID = chapterID;
        selectedLevel = level;
    }
    public void LevelUp() {
        int passedLevel = LevelUp(selectedChapterID);
        int chapterMaxLevel = GameData.Instance.GetChapterConfig(selectedChapterID).normalLevel;
        if(passedLevel == chapterMaxLevel) {
            int nextChapterID = selectedChapterID + 1;
            if (!IsUnlockedChapter(nextChapterID)) {
                ChapterSaveData chapterSaveData = new ChapterSaveData {
                    chapterID = nextChapterID,
                    passedLevel = 0,
                    superLevelPassed = 0,
                };
                chapterSaveDatas.Add(chapterSaveData);
            }
        }
    }
    public int LevelUp(int chapterID) {
        ChapterSaveData chapter = GetChapterSaveData(chapterID);
        if (chapter == null) return -1;
        return chapter.LevelUp();
    }
    public void SuperLevelUp(int chapterID) {
        ChapterSaveData chapter = GetChapterSaveData(chapterID);
        chapter.SuperLevelUp();
    }
    public ChapterSaveData GetChapterSaveData(int chapterID) {
        for(int i = 0; i < chapterSaveDatas.Count; i++) {
            ChapterSaveData chapterSaveData = chapterSaveDatas[i];
            if(chapterSaveData.chapterID == chapterID) {
                return chapterSaveData;
            }
        }
        return null;
    }
    public bool IsUnlockedChapter(int chapterID) {
        ChapterSaveData chapterSaveData = GetChapterSaveData(chapterID);
        if(chapterSaveData == null) {
            return false;
        } else {
            return true;
        }
    }
    public int GetPassedLevel(int chapterID) {
        ChapterSaveData chapterSaveData = GetChapterSaveData(chapterID);
        if(chapterSaveData == null) {
            return -1;
        } else {
            return chapterSaveData.GetPassedLevel();
        }
    }
}
[Serializable]
public class ChapterSaveData {
    public int chapterID;
    public int passedLevel;
    public int superLevelPassed;
    
    public int GetPassedLevel() {
        return passedLevel;
    }
    public bool IsPassedLevel(int level) {
        return passedLevel >= level;
    }
    public bool IsPassedSuperLevel(int level) {
        return superLevelPassed >= level;
    }
    public int LevelUp() {
        passedLevel++;
        return passedLevel;
    }
    public void SuperLevelUp() {
        superLevelPassed++;
    }
}