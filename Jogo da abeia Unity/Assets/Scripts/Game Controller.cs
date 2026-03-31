using UnityEngine;
using TMPro;

public class GameController : MonoBehaviour
{
    public int TotalScore;
    public TMP_Text scoreText;

    public static GameController instance;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        instance = this;
    }

    public void UpdateScoreText()
    {
        scoreText.text = TotalScore.ToString() + "x";
    }

}
