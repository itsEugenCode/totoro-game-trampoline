using UnityEditor;

/// <summary>
/// Совместимость: старое меню собирает зону 1 (лесная поляна).
/// </summary>
public static class FirstLevelSetup
{
    [MenuItem("Totoro/Build First Level")]
    public static void Build()
    {
        Zone1ForestGladeSetup.Build();
    }
}
