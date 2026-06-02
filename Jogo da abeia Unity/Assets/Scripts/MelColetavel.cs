using UnityEngine;

public class MelColetavel : MonoBehaviour
{
    [SerializeField] private int score = 1;

    private bool coletado = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (coletado)
            return;

        if (!collision.CompareTag("Player"))
            return;

        coletado = true;

        if (Labirinto.instance != null)
        {
            Labirinto.instance.AddScore(score);
        }

        Destroy(gameObject);
    }
}