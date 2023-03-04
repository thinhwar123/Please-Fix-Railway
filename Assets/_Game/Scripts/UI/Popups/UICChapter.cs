using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICChapter : UICanvas {
    public List<UIChapterCard> m_UIChapterCards;
    public override void Setup() {
        base.Setup();
        for(int i = 0; i < m_UIChapterCards.Count; i++) {
            int chapterID = i + 1;
            UIChapterCard ui = m_UIChapterCards[i];
            ui.Setup(chapterID);
        }
    }
    private void OnEnable() {
        Debug.Log("Setup");
        Setup();
    }
}