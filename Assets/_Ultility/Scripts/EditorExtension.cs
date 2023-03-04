using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif


namespace MyExtension
{
    public static class EditorExtension
    {
        public static bool IsInPrefabStage()
        {
#if UNITY_EDITOR
            var stage = UnityEditor.SceneManagement.PrefabStageUtility.GetCurrentPrefabStage();

            return stage != null;
#else
        return true;
#endif
        }
        public static bool IsInPrefabStage(this MonoBehaviour mono)
        {
#if UNITY_EDITOR
            var stage = UnityEditor.SceneManagement.PrefabStageUtility.GetCurrentPrefabStage();
            return stage != null && stage.prefabContentsRoot.name == mono.name && !EditorUtility.IsPersistent(mono.gameObject);
#else
        return true;
#endif
        }
        /// <summary>
        /// Find child transform by name (Only use in editor)
        /// </summary>
        /// <param name="name"> Child name </param>
        /// <param name="parent"> Transform parent </param>
        /// <returns>Transform of gameobject if don't have gameobject create a new one</returns>
        public static Transform FindChildOrCreate(this Transform parent, string name, bool includeInactive = false)
        {
            Transform res = parent.GetComponentsInChildren<Transform>(includeInactive).FirstOrDefault(value => value.name == name);
            if (res == default)
            {
                res = new GameObject(name).transform;
                res.SetParent(parent);
                res.transform.localPosition = Vector3.zero;
                res.transform.localRotation = Quaternion.identity;
            }
            return res;
        }
        /// <summary>
        /// Return a prefab in Asset by name (Only use in editor)
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static GameObject FindPrefab(string name)
        {
#if UNITY_EDITOR
            string[] guids = AssetDatabase.FindAssets($"t:Prefab {name}");
            if (guids.Length > 0) return AssetDatabase.LoadAssetAtPath<GameObject>(AssetDatabase.GUIDToAssetPath(guids[0]));
#endif
            return null;
        }
        /// <summary>
        /// Return a model in Asset by name (Only use in editor)
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static GameObject FindModel(string name)
        {
#if UNITY_EDITOR
            string[] guids = AssetDatabase.FindAssets($"t:Model {name}");
            if (guids.Length > 0) return AssetDatabase.LoadAssetAtPath<GameObject>(AssetDatabase.GUIDToAssetPath(guids[0]));
#endif
            return null;
        }
    }
}

