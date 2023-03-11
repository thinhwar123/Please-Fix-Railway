using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICMainMenu : UICanvas
{
    [SerializeField] private ButtonSelectChap m_ButtonSelectChap;
    [SerializeField] private Transform m_Content;

    private void Awake()
    {
        List<ChapterConfig> chapterConfigs = ChapterGlobalConfig.Instance.m_ChapterConfigs;
        for (int i = 0; i < chapterConfigs.Count; i++)
        {
            ButtonSelectChap temp = Instantiate(m_ButtonSelectChap, m_Content);
            temp.Setup(chapterConfigs[i].chap, chapterConfigs[i].chapterName);
        }
    }
}
