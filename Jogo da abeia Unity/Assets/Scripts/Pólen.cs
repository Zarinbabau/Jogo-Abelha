using UnityEngine;

public class Pólen : MonoBehaviour
{
    public int Score;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {

            GameController.instance.TotalScore += Score;
            GameController.instance.UpdateScoreText();

            Destroy(gameObject);

        }
    }
}
