using UnityEngine;
using TMPro;

/// <summary>
/// GAME MANAGER - WATER JUG GAME
/// 
/// FUNÇÕES:
/// - Recebe fases do WaterJugGenerator
/// - Aplica valores nos vasos da cena
/// - Atualiza os textos dos prefabs
/// - Mostra objetivo no formato X,Y,Z
/// - Permite transferências
/// - Conta movimentos
/// - Detecta vitória
/// - Troca de fases
/// </summary>

public class GameManager : MonoBehaviour
{
    [Header("Referências")]
    public WaterJugGenerator generator;

    [Header("Vasos da cena")]
    public Jug[] jugs;

    [Header("UI")]
    public TMP_Text objectiveText;
    public TMP_Text moveText;
    public TMP_Text phaseText;
    public TMP_Text resultText;

    // =====================================================
    // CONTROLE
    // =====================================================

    int currentPhase = 0;

    WaterJugGenerator.PuzzleLevel currentLevel;

    Jug selectedJug;

    int moveCount = 0;

    // =====================================================
    // START
    // =====================================================

    void Start()
{
    generator.GenerateAllLevels();

    LoadPhase(0);
}

    // =====================================================
    // CARREGA FASE
    // =====================================================

    void LoadPhase(int phaseIndex)
    {
        currentPhase = phaseIndex;

        currentLevel =
            generator.generatedLevels[phaseIndex];

        moveCount = 0;

        selectedJug = null;

        resultText.text = "";

        // =========================================
        // APLICA VALORES NOS PREFABS
        // =========================================

        for (int i = 0; i < jugs.Length; i++)
        {
            // desativa vasos extras
            if (i >= currentLevel.capacities.Length)
            {
                jugs[i].gameObject.SetActive(false);
                continue;
            }

            jugs[i].gameObject.SetActive(true);

            // capacidade do vaso
            jugs[i].capacity =
                currentLevel.capacities[i];

            // líquido atual
            jugs[i].currentVolume =
                currentLevel.startState[i];

            // atualiza texto do prefab
            jugs[i].UpdateVisual();
        }

        UpdateUI();
    }

    // =====================================================
    // SELEÇÃO DOS VASOS
    // =====================================================

    public void SelectJug(Jug jug)
    {
        // primeira seleção
        if (selectedJug == null)
        {
            selectedJug = jug;
            return;
        }

        // desmarca se clicar no mesmo
        if (selectedJug == jug)
        {
            selectedJug = null;
            return;
        }

        // transfere líquido
        Transfer(selectedJug, jug);

        // limpa seleção
        selectedJug = null;
    }

    // =====================================================
    // TRANSFERÊNCIA
    // =====================================================

    void Transfer(Jug from, Jug to)
    {
        int free =
            to.capacity - to.currentVolume;

        int amount =
            Mathf.Min(
                from.currentVolume,
                free
            );

        // impede movimento inválido
        if (amount <= 0)
            return;

        from.currentVolume -= amount;
        to.currentVolume += amount;

        // atualiza textos individuais
        from.UpdateVisual();
        to.UpdateVisual();

        moveCount++;

        UpdateUI();

        CheckWin();
    }

    // =====================================================
    // UI
    // =====================================================

    void UpdateUI()
    {
        moveText.text =
            "Movimentos: " + moveCount;

        phaseText.text =
            "Fase " + (currentPhase + 1);

        objectiveText.text =
            BuildObjectiveText();
    }

    // =====================================================
    // TEXTO OBJETIVO
    // FORMATO:
    // 5,5,0
    // =====================================================

    string BuildObjectiveText()
    {
        return
            "Objetivo:\n" +
            string.Join(
                ", ",
                currentLevel.targetState
            );
    }

    // =====================================================
    // CHECA VITÓRIA
    // =====================================================

    void CheckWin()
    {
        for (int i = 0; i < currentLevel.targetState.Length; i++)
        {
            if (
                jugs[i].currentVolume !=
                currentLevel.targetState[i]
            )
            {
                return;
            }
        }

        Win();
    }

    // =====================================================
    // WIN
    // =====================================================

    void Win()
    {
        string medal = "";

        if (moveCount <= currentLevel.minimumMoves)
        {
            medal = "OURO";
        }
        else if (moveCount <= currentLevel.mediumMoves)
        {
            medal = "PRATA";
        }
        else
        {
            medal = "BRONZE";
        }

        resultText.text =
            "FASE COMPLETA!\n" +
            medal +
            "\nMovimentos: " +
            moveCount;

        Invoke(nameof(NextPhase), 2f);
    }

    // =====================================================
    // PRÓXIMA FASE
    // =====================================================

    void NextPhase()
    {
        currentPhase++;

        // terminou todas as fases
        if (
            currentPhase >=
            generator.generatedLevels.Count
        )
        {
            resultText.text =
                "VOCÊ COMPLETOU TODAS AS FASES!";
            return;
        }

        LoadPhase(currentPhase);
    }

    // =====================================================
    // RESET
    // =====================================================

    public void ResetPhase()
    {
        LoadPhase(currentPhase);
    }
}