using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Barrier : DynamicRail
{
    [SerializeField] private Transform m_Barrier;
    public bool m_IsOpen;

    private Tween m_RotateTween;
    public override void TriggerEvent()
    {
        base.TriggerEvent();
        m_IsOpen = !m_IsOpen;

        Vector3 newAngle = m_IsOpen ? new Vector3(90f, 0f, 0f) : new Vector3(0f, 0f, 0f);
        m_RotateTween?.Kill();
        m_RotateTween = m_Barrier.DOLocalRotate(newAngle, 0.7f).SetEase(Ease.OutBack);
    }

}
