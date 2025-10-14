#if UNITY_EDITOR
using System.IO;
using System.Linq;
using UnityEditor;

public static class AddScenesToBuild
{
    [MenuItem("Tools/Oware/Add All _Project/Scenes To Build")]
    public static void AddAll()
    {
        var dir = "Assets/_Project/Scenes";
        var scenePaths = Directory.Exists(dir)
            ? Directory.GetFiles(dir, "*.unity", SearchOption.AllDirectories)
            : new string[0];

        var existing = EditorBuildSettings.scenes.ToList();
        foreach (var p in scenePaths)
        {
            if (!existing.Any(s => s.path == p))
                existing.Add(new EditorBuildSettingsScene(p, true));
        }

        existing = existing
            .OrderBy(s => s.path.EndsWith("/Boot.unity") ? 0 : 1)
            .ThenBy(s => s.path)
            .ToList();

        EditorBuildSettings.scenes = existing.ToArray();
        EditorUtility.DisplayDialog("Scenes Added",
            $"Added {scenePaths.Length} scene(s) from {dir} to Build Settings.", "OK");
    }
}
#endif
