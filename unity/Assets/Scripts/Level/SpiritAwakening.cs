using System.Collections;
using UnityEngine;

/// <summary>
/// Один раз после 5/5: затишье → лёгкий placeholder пробуждения → светящаяся тропинка.
/// </summary>
public class SpiritAwakening : MonoBehaviour
{
    [SerializeField] int requiredCount = 5;
    [SerializeField] GameObject glowPath;
    [SerializeField] SpiritBreath spiritBreath;
    [SerializeField] Transform spiritVisual;
    [SerializeField] float hushSeconds = 0.5f;
    [SerializeField] float pulseSeconds = 0.8f;

    CollectibleTally tally;
    bool played;

    void OnEnable()
    {
        tally = FindFirstObjectByType<CollectibleTally>();
        if (tally != null)
            tally.CountChanged += OnCountChanged;
    }

    void OnDisable()
    {
        if (tally != null)
            tally.CountChanged -= OnCountChanged;
    }

    void Start()
    {
        if (glowPath != null)
            glowPath.SetActive(false);

        if (tally == null)
            tally = FindFirstObjectByType<CollectibleTally>();

        if (tally != null && tally.Count >= requiredCount)
            TryPlay();
    }

    void OnCountChanged(int count)
    {
        if (count >= requiredCount)
            TryPlay();
    }

    void TryPlay()
    {
        if (played)
            return;

        played = true;
        StartCoroutine(PlaySequence());
    }

    IEnumerator PlaySequence()
    {
        if (spiritBreath != null)
            spiritBreath.Stop();

        yield return new WaitForSeconds(hushSeconds);

        if (spiritVisual != null)
        {
            Vector3 baseScale = spiritVisual.localScale;
            float elapsed = 0f;
            while (elapsed < pulseSeconds)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / pulseSeconds);
                float bump = 1f + 0.04f * Mathf.Sin(t * Mathf.PI);
                spiritVisual.localScale = baseScale * bump;
                yield return null;
            }

            spiritVisual.localScale = baseScale;
        }

        if (glowPath != null)
            glowPath.SetActive(true);

        Debug.Log("SpiritAwakening: пробуждение завершено, тропинка видна.", this);
    }
}
