using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class UICIngame : UICanvas
{
    [SerializeField] private SwitchButton m_ChangeModifyButton;
    [SerializeField] private TextMeshProUGUI m_TextRailCount;

    [SerializeField] private Button m_NextLevelButton;
    [SerializeField] private Button m_BackLevelButton;
    [SerializeField] private Button m_PlayButton;
    [SerializeField] private Button m_MapButton;

    private void Awake()
    {
        m_ChangeModifyButton.m_OnClick.AddListener(OnSwitchModify);
        m_NextLevelButton.onClick.AddListener(OnClickNextLevelButton);
        m_BackLevelButton.onClick.AddListener(OnClickBackLevelButton);
        m_PlayButton.onClick.AddListener(OnClickPlayButton);
    }
    public void Setup(bool modifyMode)
    {
        m_TextRailCount.text = GameManager.Instance.CurrentRailCount.ToString();
        m_ChangeModifyButton.Setup(modifyMode);
    }
    public void OnSwitchModify(bool canAdd)
    {
        if (canAdd)
        {
            GameInputHandler.Instance.ChangeEntityModifyMode(GameInputHandler.EntityModifyMode.Add);
        }
        else
        {
            GameInputHandler.Instance.ChangeEntityModifyMode(GameInputHandler.EntityModifyMode.Remove);
        }
    }
    public void OnClickNextLevelButton()
    {
        GameManager.Instance.LoadNextLevel();
    }
    public void OnClickBackLevelButton()
    {
        GameManager.Instance.LoadBackLevel();
    }
    public void OnClickPlayButton()
    {
        for (int i = 0; i < CellManager.Instance.CurrentHeight; i++)
        {
            for (int j = 0; j < CellManager.Instance.CurrentWidth; j++)
            {
                if (CellManager.Instance.GetCell(j, i).Entity == null) continue;
                if (CellManager.Instance.GetCell(j, i).Entity is CarRail)
                {
                    (CellManager.Instance.GetCell(j, i).Entity as CarRail).m_Wagon.Run();
                }
            }
        }
    }
}
