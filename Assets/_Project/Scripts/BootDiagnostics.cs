using UnityEngine;
using UnityEngine.SceneManagement;

public class BootDiagnostics : MonoBehaviour
{
    void Start()
    {
        Debug.Log("[BootDiagnostics] Scenes In Build:");
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            var p = SceneUtility.GetScenePathByBuildIndex(i);
            Debug.Log($"  [{i}] {p}");
        }
    }
}
