using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIButtonInput : MonoBehaviour {
    public GameObject m_ImageBG_Up, m_ImageBG_Down;
    public Image m_ImageUp, m_ImageDown;
    public TextMeshProUGUI m_TextTotalMove;

    public Button m_Button;
    public InputType m_InputType;
    public int m_TotalMove;
    private void Start() {
        UI_Game.AddClickListener(m_Button, OnClick);
    }
    private void OnEnable() {
        m_InputType = InputType.INSERT;
        UpdateInputType();
    }
    public void SetTotalMove(int move) {
        m_TextTotalMove.text = move.ToString();
    }
    public void OnClick() {
        if (m_InputType == InputType.INSERT) {
            m_InputType = InputType.DELETE;
        } else {
            m_InputType = InputType.INSERT;
        }
        UpdateInputType();
        IngameManager.Instance.m_InputType = m_InputType;
    }
    private void UpdateInputType() {
        if (m_InputType == InputType.INSERT) {
            m_ImageBG_Down.SetActive(true);
            m_ImageBG_Up.SetActive(false);
            m_ImageDown.color = Color.white;
            m_ImageUp.color = Color.gray;
        } else {
            m_ImageBG_Down.SetActive(false);
            m_ImageBG_Up.SetActive(true);
            m_ImageDown.color = Color.gray;
            m_ImageUp.color = Color.white;
        }
    }
}