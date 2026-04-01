using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;

    public float fixedY = 2f;     // Y travado
    public float minX = 0f;       // ponto inicial
    public float maxX = 20f;      // limite m�ximo

    public float smoothSpeed = 5f;
    public Vector3 offset;

    Vector3 velocity = Vector3.zero;

    void LateUpdate()
{
    if (target == null) return;

    float targetX = target.position.x + offset.x;
    float clampedX = Mathf.Clamp(targetX, minX, maxX);

    Vector3 desiredPosition = new Vector3(
        clampedX,
        fixedY,
        offset.z
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

