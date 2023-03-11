using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using DG.Tweening;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Image))]
public class SwitchButton : Selectable, IPointerClickHandler
{
    public UnityEvent<bool> m_OnClick;

    public bool m_Value;
    public GameObject m_ImageOn;
    public GameObject m_ImageOff;
    public GameObject m_ImageBtn;

    private float m_XPosition;
    private Vector3 m_Position;
    private Tween m_Tween;
    protected override void Awake()
    {
        base.Awake();
        if (m_ImageOn!= null && m_ImageOff != null && m_ImageBtn!= null)
        {
            m_Position = m_ImageBtn.transform.localPosition;
            m_XPosition = m_Position.x;
        }

    }
    public void Setup(bool value)
    {
        m_Value = value;
        m_ImageOn.SetActive(value);
        m_ImageOff.SetActive(!value);

        m_Position.x = m_XPosition * (value ? 1 : -1);
        m_ImageBtn.transform.localPosition = m_Position;

    }
    private void Press()
    {
        if (IsActive() && IsInteractable())
        {

            m_Value = !m_Value;
            m_ImageOn.SetActive(m_Value);
            m_ImageOff.SetActive(!m_Value);

            m_Tween?.Kill();
            float x = m_XPosition * (m_Value ? 1 : -1);
            m_Tween = m_ImageBtn.transform.DOLocalMoveX(x, 0.2f);

            m_OnClick.Invoke(m_Value);
        }
    }

    public virtual void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            Press();
        }
    }
}
