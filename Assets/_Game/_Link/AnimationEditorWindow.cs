using System;
using UnityEditor;
using UnityEngine;

public class AnimationEditorWindow : EditorWindow
{
    public const string LOCAL_POS_X = "m_LocalPosition.x";
    public const string LOCAL_POS_Y = "m_LocalPosition.y";
    public const string LOCAL_POS_Z = "m_LocalPosition.z"; 
    
    public const string LOCAL_ROT_X = "localEulerAnglesRaw.x";
    public const string LOCAL_ROT_Y = "localEulerAnglesRaw.y";
    public const string LOCAL_ROT_Z = "localEulerAnglesRaw.z";    
    
    public const string LOCAL_SCA_X = "m_LocalScale.x";
    public const string LOCAL_SCA_Y = "m_LocalScale.y";
    public const string LOCAL_SCA_Z = "m_LocalScale.z";

    private AnimationClip clip;
    private float time = 0;

    [MenuItem("Window/Animation Window")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        AnimationEditorWindow window = (AnimationEditorWindow)EditorWindow.GetWindow(typeof(AnimationEditorWindow));
        window.Show();
    }

    private void OnGUI()
    {
        clip = EditorGUILayout.ObjectField(clip, typeof(AnimationClip), false) as AnimationClip;
        time = EditorGUILayout.FloatField("Key Time", time);

        if (GUILayout.Button("Rename"))
        {
            GameObject[] gameObjects = Selection.gameObjects;

            for (int i = 0; i < gameObjects.Length; i++)
            {
                Cell cell = gameObjects[i].GetComponent<Cell>();
                if (cell != null) 
                {
                    gameObjects[i].name = $"Cell_{cell.Coordinates.x}_{cell.Coordinates.y}";
                }
            }

        }

        if (GUILayout.Button("Keys Selecting") && clip != null)
        {
            for (int i = 0; i < Selection.gameObjects.Length; i++)
            {
                SetTransformProperty(Selection.gameObjects[i].name, Selection.gameObjects[i].transform, time);
            }
        }


        if (GUILayout.Button("Key All") && clip != null)
        {
            Cell[] gameObjects = GameObject.FindObjectsOfType<Cell>();
            for (int i = 0; i < gameObjects.Length; i++)
            {
                SetTransformProperty(gameObjects[i].name, gameObjects[i].transform, time);
            }

            time += 0.1f;
            time = (float)Math.Round(time, 1);

            Debug.Log("Done - " + time);
        }

        DrawSelectingObject();

    }

    #region Property

    public float GetPropertyValue(string objectName, string propertyName, float time)
    {
        // adjust this to match the property you want to get
        AnimationCurve curve = null;
        foreach (EditorCurveBinding binding in AnimationUtility.GetCurveBindings(clip))
        {
            if (binding.path == objectName && binding.propertyName == propertyName)
            {
                curve = AnimationUtility.GetEditorCurve(clip, binding);
                break;
            }
        }

        // Evaluate the curve at a specific time (in seconds)
        return curve != null ? curve.Evaluate(time) : -1;
    }
    
    public void SetPropertyValue(string objectName, string propertyName, float time, float value, System.Type type)
    {
        // adjust this to match the property you want to get
        AnimationCurve curve = null;
        EditorCurveBinding bind = new();

        foreach (EditorCurveBinding binding in AnimationUtility.GetCurveBindings(clip))
        {
            if (binding.path == objectName && binding.propertyName == propertyName)
            {
                bind = binding;
                curve = AnimationUtility.GetEditorCurve(clip, bind);
                break;
            }
        }

        if (curve == null)
        {
            bind.path = objectName; // set the object path
            bind.type = type; // set the object type
            bind.propertyName = propertyName; // set the property name

            // Create a new AnimationCurve for the new property
            curve = new AnimationCurve();
        }

        if (curve != null)
        {
            curve.AddKey(time, value);
            AnimationUtility.SetEditorCurve(clip, bind, curve);
        }

    }

    #endregion

    #region Transform
    private AnimationCurve GetCurve(string objectName, string propertyName, System.Type type, ref EditorCurveBinding bind)
    {
        AnimationCurve curve = null;

        foreach (EditorCurveBinding binding in AnimationUtility.GetCurveBindings(clip))
        {
            if (binding.path == objectName && binding.propertyName == propertyName)
            {
                bind = binding;
                curve = AnimationUtility.GetEditorCurve(clip, bind);
                break;
            }
        }

        if (curve == null)
        {
            curve = new AnimationCurve();
            bind.path = objectName; // set the object path
            bind.type = type; // set the object type
            bind.propertyName = propertyName; // set the property name
            AnimationUtility.SetEditorCurve(clip, bind, curve);
        }

        return curve;
    }

    public void SetTransformProperty(string objectName, Transform tf, float time)
    {
        SetValue(objectName, LOCAL_POS_X, time, tf.localPosition.x);
        SetValue(objectName, LOCAL_POS_Y, time, tf.localPosition.y);
        SetValue(objectName, LOCAL_POS_Z, time, tf.localPosition.z);

        SetValue(objectName, LOCAL_ROT_X, time, tf.localRotation.eulerAngles.x);
        SetValue(objectName, LOCAL_ROT_Y, time, tf.localRotation.eulerAngles.y);
        SetValue(objectName, LOCAL_ROT_Z, time, tf.localRotation.eulerAngles.z);

        SetValue(objectName, LOCAL_SCA_X, time, tf.localScale.x);
        SetValue(objectName, LOCAL_SCA_Y, time, tf.localScale.y);
        SetValue(objectName, LOCAL_SCA_Z, time, tf.localScale.z);

    }

    private void SetValue(string objectName, string propertyName, float time, float value)
    {
        EditorCurveBinding bind = new();
        AnimationCurve curve = GetCurve(objectName, propertyName, typeof(Transform), ref bind);
        curve.AddKey(time, value);
        AnimationUtility.SetEditorCurve(clip, bind, curve);
        Undo.RegisterCompleteObjectUndo(clip, "Save Anim");
    }

    #endregion

    private void DrawSelectingObject()
    {
        EditorGUILayout.BeginToggleGroup("", false);

        GUILayout.Label("Selected Objests : " + Selection.gameObjects.Length, EditorStyles.boldLabel);

        for (int i = 0; i < Selection.gameObjects.Length; i++)
        {
            GUILayout.Label($"{Selection.gameObjects[i].name} : {Selection.gameObjects[i].transform.position}");
        }

        EditorGUILayout.EndToggleGroup();
    }
}
