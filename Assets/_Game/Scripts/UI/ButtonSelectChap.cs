using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonSelectChap : MonoBehaviour
{
    [SerializeField] private int m_Chap;
    [SerializeField] private Button m_Button;
    [SerializeField] private TextMeshProUGUI m_TextChap;
    [SerializeField] private TextMeshProUGUI m_TextChapName;
    [SerializeField] private Image m_ImageBG;
    [SerializeField] private List<Sprite> m_ImageBGList;
    private void Awake()
    {
        m_Button.onClick.AddListener(OnClickButton);
    }
    public void Setup(int chap, string chapName)
    {
        m_Chap = chap;
        m_ImageBG.sprite = m_ImageBGList[chap % m_ImageBGList.Count];
        m_TextChap.text = $"Chap {chap}";
        m_TextChapName.text = $"{chapName}";
    }
    public void OnClickButton()
    {
        UI_Game.Instance.OpenUI<UICLevelSelect>(UIID.UICLevelSelect).Setup(m_Chap);
        UI_Game.Instance.CloseUI(UIID.UICMainMenu);
    }
}
