using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIChapterCard : MonoBehaviour {
    public Button m_ButtonSelect;
    public Image m_ImageButton;
    public TextMeshProUGUI m_TextChapterID;
    public List<Sprite> m_SpriteButtons;
    private int m_ChapterID;
    private void Start() {
        m_ButtonSelect.onClick.AddListener(OnSelect);
    }
    public void Setup(int chapterID) {
        m_ChapterID = chapterID;
        m_TextChapterID.text = "Chapter " + m_ChapterID.ToString();
        bool isUnlocked = PlayerData.Instance.IsUnlockChapter(chapterID);
        m_ButtonSelect.interactable = isUnlocked;
        if (isUnlocked) {
            m_ImageButton.sprite = m_SpriteButtons[0];
        } else {
            m_ImageButton.sprite = m_SpriteButtons[1];
        }
    }
    public void OnSelect() {
        UI_Game.Instance.CloseUI(UIID.UICChapter);
        UI_Game.Instance.OpenUI<UICMapProgress>(UIID.UICMapProgress).Setup(m_ChapterID);
    }
}