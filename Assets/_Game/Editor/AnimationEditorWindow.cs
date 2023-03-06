using UnityEditor;
using UnityEngine;

public class AnimationEditorWindow : EditorWindow
{
    public const string LOCAL_POS_X = "m_LocalPosition.x";
    public const string LOCAL_POS_Y = "m_LocalPosition.y";
    public const string LOCAL_POS_Z = "m_LocalPosition.z"; 
    
    public const string LOCAL_ROT_X = "m_LocalRotation.x";
    public const string LOCAL_ROT_Y = "m_LocalRotation.y";
    public const string LOCAL_ROT_Z = "m_LocalRotation.z";    
    
    public const string LOCAL_SCA_X = "m_LocalScale.x";
    public const string LOCAL_SCA_Y = "m_LocalScale.y";
    public const string LOCAL_SCA_Z = "m_LocalScale.z";

    private AnimationClip clip;
    private GameObject go;

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
        go = EditorGUILayout.ObjectField(go, typeof(GameObject), true) as GameObject;

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

        if (GUILayout.Button("Test") && clip != null)
        {
            Debug.Log(clip.name);
            SetPropertyValue("Cell_2_0", LOCAL_ROT_Y, 0.5f, 0.5f, typeof(Transform));
        }
    }

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
}
