using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    public Player player;
    public InventoryUI inventoryUI;

    [Header("Pontuação")]
    public int TotalScore;
    public TMP_Text scoreText;

    [Header("Tempo")]
    public float tempoDeFase = 90f;
    public TMP_Text timerText;

    [Header("Fim da fase")]
    public TMP_Text endText;

    [Header("Próxima fase")]
    [SerializeField] private Object proximaCena;


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

        // AQUI LIGA O INVENTÁRIO COM O PLAYER
        if (inventoryUI != null && player != null)
        {
            inventoryUI.Init(player);
        }
        Debug.Log("Init UI chamando...");
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
        scoreText.text = TotalScore.ToString() + "/10";
    }

    void FinalizarFase(string mensagemFinal)
    {
        if (faseTerminada) return;

        faseTerminada = true;

        // Impede o jogador de se mover
        FindFirstObjectByType<Player>().TravarMovimento();

        if (endText != null)
        {
            endText.gameObject.SetActive(true);

            endText.text =
                mensagemFinal + "\n\n" +
                "Pólen entregue: " +
                TotalScore.ToString();
        }

        // Só troca de fase se venceu
        if (mensagemFinal == "VOCÊ COLETOU TODOS OS POLENS!")
        {
            StartCoroutine(CarregarIntro());
        }

        else
        {
            Time.timeScale = 0f;
        }

    }
    IEnumerator CarregarIntro()
    {
        // Define qual será a próxima fase
        IntroFase.proximaFase = proximaCena.name;

        // Espera 2 segundos mostrando a mensagem
        yield return new WaitForSecondsRealtime(2f);

        // Abre a cena de introdução
        SceneManager.LoadScene("Intro");
    }
}