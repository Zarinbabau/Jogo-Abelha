using UnityEngine;

public class Polen : MonoBehaviour
{
    public int Score = 1;

    private Vector3 startPosition;

    private bool entregue = false;

    void Start()
    {
        startPosition = transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (entregue)
            return;

        if (collision.CompareTag("Player"))
        {
            Player player =
                collision.GetComponent<Player>();

            if (player != null &&
                player.PodeColetar())
            {
                player.ColetarPolen(this);

                gameObject.SetActive(false);
            }
        }
    }

    public void Entregar()
    {
        entregue = true;

        gameObject.SetActive(false);
    }

    public void Respawn()
    {
        if (entregue)
            return;

        transform.position = startPosition;

        gameObject.SetActive(true);
    }
}