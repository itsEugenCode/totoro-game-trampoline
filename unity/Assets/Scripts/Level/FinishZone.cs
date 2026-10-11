using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class FinishZone : MonoBehaviour
{
    [SerializeField] bool reached;
    [SerializeField] bool logOnReach = true;

    public bool Reached => reached;

    void Reset()
    {
        Collider2D col = GetComponent<Collider2D>();
        col.isTrigger = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (reached)
            return;

        if (other.GetComponent<CollectibleCollector>() == null)
            return;

        reached = true;
        if (logOnReach)
            Debug.Log("First level finish reached");
    }
}
