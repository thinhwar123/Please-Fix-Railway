using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UICMapProgress : UICanvas {
    public TextMeshProUGUI m_TextChapter;
    public Transform m_UIChapterParent;
    public UIChapter m_UIChapter;

    public List<GameObject> m_UIChapterPrefabs;
    private int m_CurrentChapterID;
    private void OnEnable() {
        Setup(1);
    }
    public void Setup(int chapterID) {
        m_CurrentChapterID = chapterID;
        m_TextChapter.text = "Chapter " + chapterID;

        if(m_UIChapter == null || (m_UIChapter != null && m_UIChapter.m_ChapterID != chapterID)) {
            if (m_UIChapter != null) {
                Destroy(m_UIChapter); 
            }
            GameObject prefab = GetUIChapterPrefab(chapterID);
            GameObject go = GameObject.Instantiate(prefab, Vector3.zero, Quaternion.identity, m_UIChapterParent);
            go.transform.localPosition = Vector3.zero;

            m_UIChapter = go.GetComponent<UIChapter>();
        }
        m_UIChapter.Setup(m_CurrentChapterID);
    }
    public GameObject GetUIChapterPrefab(int chapterID) {
        return m_UIChapterPrefabs[chapterID - 1];
    }
}