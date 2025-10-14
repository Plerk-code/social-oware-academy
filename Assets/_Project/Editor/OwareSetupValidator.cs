#if UNITY_EDITOR
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class OwareSetupValidator
{
    [MenuItem("Tools/Oware/Validate & Fix Boot→MainMenu Setup")]
    public static void ValidateAndFix()
    {
        // 1) Locate scenes
        string bootPath = FindSceneByExactName("Boot") ?? FindSceneByPartial("Boot");
        string mainPath = FindSceneByExactName("MainMenu") ?? FindSceneByPartial("MainMenu");

        if (string.IsNullOrEmpty(bootPath))
        {
            EditorUtility.DisplayDialog("Oware Setup",
                "Could not find a Boot.unity scene anywhere in Assets.\n\n" +
                "Please create one (Assets/_Project/Scenes/Boot.unity) and run this again.",
                "OK");
            return;
        }

        if (string.IsNullOrEmpty(mainPath))
        {
            // Best-effort: pick any non-Boot scene under _Project/Scenes as Main
            var candidates = Directory.Exists("Assets/_Project/Scenes")
                ? Directory.GetFiles("Assets/_Project/Scenes", "*.unity", SearchOption.AllDirectories).ToList()
                : new System.Collections.Generic.List<string>();
            mainPath = candidates.FirstOrDefault(p => !p.EndsWith("/Boot.unity"));
        }

        if (string.IsNullOrEmpty(mainPath))
        {
            EditorUtility.DisplayDialog("Oware Setup",
                "Could not find MainMenu.unity (or any non-Boot scene) to load after Boot.\n" +
                "Create MainMenu.unity then re-run.", "OK");
            return;
        }

        // 2) Add all scenes in Assets/_Project/Scenes to Build Settings (if any)
        AddAllScenesInFolderToBuild("Assets/_Project/Scenes");

        // Ensure boot & main present
        AddSceneToBuildIfMissing(bootPath);
        AddSceneToBuildIfMissing(mainPath);

        // Ensure Boot is first
        ForceBootFirst(bootPath);

        // 3) Open Boot and ensure BootLoader exists + configured
        var scene = EditorSceneManager.OpenScene(bootPath, OpenSceneMode.Single);
        var bootLoader = GameObject.FindFirstObjectByType<BootLoader>();
        if (bootLoader == null)
        {
            var go = new GameObject("BootLoader");
            bootLoader = go.AddComponent<BootLoader>();
        }

        // Configure BootLoader
        var so = new SerializedObject(bootLoader);
        so.FindProperty("firstSceneName").stringValue = Path.GetFileNameWithoutExtension(mainPath);
        so.FindProperty("firstScenePath").stringValue = mainPath.Replace("\\", "/");
        so.ApplyModifiedPropertiesWithoutUndo();

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);

        // 4) Report Build Settings
        var list = string.Join("\n", Enumerable.Range(0, SceneManager.sceneCountInBuildSettings)
            .Select(i => $"[{i}] {SceneUtility.GetScenePathByBuildIndex(i)}"));
        Debug.Log($"[OwareSetupValidator] Boot: {bootPath}\nMain: {mainPath}\n--- Scenes In Build ---\n{list}");

        EditorUtility.DisplayDialog("Oware Setup",
            "Validation complete.\n\n- Boot is first in Build Settings\n- BootLoader configured to load your MainMenu\n\nCheck the Console for the full scene list.",
            "Great");
    }

    static string FindSceneByExactName(string name)
    {
        var guids = AssetDatabase.FindAssets($"t:Scene {name}");
        foreach (var g in guids)
        {
            var path = AssetDatabase.GUIDToAssetPath(g).Replace("\\", "/");
            if (Path.GetFileNameWithoutExtension(path) == name) return path;
        }
        return null;
    }

    static string FindSceneByPartial(string partial)
    {
        var guids = AssetDatabase.FindAssets("t:Scene");
        foreach (var g in guids)
        {
            var path = AssetDatabase.GUIDToAssetPath(g).Replace("\\", "/");
            if (Path.GetFileNameWithoutExtension(path).ToLower().Contains(partial.ToLower()))
                return path;
        }
        return null;
    }

    static void AddAllScenesInFolderToBuild(string folder)
    {
        if (!Directory.Exists(folder)) return;
        var found = Directory.GetFiles(folder, "*.unity", SearchOption.AllDirectories);

        var scenes = EditorBuildSettings.scenes.ToList();
        foreach (var p in found)
        {
            if (!scenes.Any(s => s.path == p))
                scenes.Add(new EditorBuildSettingsScene(p, true));
        }
        EditorBuildSettings.scenes = scenes.ToArray();
    }

    static void AddSceneToBuildIfMissing(string path)
    {
        var scenes = EditorBuildSettings.scenes.ToList();
        if (!scenes.Any(s => s.path == path))
        {
            scenes.Add(new EditorBuildSettingsScene(path, true));
            EditorBuildSettings.scenes = scenes.ToArray();
        }
    }

    static void ForceBootFirst(string bootPath)
    {
        var scenes = EditorBuildSettings.scenes.ToList();
        var idx = scenes.FindIndex(s => s.path == bootPath);
        if (idx > 0)
        {
            var boot = scenes[idx];
            scenes.RemoveAt(idx);
            scenes.Insert(0, boot);
            EditorBuildSettings.scenes = scenes.ToArray();
        }
    }
}
#endif
