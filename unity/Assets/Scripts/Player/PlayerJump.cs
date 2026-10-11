using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerMovement))]
public class PlayerJump : MonoBehaviour
{
    [SerializeField] float jumpSpeed = 9.5f;

    Rigidbody2D body;
    PlayerMovement movement;

    void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        movement = GetComponent<PlayerMovement>();
    }

    public void TryJump()
    {
        if (!movement.IsGrounded)
            return;

        body.linearVelocity = new Vector2(body.linearVelocity.x, jumpSpeed);
        movement.LeaveGround();
    }
}
