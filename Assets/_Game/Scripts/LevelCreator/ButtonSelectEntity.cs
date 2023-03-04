using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
public class ButtonSelectEntity : MonoBehaviour
{
    private EntityConfig m_EntityConfig;
    public TextMeshProUGUI m_TextPrefabName;
    public TextMeshProUGUI m_TextInput;
    public Image m_PreviewField;
    public Button m_Button;
    public GameObject m_ImageOnSelect;
    public UnityAction<EntityType> m_OnClickCallback;
    private void Awake()
    {
        m_Button.onClick.AddListener(OnClickButton);
    }

    public void Setup(EntityConfig entityConfig, UnityAction<EntityType> onclickCallback)
    {
        m_EntityConfig = entityConfig;
        m_TextPrefabName.text = m_EntityConfig.EntityType.ToString();
        m_PreviewField.sprite = m_EntityConfig.GetCurrentSprite();

        m_TextInput.text = ((int)m_EntityConfig.EntityType).ToString() ;
        m_OnClickCallback = onclickCallback;
    }
    public void OnClickButton()
    {
        m_OnClickCallback?.Invoke(m_EntityConfig.EntityType);
    }
    public void SetImageOnChoose(EntityType onChooseType)
    {
        m_ImageOnSelect.SetActive(onChooseType == m_EntityConfig.EntityType);
    }
    public void UpdatePreviewField()
    {
        m_PreviewField.sprite = m_EntityConfig.GetCurrentSprite();
    }
}
