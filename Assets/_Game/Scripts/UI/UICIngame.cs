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
    }
    public override void Open()
    {
        base.Open();
        m_TextRailCount.text = GameManager.Instance.CurrentRailCount.ToString();
        m_ChangeModifyButton.Setup(true);
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
}
