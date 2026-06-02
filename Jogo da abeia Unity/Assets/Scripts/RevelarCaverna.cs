using UnityEngine;

public class CavernaOcclusion : MonoBehaviour
{
    [SerializeField] private SpriteRenderer cavernaExterno;

    private bool playerDentro = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        playerDentro = true;

        cavernaExterno.enabled = false;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        playerDentro = false;

        // Se o player saiu pela esquerda
        if (other.transform.position.x < transform.position.x)
        {
            cavernaExterno.enabled = true;
        }
        // Se saiu pela direita
        else
        {
            cavernaExterno.enabled = false;
        }
    }

    private void Update()
    {
        if (playerDentro)
        {
            cavernaExterno.enabled = false;
        }
    }
}