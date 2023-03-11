using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICLevelSelect : UICanvas
{
    [SerializeField] private Button m_ButtonBack;
    [SerializeField] private ButtonSelectLevel m_ButtonSelectNormalLevel;
    [SerializeField] private ButtonSelectLevel m_ButtonSelectHardLevel;

    [SerializeField] private Transform m_NormalContent;
    [SerializeField] private Transform m_HardContent;

    private List<ButtonSelectLevel> m_ButtonSelectLevels = new List<ButtonSelectLevel>();

    private void Awake()
    {
        m_ButtonBack.onClick.AddListener(OnClickButtonBack);
    }
    public void Setup(int chap)
    {
        ChapterConfig chapterConfig = ChapterGlobalConfig.Instance.GetChapterConfig(chap);

        for (int i = 0; i < chapterConfig.normalLevel; i++)
        {
            ButtonSelectLevel temp = Instantiate(m_ButtonSelectNormalLevel, m_NormalContent);
            temp.Setup(chap, i + 1);
            m_ButtonSelectLevels.Add(temp);
        }
        for (int i = 0; i < chapterConfig.hardLevel; i++)
        {
            ButtonSelectLevel temp = Instantiate(m_ButtonSelectHardLevel, m_HardContent);
            temp.Setup(-chap, i + 1);
            m_ButtonSelectLevels.Add(temp);
        }
    }
    public void OnClickButtonBack()
    {
        UI_Game.Instance.OpenUI(UIID.UICMainMenu);
        Close();
    }
    public override void Close()
    {
        for (int i = 0; i < m_ButtonSelectLevels.Count; i++)
        {
            Destroy(m_ButtonSelectLevels[i].gameObject);
        }
        m_ButtonSelectLevels.Clear();
        base.Close();
    }
}
