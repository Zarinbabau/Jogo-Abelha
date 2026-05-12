using UnityEngine;
using TMPro;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    [Header("Pontuação")]
    public int TotalScore;
    public TMP_Text scoreText;

    [Header("Tempo")]
    public float tempoDeFase = 90f;
    public TMP_Text timerText;

    [Header("Fim da fase")]
    public TMP_Text endText;

    private bool faseTerminada = false;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        UpdateScoreText();

        if (endText != null)
            endText.gameObject.SetActive(false);
    }

    void Update()
    {
        if (faseTerminada)
            return;

        tempoDeFase -= Time.deltaTime;

        if (tempoDeFase <= 0)
        {
            tempoDeFase = 0;
            FinalizarFase("TEMPO ESGOTADO");
        }

        AtualizarTimer();
    }

    void AtualizarTimer()
    {
        int minutos = Mathf.FloorToInt(tempoDeFase / 60);
        int segundos = Mathf.FloorToInt(tempoDeFase % 60);

        timerText.text = minutos.ToString("00") + ":" +
                         segundos.ToString("00");
    }

    public void AddScore(int valor)
    {
        if (faseTerminada) return;

        TotalScore += valor;
        UpdateScoreText();

        // CONDIÇÃO DE VITÓRIA POR SCORE
        if (TotalScore >= 10)
        {
            FinalizarFase("VOCÊ COLETOU TODOS OS POLENS!");
        }
    }

    public void UpdateScoreText()
    {
        scoreText.text = TotalScore.ToString() + "x";
    }

    void FinalizarFase(string mensagemFinal)
    {
        if (faseTerminada) return;

        faseTerminada = true;

        if (endText != null)
        {
            endText.gameObject.SetActive(true);

            endText.text =
                mensagemFinal + "\n\n" +
                "Pólen entregue: " +
                TotalScore.ToString();
        }

        Time.timeScale = 0f;
    }
}