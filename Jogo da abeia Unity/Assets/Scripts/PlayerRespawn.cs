using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint;

    private Rigidbody2D rb;

    private Player player;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        player = GetComponent<Player>();
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
        // Solta o pólen atual
        if (player != null)
        {
            player.DroparPolens();
        }

        // Zera velocidade
        rb.linearVelocity = Vector2.zero;

        // Volta ao spawn
        transform.position = spawnPoint.position;
    }
}