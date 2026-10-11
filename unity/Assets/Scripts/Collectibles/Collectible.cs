using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Collectible : MonoBehaviour
{
    Collider2D zone;
    bool collected;

    void Awake()
    {
        zone = GetComponent<Collider2D>();
        zone.isTrigger = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        CollectibleCollector collector = other.GetComponent<CollectibleCollector>();
        if (collector == null)
            return;

        collector.Collect(this);
    }

    public bool TryConsume()
    {
        if (collected)
            return false;

        collected = true;
        if (zone != null)
            zone.enabled = false;

        gameObject.SetActive(false);
        return true;
    }
}
