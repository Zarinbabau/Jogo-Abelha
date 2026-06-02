using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class Labirinto : MonoBehaviour
{
    public static Labirinto instance;

    [Header("Pontuação")]
    public int TotalScore;
    public int scoreParaVencer = 3;
    public TMP_Text scoreText;

    [Header("Tempo")]
    public float tempoDeFase = 150f;
    public TMP_Text timerText;

    [Header("Fim da fase")]
    public TMP_Text endText;

    [Header("Próxima fase")]
    [SerializeField] private Object proximaCena;

    private bool faseTerminada = false;

    // Libera a entrega quando todo o mel for coletado
    private bool podeEntregar = false;

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
            Derrota();
        }

        AtualizarTimer();
    }

    void AtualizarTimer()
    {
        int minutos = Mathf.FloorToInt(tempoDeFase / 60);
        int segundos = Mathf.FloorToInt(tempoDeFase % 60);

        timerText.text =
            minutos.ToString("00") + ":" +
            segundos.ToString("00");
    }

    // =====================================
    // PONTUAÇÃO
    // =====================================

    public void AddScore(int valor)
    {
        if (faseTerminada)
            return;

        TotalScore += valor;

        UpdateScoreText();

        if (TotalScore >= scoreParaVencer && !podeEntregar)
        {
            podeEntregar = true;

            Player player = FindFirstObjectByType<Player>();

            if (player != null)
                player.TravarMovimento();

            StartCoroutine(
                MostrarMensagemTemporaria(
                    "Todo o mel foi coletado!\n\nLeve-o até a Rainha.",
                    2f
                )
            );
        }
    }

    public void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text =
                TotalScore.ToString() +
                "/" +
                scoreParaVencer.ToString();
        }
    }

    // =====================================
    // ENTREGA
    // =====================================

    public void EntregarMel()
    {
        if (!podeEntregar)
            return;

        Vitoria();
    }

    // =====================================
    // VITÓRIA
    // =====================================

    public void Vitoria()
    {
        if (faseTerminada)
            return;

        faseTerminada = true;

        Player player = FindFirstObjectByType<Player>();

        if (player != null)
            player.TravarMovimento();

        if (endText != null)
        {
            endText.gameObject.SetActive(true);

            endText.text =
                "PARABÉNS!" +
                "\n\n" +
                "Você entregou todo o mel!";
        }

        StartCoroutine(CarregarIntro());
    }

    // =====================================
    // DERROTA
    // =====================================

    void Derrota()
    {
        if (faseTerminada)
            return;

        faseTerminada = true;

        Player player = FindFirstObjectByType<Player>();

        if (player != null)
            player.TravarMovimento();

        if (endText != null)
        {
            endText.gameObject.SetActive(true);

            endText.text =
                "TEMPO ESGOTADO!" +
                "\n\n" +
                "Mel coletado: " +
                TotalScore.ToString() +
                "/" +
                scoreParaVencer.ToString();
        }

        StartCoroutine(ReiniciarFase());
    }

    // =====================================
    // CARREGA INTRO
    // =====================================

    IEnumerator CarregarIntro()
    {
        IntroFase.proximaFase = proximaCena.name;

        yield return new WaitForSecondsRealtime(2f);

        SceneManager.LoadScene("Intro");
    }

    // =====================================
    // REINICIA FASE
    // =====================================

    IEnumerator ReiniciarFase()
    {
        yield return new WaitForSecondsRealtime(2f);

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // =====================================
    // MENSAGEM TEMPORÁRIA
    // =====================================

    IEnumerator MostrarMensagemTemporaria(string mensagem, float tempo)
    {
        if (endText == null)
            yield break;

        endText.gameObject.SetActive(true);
        endText.text = mensagem;

        yield return new WaitForSeconds(tempo);

        if (!faseTerminada)
        {
            endText.gameObject.SetActive(false);

            Player player = FindFirstObjectByType<Player>();

            if (player != null)
                player.LiberarMovimento();
        }
    }
}