using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIChapter : MonoBehaviour {
    public int m_ChapterID;
    public List<UIButtonLevel> m_UIButtonLevels;
    
    public void Setup(int chapter) {
        ChapterConfig chapterConfig = GameData.Instance.GetChapterConfig(m_ChapterID);
        int passedLevel = PlayerData.Instance.GetPassedLevel(m_ChapterID);
        int currentLevel = passedLevel + 1;
        for (int i = 0; i < chapterConfig.maxLevel; i++) {
            UIButtonLevel uiLevel = m_UIButtonLevels[i];
            int level = i + 1;
            ButtonLevelState buttonState;
            if (level == currentLevel) {
                buttonState = ButtonLevelState.CURRENT;
            }else if (level < currentLevel) {
                buttonState = ButtonLevelState.PASSED;
            }else {
                buttonState = ButtonLevelState.LOCKED;
            }
            uiLevel.Setup(chapter, level, buttonState);
        }
    }
    public void UpdateLinkLine() {
        for(int i = 0; i < m_UIButtonLevels.Count; i++) {
            UIButtonLevel ui = m_UIButtonLevels[i];
            ui.Setup(m_ChapterID, i + 1, ButtonLevelState.LOCKED);

            if(i < m_UIButtonLevels.Count - 1) {
                UIButtonLevel nextLevel = m_UIButtonLevels[i + 1];
                ui.LinkTo(nextLevel.m_LinkLine.transform);
            }
        }
    }
}