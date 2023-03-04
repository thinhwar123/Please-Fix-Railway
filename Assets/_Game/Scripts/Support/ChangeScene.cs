#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;


public class ChangeScene : Editor {

    [MenuItem("Open Scene/Loading #1")]
    public static void OpenLoading()
    {
        OpenScene("FirstScene");
    }

    [MenuItem("Open Scene/Game #2")]
    public static void OpenGame()
    {
        OpenScene("MainScene");
    }
    private static void OpenScene (string sceneName) {
		if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo ()) {
			EditorSceneManager.OpenScene ("Assets/_Game/Scenes/" + sceneName + ".unity");
		}
	}
}
#endif