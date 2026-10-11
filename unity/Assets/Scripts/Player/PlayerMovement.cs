using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float maxSpeed = 6f;
    [SerializeField] float groundAcceleration = 40f;
    [SerializeField] float groundDeceleration = 50f;
    [SerializeField] float airMaxSpeed = 6f;
    [SerializeField] float airAcceleration = 20f;
    [SerializeField] float gravityScale = 3f;
    [SerializeField] float groundCheckDistance = 0.08f;
    [SerializeField] float groundedVelocityTolerance = 0.01f;
    [SerializeField] LayerMask groundMask;

    Rigidbody2D body;
    Collider2D bodyCollider;
    float moveInput;
    bool grounded;

    public bool IsGrounded => grounded;

    public void LeaveGround()
    {
        grounded = false;
    }

    void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        bodyCollider = GetComponent<Collider2D>();
        ApplyGravityScale();
    }

    void OnValidate()
    {
        body = GetComponent<Rigidbody2D>();
        ApplyGravityScale();
    }

    public void SetMoveInput(float input)
    {
        moveInput = Mathf.Clamp(input, -1f, 1f);
    }

    void FixedUpdate()
    {
        ApplyGravityScale();
        UpdateGrounded();

        bool hasInput = Mathf.Abs(moveInput) > 0f;
        float nextSpeed = body.linearVelocity.x;

        // В воздухе без команды горизонтальная скорость не меняется.
        if (grounded)
        {
            float targetSpeed = hasInput ? moveInput * maxSpeed : 0f;
            float acceleration = hasInput ? groundAcceleration : groundDeceleration;
            nextSpeed = Mathf.MoveTowards(nextSpeed, targetSpeed, acceleration * Time.fixedDeltaTime);
        }
        else if (hasInput)
        {
            float targetSpeed = moveInput * airMaxSpeed;
            nextSpeed = Mathf.MoveTowards(nextSpeed, targetSpeed, airAcceleration * Time.fixedDeltaTime);
        }

        // Горизонталь только здесь; y оставляем — прыжок и отскок пишут вертикаль сами.
        body.linearVelocity = new Vector2(nextSpeed, body.linearVelocity.y);
    }

    void ApplyGravityScale()
    {
        if (body != null)
            body.gravityScale = gravityScale;
    }

    void UpdateGrounded()
    {
        // Луч стартует в центре тела. Слой персонажа не входит в маску,
        // поэтому длина до края коллайдера плюс зазор как раз достаёт до земли.
        float distance = bodyCollider.bounds.extents.y + groundCheckDistance;
        RaycastHit2D hit = Physics2D.Raycast(body.position, Vector2.down, distance, groundMask);
        bool supported = hit.collider != null;
        grounded = supported && body.linearVelocity.y <= groundedVelocityTolerance;
    }
}
