using UnityEngine;
using UnityEngine.SceneManagement;

public class VoltarMenu : MonoBehaviour
{
    public string nomeMenu = "Menu";

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Saiu pela esquerda do trigger
            if (other.transform.position.x < transform.position.x)
            {
                SceneManager.LoadScene(nomeMenu);
            }
        }
    }
}