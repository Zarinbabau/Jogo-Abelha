using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;

public class CointainerTest : MonoBehaviour
{
    public bool MoverPotes = true;

    [Header("Gerador")]
    public GeradorDeFases gerador;

    [Header("UI")]
    public TMP_Text dificuldadeText;
    public TMP_Text objetivoText;
    public TMP_Text movesText;

    public GameObject endPanel;
    public TMP_Text endText;

    [Header("Movimentos")]
    public int maxMovimentos = 0;
    private int movimentosAtuais = 0;

    [Header("Jogo")]
    public GameObject[] options;

    [Header("Próxima cena")]
    [SerializeField] private Object proximaCena;

    public int[] targetState;

    int currentIndex = 0;
    Jug selectedJug;
    bool hasWon = false;

    void Start()
    {
        UpdateSelection();
        AtualizarMovimentos();
        endPanel.SetActive(false);
    }

    void Update()
    {
        if (!MoverPotes) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jug j = options[currentIndex].GetComponent<Jug>();

            if (selectedJug == null)
                selectedJug = j;
            else
            {
                Transfer(selectedJug, j);
                selectedJug = null;
            }
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            currentIndex = (currentIndex + 1) % options.Length;
            UpdateSelection();
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            currentIndex--;
            if (currentIndex < 0)
                currentIndex = options.Length - 1;

            UpdateSelection();
        }
    }

    void Transfer(Jug from, Jug to)
    {
        int free = to.capacity - to.currentVolume;
        int amount = Mathf.Min(from.currentVolume, free);

        if (amount <= 0) return;

        from.currentVolume -= amount;
        to.currentVolume += amount;

        from.UpdateVisual();
        to.UpdateVisual();

        movimentosAtuais++;
        AtualizarMovimentos();

        CheckVictory();

        if (!hasWon && movimentosAtuais >= maxMovimentos)
            Derrota();
    }

    public void AtualizarMovimentos()
    {
        movesText.text =
            "Movimentos: " + movimentosAtuais + "/" + maxMovimentos;
    }

    public void AtualizarUIFase()
    {
        dificuldadeText.text =
            "Dificuldade: " +
            (gerador.currentLevelIndex == 0 ? "Fácil" :
             gerador.currentLevelIndex == 1 ? "Médio" : "Difícil");

        objetivoText.text =
            "Objetivo: Deixe os potes na seguinte ordem\n" +
            string.Join(" | ", targetState);
    }

    void CheckVictory()
    {
        if (hasWon) return;

        for (int i = 0; i < options.Length; i++)
        {
            if (options[i].GetComponent<Jug>().currentVolume != targetState[i])
                return;
        }

        Vitoria();
    }

    void Vitoria()
    {
        if (hasWon) return;

        hasWon = true;
        MoverPotes = false;

        if (gerador.currentLevelIndex < gerador.levels.Count - 1)
        {
            StartCoroutine(ProximaFase());
            return;
        }

        endPanel.SetActive(true);
        endText.text = "VITÓRIA!\n\nVocê completou todas as fases!";

        StartCoroutine(CarregarIntro());
    }

    void Derrota()
    {
        if (hasWon) return;

        hasWon = true;
        MoverPotes = false;

        endPanel.SetActive(true);
        endText.text = "DERROTA!\n\nTentativa falhou.";

        StartCoroutine(ReiniciarFase());
    }

    IEnumerator ReiniciarFase()
    {
        yield return new WaitForSecondsRealtime(2f);

        gerador.ApplyLevel(gerador.currentLevelIndex);

        movimentosAtuais = 0;
        AtualizarMovimentos();

        ResetSelecao();

        hasWon = false;
        MoverPotes = true;

        endPanel.SetActive(false);
    }

    IEnumerator ProximaFase()
    {
        MoverPotes = false;

        yield return new WaitForSecondsRealtime(1.5f);

        gerador.currentLevelIndex++;
        gerador.ApplyLevel(gerador.currentLevelIndex);

        movimentosAtuais = 0;
        AtualizarMovimentos();

        ResetSelecao();

        hasWon = false;
        MoverPotes = true;

        endPanel.SetActive(false);
    }

    IEnumerator CarregarIntro()
    {
        IntroFase.proximaFase = proximaCena.name;

        yield return new WaitForSecondsRealtime(2f);

        SceneManager.LoadScene("Intro");
    }

    void ResetSelecao()
    {
        currentIndex = 0;
        selectedJug = null;
        UpdateSelection();
    }

    void UpdateSelection()
    {
        for (int i = 0; i < options.Length; i++)
        {
            options[i].transform.GetChild(0)
                .gameObject.SetActive(i == currentIndex);
        }
    }
}