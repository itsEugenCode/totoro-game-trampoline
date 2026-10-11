using UnityEngine;

public class CollectibleCollector : MonoBehaviour
{
    CollectibleTally tally;

    void Awake()
    {
        tally = FindFirstObjectByType<CollectibleTally>();
    }

    public void Collect(Collectible item)
    {
        if (item == null)
            return;

        if (tally == null)
            tally = FindFirstObjectByType<CollectibleTally>();

        if (!item.TryConsume())
            return;

        if (tally != null)
            tally.Add(1);
    }
}
