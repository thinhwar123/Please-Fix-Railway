using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(UIChapter))]
public class UIChapterEditor : Editor{
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        if (GUILayout.Button("UpdateLinkLine")) {
            UIChapter uiChapter = (UIChapter)target;
            uiChapter.UpdateLinkLine();
            EditorUtility.SetDirty(uiChapter);
        }
    }

}