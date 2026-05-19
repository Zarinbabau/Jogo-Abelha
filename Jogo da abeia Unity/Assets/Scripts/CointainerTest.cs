using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using TMPro;

public class CointainerTest : MonoBehaviour
{
    public bool MoverPotes = true;

    [Header("UI")]
    public GameObject endPanel;
    public TMP_Text endText;

    [Header("Movimentos")]
    public int maxMovimentos = 10;

    private int movimentosAtuais = 0;

    public TMP_Text movesText;

    [Header("Próxima fase")]
    [SerializeField] private Object proximaCena;

    [Header("Objetos selecionáveis")]
    public GameObject[] options;

    [Header("Resposta correta")]
    public int[] targetState;

    int currentIndex = 0;

    Jug selectedJug = null;

    bool hasWon = false;

    void Start()
    {
        UpdateSelection();

        AtualizarMovimentos();

        if (endPanel != null)
            endPanel.SetActive(false);
    }

    void Update()
    {
        if (!MoverPotes) return;

        // =====================================
        // CONFIRMA SELEÇÃO / TRANSFERÊNCIA
        // =====================================

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jug currentJug =
                options[currentIndex]
                .GetComponent<Jug>();

            if (selectedJug == null)
            {
                selectedJug = currentJug;

                Debug.Log("Origem: " + selectedJug.jugID);
            }
            else
            {
                if (selectedJug == currentJug)
                {
                    selectedJug = null;
                    return;
                }

                TransferLiquid(selectedJug, currentJug);

                selectedJug = null;
            }
        }

        // =====================================
        // DIREITA
        // =====================================

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            currentIndex++;

            if (currentIndex >= options.Length)
                currentIndex = 0;

            UpdateSelection();
        }

        // =====================================
        // ESQUERDA
        // =====================================

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            currentIndex--;

            if (currentIndex < 0)
                currentIndex = options.Length - 1;

            UpdateSelection();
        }
    }

    // =====================================
    // TRANSFERÊNCIA
    // =====================================

    void TransferLiquid(Jug from, Jug to)
    {
        int freeSpace = to.capacity - to.currentVolume;

        int amount = Mathf.Min(from.currentVolume, freeSpace);

        // Movimento inválido NÃO conta
        if (amount <= 0)
        {
            Debug.Log("Movimento inválido");
            return;
        }

        from.currentVolume -= amount;
        to.currentVolume += amount;

        from.UpdateVisual();
        to.UpdateVisual();

        Debug.Log("Transferiu " + amount + "L");

        // =====================================
        // CONTA MOVIMENTO
        // =====================================

        movimentosAtuais++;

        AtualizarMovimentos();

        // =====================================
        // VERIFICA VITÓRIA PRIMEIRO
        // =====================================

        CheckVictory();

        // Se venceu, não verifica derrota
        if (hasWon)
            return;

        // =====================================
        // DERROTA
        // =====================================

        if (movimentosAtuais >= maxMovimentos)
        {
            Derrota();
        }
    }

    // =====================================
    // UI MOVIMENTOS
    // =====================================

    void AtualizarMovimentos()
    {
        if (movesText != null)
        {
            movesText.text =
                movimentosAtuais + "/" +
                maxMovimentos;
        }
    }

    // =====================================
    // VITÓRIA
    // =====================================

    void CheckVictory()
    {
        if (hasWon) return;

        for (int i = 0; i < options.Length; i++)
        {
            Jug j = options[i].GetComponent<Jug>();

            if (j.currentVolume != targetState[i])
                return;
        }

        Vitoria();
    }

    void Vitoria()
    {
        if (hasWon) return;

        hasWon = true;
        MoverPotes = false;

        endPanel.SetActive(true);

        endText.text =
            "VITÓRIA!\n\n" +
            "Você organizou o mel corretamente.";

        StartCoroutine(VictoryRoutine());
    }

    // =====================================
    // DERROTA
    // =====================================

    void Derrota()
    {
        if (hasWon) return;

        hasWon = true;
        MoverPotes = false;

        endPanel.SetActive(true);

        endText.text =
            "DERROTA!\n\n" +
            "Você excedeu o número de movimentos.";

        StartCoroutine(ReiniciarFase());
    }

    // =====================================
    // CORROTINAS
    // =====================================

    IEnumerator VictoryRoutine()
    {
        Debug.Log("VITÓRIA!");

        // Define qual será a próxima fase
        IntroFase.proximaFase = proximaCena.name;

        // Espera antes da intro
        yield return new WaitForSecondsRealtime(1.5f);

        // Abre a cena de introdução
        SceneManager.LoadScene("Intro");
    }

    IEnumerator ReiniciarFase()
    {
        yield return new WaitForSecondsRealtime(2f);

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().name
        );
    }

    // =====================================
    // VISUAL SELEÇÃO
    // =====================================

    void UpdateSelection()
    {
        for (int i = 0; i < options.Length; i++)
        {
            Transform child =
                options[i].transform.GetChild(0);

            child.gameObject.SetActive(
                i == currentIndex
            );
        }
    }
}