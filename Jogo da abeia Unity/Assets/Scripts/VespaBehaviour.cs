using UnityEngine;

public class VespaBehaviour : MonoBehaviour
{
    [SerializeField] private float speed = 3f;

    private Transform target;
    private Vector3 startPosition;

    private bool isChasing = false;

    // Referência da área
    private Transform areaCenter;
    private float areaRadius;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        if (isChasing && target != null)
        {
            transform.position = Vector2.MoveTowards(
                transform.position,
                target.position,
                speed * Time.deltaTime
            );

            // 🔴 Checa se saiu da área
            float distance = Vector2.Distance(transform.position, areaCenter.position);

            if (distance > areaRadius)
            {
                StopChasing();
            }
        }
        else
        {
            transform.position = Vector2.MoveTowards(
                transform.position,
                startPosition,
                speed * Time.deltaTime
            );
        }
    }

    public void SetTarget(Transform newTarget, Transform area, float radius)
    {
        target = newTarget;
        areaCenter = area;
        areaRadius = radius;
        isChasing = true;
    }

    public void StopChasing()
    {
        isChasing = false;
        target = null;
    }
}