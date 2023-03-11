using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonSelectLevel : MonoBehaviour
{
    [SerializeField] private int m_Chap;
    [SerializeField] private int m_Level;
    [SerializeField] private Button m_Button;
    [SerializeField] private TextMeshProUGUI m_TextLevel;
    private void Awake()
    {
        m_Button.onClick.AddListener(OnClickButton);
    }
    public void Setup(int chap, int level)
    {
        m_Chap = chap;
        m_Level = level;

        m_TextLevel.text = level.ToString();
    }
    private void OnClickButton()
    {
        GameManager.Instance.LoadLevel(m_Chap, m_Level);
        UI_Game.Instance.CloseUI(UIID.UICLevelSelect);
        UI_Game.Instance.OpenUI(UIID.UICIngame);

    }
}
