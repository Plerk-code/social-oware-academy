#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;

[InitializeOnLoad]
public static class PlayFromBoot
{
    const string BootPath = "Assets/_Project/Scenes/Boot.unity";

    static PlayFromBoot()
    {
        EditorApplication.playModeStateChanged += (s) =>
        {
            if (s == PlayModeStateChange.ExitingEditMode)
            {
                if (EditorSceneManager.GetActiveScene().path != BootPath)
                    EditorSceneManager.OpenScene(BootPath);
            }
        };
    }
}
#endif
