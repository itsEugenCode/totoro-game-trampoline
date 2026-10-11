using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class ClearingSceneSetup
{
    const string ScenePath = "Assets/Scenes/Clearing.unity";
    const string PrefabPath = "Assets/Prefabs/Player.prefab";

    const float PlayerSize = 1f;
    const float GroundWidth = 24f;
    const float GroundHeight = 1f;

    public static void Build()
    {
        int groundLayer = LayerMask.NameToLayer("Ground");
        if (groundLayer < 0)
            throw new System.InvalidOperationException("Слой Ground не найден.");

        EnsureFolder("Assets/Scenes");
        EnsureFolder("Assets/Prefabs");

        var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

        var ground = new GameObject("Ground");
        ground.layer = groundLayer;
        ground.transform.position = new Vector3(0f, -GroundHeight * 0.5f, 0f);
        ground.transform.localScale = new Vector3(GroundWidth, GroundHeight, 1f);
        var groundRenderer = ground.AddComponent<SpriteRenderer>();
        groundRenderer.color = new Color(0.45f, 0.62f, 0.38f);
        var groundCollider = ground.AddComponent<BoxCollider2D>();
        groundCollider.size = Vector2.one;

        var player = new GameObject("Player");
        player.layer = LayerMask.NameToLayer("Default");
        player.transform.position = new Vector3(0f, PlayerSize * 0.5f, 0f);
        var playerRenderer = player.AddComponent<SpriteRenderer>();
        playerRenderer.sortingOrder = 1;
        playerRenderer.color = new Color(0.86f, 0.45f, 0.48f);
        var playerCollider = player.AddComponent<BoxCollider2D>();
        playerCollider.size = new Vector2(PlayerSize, PlayerSize);

        var body = player.AddComponent<Rigidbody2D>();
        body.gravityScale = 3f;
        body.freezeRotation = true;
        body.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        body.interpolation = RigidbodyInterpolation2D.Interpolate;

        var movement = player.AddComponent<PlayerMovement>();
        player.AddComponent<PlayerJump>();
        player.AddComponent<PlayerController>();

        var movementObject = new SerializedObject(movement);
        movementObject.FindProperty("groundMask").intValue = 1 << groundLayer;
        movementObject.ApplyModifiedPropertiesWithoutUndo();

        var cameraObject = new GameObject("Main Camera");
        cameraObject.tag = "MainCamera";
        var camera = cameraObject.AddComponent<Camera>();
        camera.orthographic = true;
        camera.orthographicSize = 5f;
        camera.backgroundColor = new Color(0.64f, 0.78f, 0.72f);
        camera.clearFlags = CameraClearFlags.SolidColor;
        cameraObject.AddComponent<AudioListener>();
        var cameraController = cameraObject.AddComponent<CameraController>();
        var cameraProperties = new SerializedObject(cameraController);
        cameraProperties.FindProperty("target").objectReferenceValue = player.transform;
        cameraProperties.ApplyModifiedPropertiesWithoutUndo();
        cameraObject.transform.position = player.transform.position + new Vector3(0f, 1f, -10f);

        PrefabUtility.SaveAsPrefabAssetAndConnect(player, PrefabPath, InteractionMode.AutomatedAction);
        Sprite square = CreateWhiteSquare(PrefabPath);
        playerRenderer.sprite = square;
        groundRenderer.sprite = square;
        var prefabRenderer = AssetDatabase.LoadAssetAtPath<GameObject>(PrefabPath).GetComponent<SpriteRenderer>();
        prefabRenderer.sprite = square;
        EditorUtility.SetDirty(prefabRenderer);
        EditorSceneManager.SaveScene(scene, ScenePath);

        EditorBuildSettings.scenes = new[] { new EditorBuildSettingsScene(ScenePath, true) };
        AssetDatabase.SaveAssets();
        Debug.Log("Clearing scene is ready.");
    }

    static Sprite CreateWhiteSquare(string assetPath)
    {
        var texture = new Texture2D(4, 4, TextureFormat.RGBA32, false);
        texture.name = "Square";
        var pixels = new Color[16];
        for (int i = 0; i < pixels.Length; i++)
            pixels[i] = Color.white;
        texture.SetPixels(pixels);
        texture.Apply(false, false);

        var sprite = Sprite.Create(
            texture,
            new Rect(0, 0, 4, 4),
            new Vector2(0.5f, 0.5f),
            4f,
            0,
            SpriteMeshType.FullRect);
        sprite.name = "Square";

        AssetDatabase.AddObjectToAsset(texture, assetPath);
        AssetDatabase.AddObjectToAsset(sprite, assetPath);
        return sprite;
    }

    static void EnsureFolder(string path)
    {
        if (AssetDatabase.IsValidFolder(path))
            return;

        string parent = Path.GetDirectoryName(path).Replace('\\', '/');
        string folderName = Path.GetFileName(path);
        if (!AssetDatabase.IsValidFolder(parent))
            EnsureFolder(parent);
        AssetDatabase.CreateFolder(parent, folderName);
    }
}
