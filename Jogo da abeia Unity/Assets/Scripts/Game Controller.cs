using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    [Header("Referências")]
    public Player player;
    public InventoryUI inventoryUI;

    [Header("Pontuação")]
    public int TotalScore = 0;
    public TMP_Text scoreText;

    [Header("Tempo")]
    public float tempoDeFase = 90f;
    public TMP_Text timerText;

    [Header("Fim da fase")]
    public TMP_Text endText;

    [Header("Próxima fase")]
    [SerializeField] private string proximaCena = "Fase 3";

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

        if (inventoryUI != null && player != null)
        {
            inventoryUI.Init(player);
        }

        Debug.Log("GameController iniciado.");
    }

    void Update()
    {
        if (faseTerminada)
            return;

        tempoDeFase -= Time.deltaTime;

        if (tempoDeFase <= 0)
        {
            tempoDeFase = 0;
            FinalizarFase(false);
        }

        AtualizarTimer();
    }

    void AtualizarTimer()
    {
        if (timerText == null)
            return;

        int minutos = Mathf.FloorToInt(tempoDeFase / 60);
        int segundos = Mathf.FloorToInt(tempoDeFase % 60);

        timerText.text = minutos.ToString("00") + ":" +
                         segundos.ToString("00");
    }

    public void AddScore(int valor)
    {
        if (faseTerminada)
            return;

        TotalScore += valor;

        UpdateScoreText();

        if (TotalScore >= 12)
        {
            FinalizarFase(true);
        }
    }

    void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = TotalScore + "/12";
        }
    }

    void FinalizarFase(bool venceu)
    {
        if (faseTerminada)
            return;

        faseTerminada = true;

        Debug.Log("FinalizarFase");

        Player p = FindFirstObjectByType<Player>();

        if (p != null)
        {
            p.TravarMovimento();
        }
        else
        {
            Debug.LogWarning("Player não encontrado.");
        }

        if (endText != null)
        {
            endText.gameObject.SetActive(true);

            if (venceu)
            {
                endText.text =
                    "PARABÉNS! Agora podemos começar a fazer um\nmel delicioso";
            }
            else
            {
                endText.text =
                    "TEMPO ESGOTADO\n\nPólens entregues: " + TotalScore;
            }
        }

        if (venceu)
        {
            StartCoroutine(CarregarIntro());
        }
        else
        {
            StartCoroutine(ReiniciarFase());
        }
    }

    IEnumerator CarregarIntro()
    {
        Debug.Log("Coroutine iniciada.");

        Time.timeScale = 1f;

        IntroFase.proximaFase = proximaCena;

        Debug.Log("Próxima fase: " + IntroFase.proximaFase);

        yield return new WaitForSecondsRealtime(2f);

        Debug.Log("Carregando Intro");

        SceneManager.LoadScene("Intro");
    }

    IEnumerator ReiniciarFase()
    {
        yield return new WaitForSecondsRealtime(2f);

        Time.timeScale = 1f;

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().name
        );
    }
}