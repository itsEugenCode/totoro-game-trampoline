using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerJump))]
public class PlayerController : MonoBehaviour
{
    PlayerMovement movement;
    PlayerJump jump;

    void Awake()
    {
        movement = GetComponent<PlayerMovement>();
        jump = GetComponent<PlayerJump>();
    }

    void Update()
    {
        movement.SetMoveInput(Input.GetAxisRaw("Horizontal"));

        if (Input.GetButtonDown("Jump"))
            jump.TryJump();
    }
}
