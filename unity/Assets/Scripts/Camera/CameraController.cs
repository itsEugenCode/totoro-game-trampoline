using UnityEngine;
using UnityEngine.Serialization;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] Vector3 offset = new Vector3(0f, 1f, -10f);

    [FormerlySerializedAs("smoothTime")]
    [SerializeField] float smoothTimeX = 0.15f;
    [SerializeField] float smoothTimeY = 0.25f;

    [SerializeField] float verticalDeadZone = 0.9f;

    [Tooltip("Прыжки с max vy ниже этого — низкие: камера стартует с задержкой.")]
    [SerializeField] float highJumpMinSpeed = 14f;
    [Tooltip("Насколько высоко персонаж должен подняться, прежде чем камера начнёт ехать за низким прыжком.")]
    [SerializeField] float lowJumpFollowDelayHeight = 0.85f;
    [SerializeField] float lowJumpSmoothTimeY = 0.45f;

    [SerializeField] float lookAheadUp = 2.5f;
    [SerializeField] float lookAheadDown = 2.2f;
    [SerializeField] float lookAheadVelocityThreshold = 8f;
    [SerializeField] float highJumpSmoothTimeY = 0.12f;
    [SerializeField] float fallSmoothTimeY = 0.1f;

    [Tooltip("Когда подъём замедляется ниже этого vy, камера замирает у вершины высокого прыжка.")]
    [SerializeField] float apexHoldVy = 4f;
    [SerializeField] float apexHoldSmoothTimeY = 0.08f;

    [SerializeField] bool useBounds;
    [SerializeField] Vector2 boundsMin = new Vector2(-9f, 1f);
    [SerializeField] Vector2 boundsMax = new Vector2(9f, 16f);

    Rigidbody2D targetBody;
    float velocityX;
    float velocityY;
    float followY;
    bool followYReady;

    bool airborne;
    bool highJump;
    bool apexHeld;
    float jumpStartY;
    float maxAscentVy;
    float apexHoldY;
    bool wasFalling;

    void Awake()
    {
        CacheTargetBody();
    }

    void OnValidate()
    {
        CacheTargetBody();
    }

    void Start()
    {
        SnapToTarget();
    }

    void LateUpdate()
    {
        if (target == null)
            return;

        float vy = targetBody != null ? targetBody.linearVelocity.y : 0f;
        float targetY = target.position.y;
        UpdateJumpTracking(vy, targetY);

        float lookAhead = 0f;
        float dampY = smoothTimeY;
        bool freezeVertical = false;

        if (airborne && highJump)
        {
            if (vy > apexHoldVy)
            {
                // Высокий подъём: опережаем вверх.
                float t = Mathf.Clamp01(vy / Mathf.Max(0.01f, lookAheadVelocityThreshold));
                lookAhead = lookAheadUp * t;
                dampY = highJumpSmoothTimeY;
                apexHeld = false;
            }
            else if (vy >= 0f)
            {
                // Вершина: камера замирает, персонаж ещё чуть поднимается.
                if (!apexHeld)
                {
                    apexHoldY = followY;
                    apexHeld = true;
                }
                freezeVertical = true;
                dampY = apexHoldSmoothTimeY;
            }
            else
            {
                // Спуск: камера уходит вниз чуть раньше персонажа.
                apexHeld = false;
                float t = Mathf.Clamp01(-vy / Mathf.Max(0.01f, lookAheadVelocityThreshold));
                lookAhead = -lookAheadDown * t;
                dampY = fallSmoothTimeY;
            }
        }
        else if (airborne)
        {
            // Низкий прыжок: без look-ahead вверх, следование только после задержки по высоте.
            float rise = targetY - jumpStartY;
            if (rise < lowJumpFollowDelayHeight && vy >= 0f)
            {
                freezeVertical = true;
                dampY = lowJumpSmoothTimeY;
            }
            else if (vy < 0f)
            {
                float t = Mathf.Clamp01(-vy / Mathf.Max(0.01f, lookAheadVelocityThreshold));
                lookAhead = -lookAheadDown * t * 0.5f;
                dampY = Mathf.Lerp(smoothTimeY, fallSmoothTimeY, t * 0.5f);
            }
            else
            {
                dampY = lowJumpSmoothTimeY;
            }
        }

        float targetFocusY = targetY + offset.y + lookAhead;
        if (!followYReady)
        {
            followY = targetFocusY;
            followYReady = true;
        }

        if (freezeVertical)
        {
            if (apexHeld)
                followY = apexHoldY;
            // иначе followY не двигаем вверх — задержка старта на низком прыжке
        }
        else
        {
            float halfDead = Mathf.Max(0f, verticalDeadZone) * 0.5f;
            if (targetFocusY > followY + halfDead)
                followY = targetFocusY - halfDead;
            else if (targetFocusY < followY - halfDead)
                followY = targetFocusY + halfDead;
        }

        float desiredX = target.position.x + offset.x;
        float desiredY = followY;
        float desiredZ = target.position.z + offset.z;

        if (useBounds)
        {
            desiredX = Mathf.Clamp(desiredX, boundsMin.x, boundsMax.x);
            desiredY = Mathf.Clamp(desiredY, boundsMin.y, boundsMax.y);
            followY = desiredY;
            if (apexHeld)
                apexHoldY = desiredY;
        }

        Vector3 current = transform.position;
        float x = Mathf.SmoothDamp(current.x, desiredX, ref velocityX, smoothTimeX);
        float y = Mathf.SmoothDamp(current.y, desiredY, ref velocityY, dampY);
        transform.position = new Vector3(x, y, desiredZ);
    }

    void UpdateJumpTracking(float vy, float targetY)
    {
        const float leaveGroundVy = 1.5f;
        const float landedVy = 0.2f;
        const float landedHeightSlack = 0.35f;

        if (!airborne)
        {
            if (vy > leaveGroundVy)
            {
                airborne = true;
                highJump = false;
                apexHeld = false;
                wasFalling = false;
                jumpStartY = targetY;
                maxAscentVy = vy;
            }
            return;
        }

        if (vy > maxAscentVy)
            maxAscentVy = vy;

        if (!highJump && maxAscentVy >= highJumpMinSpeed)
            highJump = true;

        if (vy < -0.5f)
            wasFalling = true;

        bool nearStartHeight = targetY <= jumpStartY + landedHeightSlack;
        if (wasFalling && Mathf.Abs(vy) <= landedVy && nearStartHeight)
        {
            airborne = false;
            highJump = false;
            apexHeld = false;
            wasFalling = false;
            maxAscentVy = 0f;
        }
    }

    void SnapToTarget()
    {
        if (target == null)
            return;

        CacheTargetBody();
        followY = target.position.y + offset.y;
        followYReady = true;
        velocityX = 0f;
        velocityY = 0f;
        airborne = false;
        highJump = false;
        apexHeld = false;
        wasFalling = false;
        maxAscentVy = 0f;

        Vector3 snapped = new Vector3(
            target.position.x + offset.x,
            followY,
            target.position.z + offset.z);

        if (useBounds)
        {
            snapped.x = Mathf.Clamp(snapped.x, boundsMin.x, boundsMax.x);
            snapped.y = Mathf.Clamp(snapped.y, boundsMin.y, boundsMax.y);
            followY = snapped.y;
        }

        transform.position = snapped;
    }

    void CacheTargetBody()
    {
        targetBody = target != null ? target.GetComponent<Rigidbody2D>() : null;
    }
}
