using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") ||
            collision.gameObject.CompareTag("Vespa"))
        {
            Respawn();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") ||
            collision.CompareTag("Vespa"))
        {
            Respawn();
        }
    }

    void Respawn()
    {
        // Zera velocidade
        rb.linearVelocity = Vector2.zero;

        // Vai para o SpawnPoint
        transform.position = spawnPoint.position;

        GameController.instance.ResetGame();
    }
}