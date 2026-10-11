using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class FallRespawnZone : MonoBehaviour
{
    void Reset()
    {
        Collider2D col = GetComponent<Collider2D>();
        col.isTrigger = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<CollectibleCollector>() == null)
            return;

        RespawnPoint point = RespawnPoint.Current;
        if (point == null)
            return;

        Rigidbody2D body = other.attachedRigidbody;
        if (body == null)
            body = other.GetComponent<Rigidbody2D>();
        if (body == null)
            return;

        Vector3 spawn = point.SpawnPosition;
        body.linearVelocity = Vector2.zero;
        body.angularVelocity = 0f;
        body.position = spawn;
        body.transform.position = spawn;
    }
}
