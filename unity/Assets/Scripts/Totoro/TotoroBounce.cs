using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class TotoroBounce : MonoBehaviour
{
    [SerializeField] float upwardSpeed = 24f;

    /// <summary>Линия контакта с землёй (верх ClearingGround). position.y подстраивается под pivot спрайта.</summary>
    [SerializeField] float feetGroundY;

    void Awake()
    {
        AlignFeetToGroundY(transform, feetGroundY);
    }

    /// <summary>
    /// Ставит transform так, чтобы нижний край спрайта был на groundY.
    /// Использует sprite.pivot (пиксели от низа rect), а не bounds — bounds в Editor часто врёт.
    /// </summary>
    public static void AlignFeetToGroundY(Transform totoroTransform, float groundY)
    {
        if (totoroTransform == null)
            return;

        SpriteRenderer renderer = totoroTransform.GetComponent<SpriteRenderer>();
        if (renderer == null)
            renderer = totoroTransform.GetComponentInChildren<SpriteRenderer>();
        if (renderer == null || renderer.sprite == null)
            return;

        Sprite sprite = renderer.sprite;
        float pivotToBottom = sprite.pivot.y / sprite.pixelsPerUnit;
        Vector3 position = totoroTransform.position;
        float targetY = groundY + pivotToBottom;
        if (Mathf.Abs(position.y - targetY) < 0.001f)
            return;

        totoroTransform.position = new Vector3(position.x, targetY, position.z);
    }

    void Reset()
    {
        BoxCollider2D box = GetComponent<BoxCollider2D>();
        box.isTrigger = true;
        box.usedByEffector = false;
        // Белый живот TotoroSleep: x 1/3–5/6, y 1/2–1 при пивоте низ-центр.
        box.size = new Vector2(3f, 1.5f);
        box.offset = new Vector2(0.5f, 2.25f);
        feetGroundY = 0f;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerVerticalImpulse impulse = other.GetComponent<PlayerVerticalImpulse>();
        if (impulse == null)
            return;

        impulse.Receive(upwardSpeed);
    }
}
