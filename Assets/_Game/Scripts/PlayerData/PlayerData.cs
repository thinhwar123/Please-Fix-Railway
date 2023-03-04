using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour {
    private static PlayerData m_Instance;
    public static PlayerData Instance {
        get {
            return m_Instance;
        }
    }
    public const string key_resource_brain = "key_res_brain";
    public const string key_level = "key_level";

    [SerializeField]
    private ResourceData m_Brain = new ResourceData(key_resource_brain, 0);
    [SerializeField]
    private LevelSaveDatas m_LevelSaveData = new LevelSaveDatas(key_level);

    private void Awake() {
        if (m_Instance != null) {
            DestroyImmediate(gameObject);
        } else {
            m_Instance = this;
            DontDestroyOnLoad(this);
        }
        Debug.Log("Load");
        Load();
    }
    private void Start() {
        
    }
    public void Load() {
        m_Brain.Load();
        m_LevelSaveData.Load();
        //for (int i = 0; i < 10; i++) {
        //    LevelUp();
        //}
        //m_LevelSaveData.Save();
    }
    
    public void SelectLevel(int level, int chapter) {
        m_LevelSaveData.SelectLevel(level, chapter);
    }
    public int GetSelectedLevel() {
        return m_LevelSaveData.selectedLevel;
    }
    public bool IsUnlockChapter(int chapterID) {
        return m_LevelSaveData.IsUnlockedChapter(chapterID);
    }
    public int GetPassedLevel(int chapterID) {
        return m_LevelSaveData.GetPassedLevel(chapterID);
    }
    public void LevelUp() {
        m_LevelSaveData.LevelUp();
    }
    public int GetBrain() {
        return m_Brain.GetAmount();
    }
    public void ConsumeBrain(int amount) {
        m_Brain.Consume(amount);
    }
    public bool IsEnough(int amount) {
        return m_Brain.IsEnough(amount);
    }
}