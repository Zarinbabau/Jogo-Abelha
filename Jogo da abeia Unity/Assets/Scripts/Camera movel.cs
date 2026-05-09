using UnityEngine;

public class Camera_movel : MonoBehaviour
{
    public Transform target;

    // LIMITES X
    public float minX = 0f;
    public float maxX = 20f;

    // LIMITES Y
    public float minY = 1f;
    public float maxY = 10f;

    public float smoothSpeed = 5f;
    public Vector2 offset; // só X e Y agora

    private Vector3 velocity = Vector3.zero;

    void LateUpdate()
    {
        if (target == null) return;

        // ===== X =====
        float targetX = target.position.x + offset.x;
        float clampedX = Mathf.Clamp(targetX, minX, maxX);

        // ===== Y =====
        float targetY = target.position.y + offset.y;
        float clampedY = Mathf.Clamp(targetY, minY, maxY);

        // ===== Z FIXO =====
        float fixedZ = -10f;

        Vector3 desiredPosition = new Vector3(
            clampedX,
            clampedY,
            fixedZ
        );

        transform.position = Vector3.SmoothDamp(
            transform.position,
            desiredPosition,
            ref velocity,
            1f / smoothSpeed
        );

        transform.rotation = Quaternion.identity;
    }
}