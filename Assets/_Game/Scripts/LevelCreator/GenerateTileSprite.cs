using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif



public class GenerateTileSprite : MonoBehaviour
{
    public LevelCreator m_LevelCreator;
    public Transform m_OffsetTransform;

#if UNITY_EDITOR

    [Button]
    public void GenerateSprite()
    {
        SnapshotCamera snapshotCamera = SnapshotCamera.MakeSnapshotCamera(6);
        for (int i = 0; i < EntityGlobalConfig.Instance.m_EntityConfigList.Count; i++)
        {
            for (int j = 0; j < EntityGlobalConfig.Instance.m_EntityConfigList[i].EnityPrefabList.Count; j++)
            {
                Texture2D texture = snapshotCamera.TakePrefabSnapshot(EntityGlobalConfig.Instance.m_EntityConfigList[i].EnityPrefabList[j].gameObject , Color.clear, m_OffsetTransform.localPosition, m_OffsetTransform.localRotation, m_OffsetTransform.localScale * 1.2f, 250, 250);
                byte[] pngShot = ImageConversion.EncodeToPNG(texture);
                System.IO.File.WriteAllBytes(System.IO.Path.Combine(Application.dataPath, "_Game/Sprites/", EntityGlobalConfig.Instance.m_EntityConfigList[i].EnityPrefabList[j].name + ".png"), pngShot);
            }
        }
        AssetDatabase.SaveAssets();
        DestroyImmediate(snapshotCamera.gameObject);
    }
    [Button]
    public void UpdateNewSprite()
    {
        for (int i = 0; i < EntityGlobalConfig.Instance.m_EntityConfigList.Count; i++)
        {
            EntityGlobalConfig.Instance.m_EntityConfigList[i].EnitySpriteList.Clear();
            for (int j = 0; j < EntityGlobalConfig.Instance.m_EntityConfigList[i].EnityPrefabList.Count; j++)
            {
                string[] guid = AssetDatabase.FindAssets("t:Sprite " + EntityGlobalConfig.Instance.m_EntityConfigList[i].EnityPrefabList[j].name);
                if (guid.Length == 0)
                {
                    Debug.Log("Missing Sprite: " + EntityGlobalConfig.Instance.m_EntityConfigList[i].EnityPrefabList[j].name);
                    continue;
                }
                for (int k = 0; k < guid.Length; k++)
                {
                    Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(AssetDatabase.GUIDToAssetPath(guid[k]));
                    if (sprite.name.CompareTo(EntityGlobalConfig.Instance.m_EntityConfigList[i].EnityPrefabList[j].name) == 0)
                    {
                        EntityGlobalConfig.Instance.m_EntityConfigList[i].EnitySpriteList.Add(sprite);
                        break;
                    }                    
                }

            }
        }
    }
#endif
}
