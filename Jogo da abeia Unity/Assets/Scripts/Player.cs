using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    private Rigidbody2D rb;

    [Header("Movimento")]
    public float moveSpeed = 6f;
    public float liftForce = 50f;
    public float maxVerticalSpeed = 25f;

    [Header("Suavização")]
    public float horizontalSmooth = 0.2f;

    [Header("Rotação Helicóptero")]
    public float tiltAngle = 25f;
    public float tiltSpeed = 5f;

    float horizontalInput;
    bool liftInput;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        horizontalInput = 0f;

        if (Input.GetKey(KeyCode.A))
            horizontalInput = -1f;

        if (Input.GetKey(KeyCode.D))
            horizontalInput = 1f;

        liftInput = Input.GetKey(KeyCode.W);

   
    }

   void FixedUpdate()
{

    HandleRotation();

    Vector2 velocity = rb.linearVelocity;

    // Movimento horizontal direto (arcade e estável)
    velocity.x = horizontalInput * moveSpeed;

    // Subida estilo helicóptero
    if (liftInput)
    {
        velocity.y += liftForce * Time.fixedDeltaTime;
    }

    rb.linearVelocity = velocity;
}


void HandleRotation()
{
    float targetAngle = -horizontalInput * tiltAngle;
    transform.rotation = Quaternion.Euler(0, 0, targetAngle);
}
}