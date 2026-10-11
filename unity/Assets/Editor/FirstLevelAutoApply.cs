using UnityEditor;
using UnityEngine;

/// <summary>
/// Пересборка Clearing под зону 1, если раскладка устарела.
/// Маркер Assets/Editor/~APPLY_ZONE1 форсирует сборку.
/// </summary>
[InitializeOnLoad]
static class FirstLevelAutoApply
{
    const string MarkerPath = "Assets/Editor/~APPLY_ZONE1";
    const string LegacyMarkerPath = "Assets/Editor/~APPLY_FIRST_LEVEL";
    const string ScenePath = "Assets/Scenes/Clearing.unity";

    static FirstLevelAutoApply()
    {
        EditorApplication.delayCall += TryApply;
    }

    static void TryApply()
    {
        bool force = System.IO.File.Exists(MarkerPath) || System.IO.File.Exists(LegacyMarkerPath);

        if (EditorApplication.isPlayingOrWillChangePlaymode)
        {
            if (!force)
                return;

            Debug.LogWarning("FirstLevelAutoApply: выхожу из Play, затем соберу зону 1.");
            EditorApplication.isPlaying = false;
            EditorApplication.delayCall += TryApply;
            return;
        }

        var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        if (!force && scene.path != ScenePath)
            return;

        if (NeedsRebuild(force))
        {
            Debug.Log("FirstLevelAutoApply: собираю Zone 1 Forest Glade в Clearing…");
            Zone1ForestGladeSetup.Build();

            if (System.IO.File.Exists(MarkerPath))
                AssetDatabase.DeleteAsset(MarkerPath);
            if (System.IO.File.Exists(LegacyMarkerPath))
                AssetDatabase.DeleteAsset(LegacyMarkerPath);
        }
    }

    static bool NeedsRebuild(bool force)
    {
        if (force)
            return true;

        if (GameObject.Find("Ground") != null)
            return true;

        if (GameObject.Find("FinishPad") != null || GameObject.Find("FinishZone") != null)
            return true;

        if (GameObject.Find("RecoveryLedge") != null || GameObject.Find("LayoutMarker_VerticalBounce") != null)
            return true;

        if (GameObject.Find("PlatformMidHigh") != null || GameObject.Find("TotoroPad") != null)
            return true;

        // Старая раскладка (ветка/семя) или нет канона ○1–○5 из geometry.md.
        if (GameObject.Find("Zone1ForestGlade") == null)
            return true;

        if (GameObject.Find("RootLow") == null
            || GameObject.Find("RootHigh") == null
            || GameObject.Find("RootBehind") == null
            || GameObject.Find("Orb_Entrance") == null
            || GameObject.Find("Orb_BehindTrunk") == null
            || GameObject.Find("GlowPath") == null
            || GameObject.Find("SpiritAwakening") == null)
            return true;

        if (GameObject.Find("ClearingGround") == null || GameObject.Find("FallRespawnZone") == null)
            return true;

        return false;
    }
}
