using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;

    public float fixedY = 2f;     // Y travado
    public float minX = 0f;       // ponto inicial
    public float maxX = 20f;      // limite máximo

    public float smoothSpeed = 5f;
    public Vector3 offset;

    void LateUpdate()
    {
        if (target == null) return;

        // pega posição desejada
        float targetX = target.position.x + offset.x;

        // limita movimento no eixo X
        float clampedX = Mathf.Clamp(targetX, minX, maxX);

        Vector3 desiredPosition = new Vector3(
            clampedX,
            fixedY,
            offset.z
        );

        transform.position = Vector3.Lerp(
            transform.position,
            desiredPosition,
            smoothSpeed * Time.deltaTime
        );

        // mantém rotação fixa
        transform.rotation = Quaternion.identity;
    }
}