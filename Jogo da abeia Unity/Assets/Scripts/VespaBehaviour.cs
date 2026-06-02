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

        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            lookTarget = player.transform;
        }
        else
        {
            Debug.LogError("Nenhum objeto com a tag 'Player' foi encontrado.");
        }
    }

    void Update()
    {
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

            float distance = Vector2.Distance(
                transform.position,
                areaCenter.position
            );

            if (distance > areaRadius)
            {
                StopChasing();
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
        if (lookTarget == null ||
            frente == null ||
            vespaVisual == null)
            return;

        // Calcula a direção usando a posição da cabeça
        Vector2 direction =
            (Vector2)(lookTarget.position - frente.position);

        float angle =
            Mathf.Atan2(direction.y, direction.x) *
            Mathf.Rad2Deg;

        // Se a arte da vespa foi desenhada olhando para a direita
        vespaVisual.rotation =
     Quaternion.Euler(0f, 0f, angle - 90f);

        /*
         * Se a cabeça não ficar alinhada,
         * teste uma destas opções:
         *
         * angle + 90f
         * angle - 90f
         * angle + 180f
         */
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