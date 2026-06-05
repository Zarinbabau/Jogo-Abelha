using UnityEngine;

public class VespaBehaviour : MonoBehaviour
{
    [Header("Movimento")]
    [SerializeField] private float speed = 3f;

    [Header("Referências")]
    [SerializeField] private Transform vespaVisual; // Arraste Vespa_0
    [SerializeField] private Transform frente;      // Arraste o Empty Frente

    private Transform target;
    private Transform lookTarget;

    private Vector3 startPosition;

    private bool isChasing = false;

    private Transform areaCenter;
    private float areaRadius;

    void Start()
    {
        startPosition = transform.position;

        Player p = FindFirstObjectByType<Player>();

        if (p != null)
        {
            lookTarget = p.transform;
            Debug.Log("Player encontrado pelo componente.");
        }
        else
        {
            Debug.LogWarning("Player não encontrado no Start.");
        }
    }

    void Update()
    {
        // Se ainda não encontrou o jogador, tenta novamente
        if (lookTarget == null)
        {
            Player p = FindFirstObjectByType<Player>();

            if (p != null)
            {
                lookTarget = p.transform;
                Debug.Log("Player encontrado pelo componente.");
            }
            else
            {
                Debug.LogWarning("Player ainda não encontrado.");
            }
        }

        // Sempre olha para o jogador
        LookAtPlayer();

        // Movimento de perseguição
        if (isChasing && target != null)
        {
            transform.position = Vector2.MoveTowards(
                transform.position,
                target.position,
                speed * Time.deltaTime
            );

            if (areaCenter != null)
            {
                float distance = Vector2.Distance(
                    transform.position,
                    areaCenter.position
                );

                if (distance > areaRadius)
                {
                    StopChasing();
                }
            }
        }
        else
        {
            // Volta para a posição inicial
            transform.position = Vector2.MoveTowards(
                transform.position,
                startPosition,
                speed * Time.deltaTime
            );
        }
    }

    private void LookAtPlayer()
    {
        if (lookTarget == null)
        {
            Debug.LogError("lookTarget NULL");
            return;
        }

        if (frente == null)
        {
            Debug.LogError("frente NULL");
            return;
        }

        if (vespaVisual == null)
        {
            Debug.LogError("vespaVisual NULL");
            return;
        }

        if (lookTarget == null || vespaVisual == null)
            return;

        Vector2 direction =
            (Vector2)(lookTarget.position - transform.position);

        float angle =
            Mathf.Atan2(direction.y, direction.x) *
            Mathf.Rad2Deg;

        vespaVisual.rotation =
            Quaternion.Euler(0f, 0f, angle - 90f);

    }

    public void SetTarget(
        Transform newTarget,
        Transform area,
        float radius)
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