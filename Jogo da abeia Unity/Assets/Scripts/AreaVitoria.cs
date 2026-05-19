using UnityEngine;

public class AreaVitoria : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Labirinto.instance.Vitoria();
        }
    }
}