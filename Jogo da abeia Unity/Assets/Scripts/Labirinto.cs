using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class Labirinto : MonoBehaviour
{
    public static Labirinto instance;

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
    // VITÓRIA
    // =====================================

    public void Vitoria()
    {
        if (faseTerminada) return;

        faseTerminada = true;

        // Trava o player
        FindFirstObjectByType<Player>().TravarMovimento();

        if (endText != null)
        {
            endText.gameObject.SetActive(true);

            endText.text =
                "Parabéns!!" +
                "\n\n" +
                "Você conseguiu entregar o mel!";
        }

        StartCoroutine(CarregarIntro());
    }

    // =====================================
    // DERROTA
    // =====================================

    void Derrota()
    {
        if (faseTerminada) return;

        faseTerminada = true;

        // Trava o player
        FindFirstObjectByType<Player>().TravarMovimento();

        if (endText != null)
        {
            endText.gameObject.SetActive(true);

            endText.text =
                "TEMPO ESGOTADO!" +
                "\n\n" +
                "Tente novamente.";
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
}