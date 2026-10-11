using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

/// <summary>
/// Зона 1: компактный маршрут §C, минимум объектов, задник без пустот в кадре.
/// Меню: Totoro → Build Zone 1 Forest Glade
/// </summary>
public static class Zone1ForestGladeSetup
{
    const string ScenePath = "Assets/Scenes/Clearing.unity";
    const string PlayerPrefabPath = "Assets/Prefabs/Player.prefab";
    const string CollectiblePrefabPath = "Assets/Prefabs/Collectible.prefab";

    const string GroundSpritePath = "Assets/Art/Ground.png";
    const string TotoroSpritePath = "Assets/Art/TotoroSleep.png";
    const string GladeBackPath = "Assets/Art/GladeBack.png";
    const string BranchLandingPath = "Assets/Art/BranchLanding.png";
    const string OrbSpritePath = "Assets/Art/SeedGroundGlow.png";

    const float PlatformHeight = 0.5f;
    const float SurfaceThickness = 0.18f;

    // geometry.md §C (досягаемость airMaxSpeed 4).
    const float TotoroX = 5f;
    const float GroundCenterX = 5f;
    const float GroundWidth = 28f;
    const float OrbScale = 4f;
    const float CameraOrtho = 6f;

    [MenuItem("Totoro/Build Zone 1 Forest Glade")]
    public static void Build()
    {
        int groundLayer = LayerMask.NameToLayer("Ground");
        if (groundLayer < 0)
            throw new System.InvalidOperationException("Слой Ground не найден.");

        Sprite groundSprite = LoadSprite(GroundSpritePath);
        GameObject playerPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(PlayerPrefabPath);
        GameObject collectiblePrefab = AssetDatabase.LoadAssetAtPath<GameObject>(CollectiblePrefabPath);
        if (playerPrefab == null || collectiblePrefab == null)
            throw new System.InvalidOperationException("Не найден Player или Collectible Prefab.");

        var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        if (scene.path != ScenePath)
            scene = EditorSceneManager.OpenScene(ScenePath, OpenSceneMode.Single);

        ClearLevelObjects();

        Vector2 boundsMin = new Vector2(-2f, 0.5f);
        Vector2 boundsMax = new Vector2(15f, 13f);
        PlaceGladeBackCoveringCamera(boundsMin, boundsMax);

        CreateTiledGround("ClearingGround", GroundCenterX, 0f, GroundWidth, groundLayer, groundSprite);

        var spawn = new GameObject("PlayerSpawn");
        spawn.transform.position = new Vector3(1.2f, 0f, 0f);
        spawn.AddComponent<RespawnPoint>();

        PlacePlayer(playerPrefab, spawn.transform.position);
        PlaceOrMoveTotoro(new Vector3(TotoroX, 0f, 0f));
        EnsureCollectibleTally();

        CreateRootPlatform("RootLow", BranchLandingPath, 2.6f, 1.2f, groundLayer, 5);
        CreateRootPlatform("RootLand", BranchLandingPath, 7.2f, 3.2f, groundLayer, 5);
        CreateRootPlatform("RootHigh", BranchLandingPath, 9.0f, 7.0f, groundLayer, 5);
        CreateRootPlatform("RootBehind", BranchLandingPath, 11.5f, 5.8f, groundLayer, 5);

        PlaceCollectible(collectiblePrefab, "Orb_Entrance", new Vector3(2.0f, 0.4f, 0f));
        PlaceCollectible(collectiblePrefab, "Orb_RootLow", new Vector3(2.6f, 1.5f, 0f));
        PlaceCollectible(collectiblePrefab, "Orb_SpiritBack", new Vector3(6.4f, 4.5f, 0f));
        PlaceCollectible(collectiblePrefab, "Orb_HighBranch", new Vector3(9.0f, 7.35f, 0f));
        PlaceCollectible(collectiblePrefab, "Orb_BehindTrunk", new Vector3(11.5f, 6.1f, 0f));

        GameObject glowPath = CreateGlowPath();
        WireSpiritSystems(glowPath);

        CreateFallZone(GroundCenterX);
        ConfigureCamera(spawn.transform, boundsMin, boundsMax);

        new GameObject("Zone1ForestGlade");

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene, ScenePath);
        AssetDatabase.SaveAssets();
        Debug.Log("Zone 1: §C компактно, задник закрывает кадр, без лишнего декора.");
    }

    /// <summary>
    /// Масштабирует GladeBack так, чтобы при любом положении камеры в bounds
    /// по краям не было «пустоты» clear-color.
    /// </summary>
    static void PlaceGladeBackCoveringCamera(Vector2 boundsMin, Vector2 boundsMax)
    {
        Sprite sprite = LoadSprite(GladeBackPath);
        float nativeW = sprite.bounds.size.x;
        float nativeH = sprite.bounds.size.y;
        if (nativeW < 0.01f || nativeH < 0.01f)
            throw new System.InvalidOperationException("GladeBack: нулевой размер спрайта.");

        float aspect = 16f / 9f;
        float viewH = CameraOrtho * 2f;
        float viewW = viewH * aspect;

        // Крайние углы видимости при камере у bounds.
        float coverMinX = boundsMin.x - viewW * 0.5f;
        float coverMaxX = boundsMax.x + viewW * 0.5f;
        float coverMinY = boundsMin.y - viewH * 0.5f;
        float coverMaxY = boundsMax.y + viewH * 0.5f;
        float needW = coverMaxX - coverMinX;
        float needH = coverMaxY - coverMinY;
        float margin = 1.08f;
        float scale = Mathf.Max(needW / nativeW, needH / nativeH) * margin;

        var go = new GameObject("GladeBack");
        go.transform.position = new Vector3(
            (coverMinX + coverMaxX) * 0.5f,
            (coverMinY + coverMaxY) * 0.5f,
            5f);
        go.transform.localScale = new Vector3(scale, scale, 1f);
        var renderer = go.AddComponent<SpriteRenderer>();
        renderer.sprite = sprite;
        renderer.sortingOrder = -20;
    }

    static Sprite LoadSprite(string path)
    {
        Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path);
        if (sprite == null)
            throw new System.InvalidOperationException("Не найден спрайт: " + path);
        return sprite;
    }

    static void ClearLevelObjects()
    {
        string[] removeNames =
        {
            "Ground", "StartPad", "TotoroPad", "ClearingGround", "RecoveryLedge",
            "PlatformMidLow", "PlatformMidHigh", "RestLedge", "FinishPad",
            "PlayerSpawn", "FallRespawnZone", "FinishZone", "LayoutMarker_VerticalBounce",
            "GladeBack", "ForestFront", "BranchOverhead", "BranchLanding", "RootRamp",
            "RootLow", "RootLand", "RootHigh", "RootBehind", "GlowPath",
            "Zone2EntryPad", "Zone2Entry", "Zone1ForestGlade", "SpiritAwakening",
            "GladePlantA", "GladePlantB", "GladePlantB_2",
            "Mushroom_1", "Mushroom_2", "Nut_1", "Seed_1", "Leaf_1", "Dust_1", "Butterfly_1",
            "SeedDecor_1", "SeedDecor_2", "SeedDecor_3",
            "FireflyIdle_1", "FireflyIdle_2", "FireflyIdle_3",
            "Collectible_SeedGround",
            "Orb_Entrance", "Orb_RootLow", "Orb_SpiritBack", "Orb_HighBranch", "Orb_BehindTrunk"
        };

        foreach (string name in removeNames)
        {
            GameObject existing = FindIncludingInactive(name);
            if (existing != null)
                Object.DestroyImmediate(existing);
        }

        foreach (Collectible collectible in Object.FindObjectsByType<Collectible>(FindObjectsInactive.Include))
            Object.DestroyImmediate(collectible.gameObject);

        foreach (FinishZone finish in Object.FindObjectsByType<FinishZone>(FindObjectsInactive.Include))
            Object.DestroyImmediate(finish.gameObject);

        foreach (SpiritAwakening awakening in Object.FindObjectsByType<SpiritAwakening>(FindObjectsInactive.Include))
            Object.DestroyImmediate(awakening.gameObject);
    }

    static GameObject FindIncludingInactive(string name)
    {
        foreach (Transform t in Object.FindObjectsByType<Transform>(
                     FindObjectsInactive.Include, FindObjectsSortMode.None))
        {
            if (t.name == name)
                return t.gameObject;
        }

        return null;
    }

    static GameObject CreateTiledGround(
        string name, float centerX, float topY, float width, int groundLayer, Sprite groundSprite)
    {
        var go = new GameObject(name);
        go.layer = groundLayer;
        go.transform.position = new Vector3(centerX, topY - PlatformHeight, 0f);

        var renderer = go.AddComponent<SpriteRenderer>();
        renderer.sprite = groundSprite;
        renderer.color = Color.white;
        renderer.drawMode = SpriteDrawMode.Tiled;
        renderer.size = new Vector2(width, PlatformHeight);
        renderer.sortingOrder = 0;

        var collider = go.AddComponent<BoxCollider2D>();
        collider.size = new Vector2(width, PlatformHeight);
        collider.offset = new Vector2(0f, PlatformHeight * 0.5f);
        return go;
    }

    static GameObject CreateRootPlatform(
        string name, string spritePath, float centerX, float topY, int groundLayer, int sortingOrder)
    {
        Sprite sprite = LoadSprite(spritePath);
        var go = new GameObject(name);
        go.layer = groundLayer;

        var renderer = go.AddComponent<SpriteRenderer>();
        renderer.sprite = sprite;
        renderer.sortingOrder = sortingOrder;

        go.transform.position = new Vector3(centerX, topY, 0f);
        float topDelta = renderer.bounds.max.y - topY;
        go.transform.position += new Vector3(0f, -topDelta, 0f);

        float width = Mathf.Max(sprite.bounds.size.x * 0.85f, 1.2f);
        var collider = go.AddComponent<BoxCollider2D>();
        collider.size = new Vector2(width, SurfaceThickness);
        float spriteTopLocal = renderer.bounds.max.y - go.transform.position.y;
        collider.offset = new Vector2(0f, spriteTopLocal - SurfaceThickness * 0.5f);
        collider.usedByEffector = true;

        var effector = go.AddComponent<PlatformEffector2D>();
        effector.useOneWay = true;
        effector.useOneWayGrouping = true;
        effector.surfaceArc = 170f;
        return go;
    }

    static void PlaceOrMoveTotoro(Vector3 position)
    {
        GameObject totoro = FindIncludingInactive("Totoro");
        if (totoro == null)
        {
            totoro = new GameObject("Totoro");
            var renderer = totoro.AddComponent<SpriteRenderer>();
            renderer.sprite = LoadSprite(TotoroSpritePath);
            renderer.sortingOrder = 1;

            var belly = totoro.AddComponent<BoxCollider2D>();
            belly.isTrigger = true;
            belly.size = new Vector2(3f, 1.5f);
            belly.offset = new Vector2(0.5f, 2.25f);
            totoro.AddComponent<TotoroBounce>();
        }

        if (totoro.GetComponent<SpiritBreath>() == null)
            totoro.AddComponent<SpiritBreath>();

        totoro.transform.position = position;
        TotoroBounce bounce = totoro.GetComponent<TotoroBounce>();
        if (bounce != null)
        {
            var so = new SerializedObject(bounce);
            so.FindProperty("upwardSpeed").floatValue = 24f;
            so.FindProperty("feetGroundY").floatValue = position.y;
            so.ApplyModifiedPropertiesWithoutUndo();
        }

        TotoroBounce.AlignFeetToGroundY(totoro.transform, position.y);
    }

    static void PlacePlayer(GameObject playerPrefab, Vector3 position)
    {
        GameObject player = FindIncludingInactive("Player");
        if (player != null)
            Object.DestroyImmediate(player);

        player = (GameObject)PrefabUtility.InstantiatePrefab(playerPrefab);
        player.name = "Player";
        player.transform.position = position;
    }

    static void EnsureCollectibleTally()
    {
        if (Object.FindAnyObjectByType<CollectibleTally>() != null)
            return;
        new GameObject("CollectibleTally").AddComponent<CollectibleTally>();
    }

    static void PlaceCollectible(GameObject prefab, string name, Vector3 position)
    {
        var instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
        instance.name = name;
        instance.transform.position = position;
        instance.transform.localScale = Vector3.one * OrbScale;

        Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(OrbSpritePath);
        var renderer = instance.GetComponent<SpriteRenderer>();
        if (renderer != null)
        {
            if (sprite != null)
                renderer.sprite = sprite;
            renderer.sortingOrder = 16;
            renderer.color = new Color(1f, 0.9f, 0.35f, 1f);
        }

        var circle = instance.GetComponent<CircleCollider2D>();
        if (circle != null)
            circle.radius = 0.28f;
    }

    static GameObject CreateGlowPath()
    {
        var root = new GameObject("GlowPath");
        root.transform.position = new Vector3(13.5f, 0.25f, 0f);
        root.transform.localScale = Vector3.one * 2.4f;
        var renderer = root.AddComponent<SpriteRenderer>();
        renderer.sprite = LoadSprite(OrbSpritePath);
        renderer.sortingOrder = 4;
        renderer.color = new Color(1f, 0.95f, 0.45f, 0.95f);
        root.SetActive(false);
        return root;
    }

    static void WireSpiritSystems(GameObject glowPath)
    {
        GameObject totoro = FindIncludingInactive("Totoro");
        var host = new GameObject("SpiritAwakening");
        SpiritAwakening awakening = host.AddComponent<SpiritAwakening>();
        var so = new SerializedObject(awakening);
        so.FindProperty("requiredCount").intValue = 5;
        so.FindProperty("glowPath").objectReferenceValue = glowPath;
        if (totoro != null)
        {
            so.FindProperty("spiritBreath").objectReferenceValue = totoro.GetComponent<SpiritBreath>();
            so.FindProperty("spiritVisual").objectReferenceValue = totoro.transform;
        }
        so.ApplyModifiedPropertiesWithoutUndo();
    }

    static void CreateFallZone(float centerX)
    {
        var go = new GameObject("FallRespawnZone");
        go.transform.position = new Vector3(centerX, -5f, 0f);
        var box = go.AddComponent<BoxCollider2D>();
        box.isTrigger = true;
        box.size = new Vector2(GroundWidth + 8f, 4f);
        go.AddComponent<FallRespawnZone>();
    }

    static void ConfigureCamera(Transform spawn, Vector2 boundsMin, Vector2 boundsMax)
    {
        Camera cam = Camera.main;
        if (cam == null)
        {
            GameObject cameraObject = GameObject.Find("Main Camera");
            if (cameraObject != null)
                cam = cameraObject.GetComponent<Camera>();
        }

        if (cam == null)
            return;

        // Если край задника всё же мелькнет — тот же тон леса, не серая пустота.
        cam.backgroundColor = new Color(0.55f, 0.72f, 0.62f, 1f);
        cam.clearFlags = CameraClearFlags.SolidColor;

        CameraController controller = cam.GetComponent<CameraController>();
        GameObject player = FindIncludingInactive("Player");
        if (controller != null)
        {
            var so = new SerializedObject(controller);
            if (player != null)
                so.FindProperty("target").objectReferenceValue = player.transform;
            so.FindProperty("useBounds").boolValue = true;
            so.FindProperty("boundsMin").vector2Value = boundsMin;
            so.FindProperty("boundsMax").vector2Value = boundsMax;
            so.ApplyModifiedPropertiesWithoutUndo();
        }

        cam.orthographicSize = CameraOrtho;
        cam.transform.position = (player != null ? player.transform.position : spawn.position)
            + new Vector3(0f, 1.5f, -10f);
    }
}
