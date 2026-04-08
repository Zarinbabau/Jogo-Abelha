using UnityEngine;

public class Polen : MonoBehaviour
{
    public int Score;
    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameController.instance.TotalScore += Score;
            GameController.instance.UpdateScoreText();

            gameObject.SetActive(false); // 🔥 em vez de Destroy
        }
    }

    public void Respawn()
    {
        transform.position = startPosition;
        gameObject.SetActive(true);
    }
}