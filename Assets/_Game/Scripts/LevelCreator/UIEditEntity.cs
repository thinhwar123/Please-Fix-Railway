using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

public class UIEditEntity : MonoBehaviour
{
    private Camera m_MainCamera;
    public Camera MainCamera { get { return m_MainCamera ??= Camera.main; } }
    private Transform m_Transform;
    public Transform Transform { get { return m_Transform ??= transform; } }
    public Entity m_EditEnity;
    public Vector3 m_Offset;
    public TMP_InputField m_InputGropID;
    public Button m_ButtonSave;

    private UnityAction m_OnClickSave;
    private void Update()
    {
        FollowEntity();
    }
    public void Setup(Entity entity, UnityAction onClickSave)
    {
        m_EditEnity = entity;
        m_OnClickSave = onClickSave;
        m_InputGropID.text = entity.GroupID.ToString();
        m_InputGropID.onValueChanged.AddListener(value => SaveInputText(value, ref entity));
        m_ButtonSave.onClick.AddListener(OnClickButtonSave);
    }
    private void FollowEntity()
    {
        if (m_EditEnity == null) return;

        Transform.position = MainCamera.WorldToScreenPoint(m_EditEnity.transform.position) + m_Offset;
    }
    private void SaveInputText(string inputText, ref Entity entity)
    {
        if (inputText == "") inputText = "1";
        m_EditEnity.GroupID = int.Parse(inputText);
        m_EditEnity.OnGroupIDChange();
    }
    public void OnClickButtonSave()
    {
        m_OnClickSave?.Invoke();
    }
}
