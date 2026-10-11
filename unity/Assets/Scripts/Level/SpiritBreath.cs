using UnityEngine;

/// <summary>
/// Лёгкое «дыхание» спящего духа (scale). Останавливается на пробуждении.
/// </summary>
public class SpiritBreath : MonoBehaviour
{
    [SerializeField] float amplitude = 0.012f;
    [SerializeField] float speed = 0.55f;

    Vector3 baseScale;
    bool active = true;

    void Awake()
    {
        baseScale = transform.localScale;
    }

    void Update()
    {
        if (!active)
            return;

        float pulse = 1f + Mathf.Sin(Time.time * speed) * amplitude;
        transform.localScale = baseScale * pulse;
    }

    public void Stop()
    {
        active = false;
        transform.localScale = baseScale;
    }
}
