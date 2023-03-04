using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public enum ButtonLevelState { CURRENT = 0, PASSED = 1, LOCKED = 2}
[ExecuteInEditMode]
public class UIButtonLevel : MonoBehaviour {
    public TextMeshProUGUI m_TextLevel;
    public List<Sprite> m_ButtonSprites;
    public Button m_ButtonLevel;
    public Image m_ImageLinkLine;
    public Transform m_LinkLine;
    private Transform m_NextLevelTransform;

    public Vector3 m_CurrentDirection1;
    public Vector3 m_TargetDirection1;

    public Vector3 m_CurrentDirection;
    public Vector3 m_TargetDirection;
    public float m_Angle;

    private int m_Chapter;
    private int m_Level;
    private void Start() {
        m_ButtonLevel.onClick.AddListener(OnSelectLevel);
    }
    private void Update() {
        if (m_NextLevelTransform != null) {
            Vector3 targetDirection = m_NextLevelTransform.position - m_LinkLine.transform.position;
            Debug.DrawRay(transform.position, targetDirection, Color.red);
        }
    }
    public void Setup(int chapter, int level, ButtonLevelState buttonState) {
        m_TextLevel.text = level.ToString();
        m_Chapter = chapter;
        m_Level = level;
        switch (buttonState) {
            case ButtonLevelState.CURRENT: {
                    m_ButtonLevel.image.sprite = m_ButtonSprites[0];
                    m_ButtonLevel.interactable = true;
                    m_ImageLinkLine.fillAmount = 0;
                }
                break;
            case ButtonLevelState.PASSED: {
                    m_ButtonLevel.image.sprite = m_ButtonSprites[1];
                    m_ButtonLevel.interactable = true;
                    m_ImageLinkLine.fillAmount = 1;
                }
                break;
            case ButtonLevelState.LOCKED: {
                    m_ButtonLevel.image.sprite = m_ButtonSprites[2];
                    m_ButtonLevel.interactable = false;
                    m_ImageLinkLine.fillAmount = 0;
                }
                break;
        }
    }
    public void LinkTo(Transform nextTarget) {
        if (m_LinkLine.gameObject.activeInHierarchy) {
            m_NextLevelTransform = nextTarget;

            m_CurrentDirection1 = m_LinkLine.position;
            m_TargetDirection1 = m_NextLevelTransform.position;

            m_TargetDirection = nextTarget.position - m_LinkLine.position;
            m_CurrentDirection = Vector3.up;

            m_Angle = Utilss.AngleBetweenVectors(m_CurrentDirection, m_TargetDirection)-90;
            m_LinkLine.localEulerAngles = new Vector3(0, 0, -m_Angle);
        }
    }
    public void OnSelectLevel() {
        Debug.Log("Select Chapter " + m_Chapter + " Select Level " + m_Level);
        LevelManager.Instance.LoadLevelStart(m_Chapter, m_Level);

        UI_Game.Instance.CloseUI(UIID.UICMapProgress);
        UI_Game.Instance.OpenUI(UIID.UICIngame);
        /// TODO: Open UI Ingame
    }
}