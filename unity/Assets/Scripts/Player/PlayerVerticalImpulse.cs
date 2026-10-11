using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[DefaultExecutionOrder(-50)]
public class PlayerVerticalImpulse : MonoBehaviour
{
    Rigidbody2D body;
    bool pending;
    bool keepHorizontal;
    float upwardSpeed;
    float horizontalSpeed;

    void Awake()
    {
        body = GetComponent<Rigidbody2D>();
    }

    public void KeepHorizontal(float speed)
    {
        horizontalSpeed = speed;
        keepHorizontal = true;
    }

    public void Receive(float upwardSpeed)
    {
        this.upwardSpeed = upwardSpeed;
        if (!keepHorizontal)
            horizontalSpeed = body.linearVelocity.x;
        pending = true;
    }

    void FixedUpdate()
    {
        if (!pending)
            return;

        pending = false;
        keepHorizontal = false;
        // Кадр столкновения уже прошёл: солвер не затирает y и не оставляет x обнулённым трением.
        body.linearVelocity = new Vector2(horizontalSpeed, upwardSpeed);
    }
}
