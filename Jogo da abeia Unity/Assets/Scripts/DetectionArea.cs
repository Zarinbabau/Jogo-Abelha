using UnityEngine;

public class DetectionArea : MonoBehaviour
{
    [SerializeField] private VespaBehaviour vespa;
    private CircleCollider2D circle;

    void Start()
    {
        circle = GetComponent<CircleCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            float radius = circle.radius * transform.lossyScale.x;

            vespa.SetTarget(
                collision.transform,
                transform,
                radius
            );
        }
    }
}