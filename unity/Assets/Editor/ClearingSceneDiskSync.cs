using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[InitializeOnLoad]
static class ClearingSceneDiskSync
{
    const string ScenePath = "Assets/Scenes/Clearing.unity";
    const float GroundTopY = 0f;

    static ClearingSceneDiskSync()
    {
        EditorApplication.delayCall += TryFixLayoutInMemory;
    }

    [MenuItem("Totoro/Reload Clearing From Disk")]
    public static void ReloadFromDiskMenu()
    {
        if (EditorApplication.isPlayingOrWillChangePlaymode)
        {
            Debug.LogWarning("Сначала выйди из Play, потом Totoro → Reload Clearing From Disk.");
            return;
        }

        EditorSceneManager.OpenScene(ScenePath, OpenSceneMode.Single);
        TryAlignTotoroFeetInEditor(force: true);
        Debug.Log("Clearing перезагружена с диска.");
    }

    [MenuItem("Totoro/Fix Totoro On Ground")]
    public static void FixTotoroOnGroundMenu()
    {
        if (TryAlignTotoroFeetInEditor(force: true))
            Debug.Log("Тоторo прижат к поляне по pivot спрайта.");
        else
            Debug.Log("Тоторo не найден или спрайт не назначен.");
    }

    static void TryFixLayoutInMemory()
    {
        if (EditorApplication.isPlayingOrWillChangePlaymode)
            return;

        var active = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        if (active.path != ScenePath)
            return;

        if (IsZone1LayoutInMemory())
        {
            TryAlignTotoroFeetInEditor();
            return;
        }

        // Один раз при загрузке редактора — без цикла каждые 1.5 с.
        Debug.Log("ClearingSceneDiskSync: в памяти не зона 1 — перезагружаю Clearing.unity с диска.");
        EditorSceneManager.OpenScene(ScenePath, OpenSceneMode.Single);
        TryAlignTotoroFeetInEditor(force: true);
    }

    static bool IsZone1LayoutInMemory()
    {
        return FindInScene("Zone1ForestGlade") != null
            && FindInScene("RootLow") != null
            && FindInScene("Orb_Entrance") != null
            && FindInScene("GlowPath") != null
            && FindInScene("SpiritAwakening") != null
            && FindInScene("ClearingGround") != null
            && FindInScene("RecoveryLedge") == null
            && FindInScene("FinishPad") == null
            && FindInScene("StartPad") == null;
    }

    /// <summary>Ищет и среди неактивных (GlowPath стартует выключенным).</summary>
    static GameObject FindInScene(string name)
    {
        foreach (Transform t in Object.FindObjectsByType<Transform>(
                     FindObjectsInactive.Include, FindObjectsSortMode.None))
        {
            if (t.name != name)
                continue;
            if (t.gameObject.scene.path != ScenePath)
                continue;
            return t.gameObject;
        }

        return null;
    }

    static bool TryAlignTotoroFeetInEditor(bool force = false)
    {
        GameObject totoro = FindInScene("Totoro");
        if (totoro == null)
            return false;

        SpriteRenderer renderer = totoro.GetComponent<SpriteRenderer>();
        if (renderer == null)
            renderer = totoro.GetComponentInChildren<SpriteRenderer>();
        if (renderer == null || renderer.sprite == null)
            return false;

        float groundY = GroundTopY;
        TotoroBounce bounce = totoro.GetComponent<TotoroBounce>();
        if (bounce != null)
        {
            var so = new SerializedObject(bounce);
            groundY = so.FindProperty("feetGroundY").floatValue;
        }

        Sprite sprite = renderer.sprite;
        float pivotToBottom = sprite.pivot.y / sprite.pixelsPerUnit;
        float targetY = groundY + pivotToBottom;
        if (!force && Mathf.Abs(totoro.transform.position.y - targetY) < 0.01f)
            return true;

        Undo.RecordObject(totoro.transform, "Align Totoro to ground");
        TotoroBounce.AlignFeetToGroundY(totoro.transform, groundY);
        EditorSceneManager.MarkSceneDirty(UnityEngine.SceneManagement.SceneManager.GetActiveScene());
        return true;
    }
}
