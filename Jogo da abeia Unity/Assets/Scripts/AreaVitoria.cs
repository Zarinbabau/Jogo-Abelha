using UnityEngine;

public class AreaEntregaMel : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
            return;

        Labirinto.instance.EntregarMel();
    }
}