using System;
using UnityEngine;

public class CollectibleTally : MonoBehaviour
{
    [SerializeField] int collectedCount;
    [SerializeField] bool logToConsole = true;

    public int Count => collectedCount;

    public event Action<int> CountChanged;

    public void Add(int amount)
    {
        if (amount <= 0)
            return;

        collectedCount += amount;
        if (logToConsole)
            Debug.Log($"Collectibles: {collectedCount}", this);

        CountChanged?.Invoke(collectedCount);
    }
}
