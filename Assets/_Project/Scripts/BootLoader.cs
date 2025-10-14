using UnityEngine;
using UnityEngine.SceneManagement;

public class BootLoader : MonoBehaviour
{
    [SerializeField] string firstSceneName = "MainMenu";
    [SerializeField] string firstScenePath = "Assets/_Project/Scenes/MainMenu.unity";
    [SerializeField] float delaySeconds = 0.1f;

    void Awake()
    {
        Debug.Log("[BootLoader] Awake() called - script is loading");
    }

    async void Start()
    {
        Debug.Log($"[BootLoader] Start() called - will load '{firstSceneName}' after {delaySeconds}s delay");
        await System.Threading.Tasks.Task.Delay((int)(delaySeconds * 1000));
        Debug.Log($"[BootLoader] Delay complete, attempting to load scene...");

        int index = SceneUtility.GetBuildIndexByScenePath(firstScenePath);
        Debug.Log($"[BootLoader] GetBuildIndexByScenePath('{firstScenePath}') returned: {index}");
        if (index >= 0)
        {
            Debug.Log($"[BootLoader] Loading scene by index {index}");
            SceneManager.LoadScene(index);
            return;
        }

        bool canLoad = Application.CanStreamedLevelBeLoaded(firstSceneName);
        Debug.Log($"[BootLoader] CanStreamedLevelBeLoaded('{firstSceneName}') returned: {canLoad}");
        if (canLoad)
        {
            Debug.Log($"[BootLoader] Loading scene by name '{firstSceneName}'");
            SceneManager.LoadScene(firstSceneName);
            return;
        }

        Debug.LogError($"[BootLoader] Scene '{firstSceneName}' / '{firstScenePath}' not in Build Settings.");
        Debug.Log("[BootLoader] Scenes in Build:");
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string p = SceneUtility.GetScenePathByBuildIndex(i);
            Debug.Log($"  [{i}] {p}");
        }
    }
}
