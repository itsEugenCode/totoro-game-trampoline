using UnityEngine;

public class RespawnPoint : MonoBehaviour
{
    public static RespawnPoint Current { get; private set; }

    [SerializeField] bool activateOnAwake = true;

    void Awake()
    {
        if (activateOnAwake || Current == null)
            Current = this;
    }

    public void Activate()
    {
        Current = this;
    }

    public Vector3 SpawnPosition => transform.position;
}
