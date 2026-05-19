using System.Collections.Generic;
using UnityEngine;

public class GeradorDeFases : MonoBehaviour
{
    [System.Serializable]
    public class PuzzleLevel
    {
        [Header("Configuração Manual")]

        // Capacidades máximas dos potes
        public int[] capacities;

        // Estado inicial
        public int[] startState;

        // Objetivo final
        public int[] targetState;

        [Header("Calculado Automaticamente")]

        // Menor quantidade possível de movimentos
        public int minimumMoves;

        // Limite de movimentos do jogador
        public int maxMoves;

        // Dificuldade automática
        public string difficulty;
    }

    [Header("GameManager")]
    public CointainerTest gameManager;

    [Header("Fases criadas no Inspector")]
    public List<PuzzleLevel> generatedLevels =
        new List<PuzzleLevel>();

    [Header("Fase atual")]
    public int currentLevelIndex = 0;

    // =========================================================
    // START
    // =========================================================

    void Start()
    {
        CalcularDadosDasFases();

        ApplyLevel(currentLevelIndex);
    }

    // =========================================================
    // CALCULA DADOS AUTOMÁTICOS
    // =========================================================

    void CalcularDadosDasFases()
    {
        foreach (PuzzleLevel level in generatedLevels)
        {
            // =====================================
            // MOVIMENTOS MÍNIMOS
            // =====================================

            level.minimumMoves =
                SolveMinimumMoves(
                    level.capacities,
                    level.startState,
                    level.targetState
                );

            // =====================================
            // MAX MOVES
            // =====================================

            level.maxMoves =
                level.minimumMoves + 4;

            // =====================================
            // DIFICULDADE
            // =====================================

            if (level.minimumMoves <= 4)
            {
                level.difficulty = "Fácil";
            }
            else if (level.minimumMoves <= 7)
            {
                level.difficulty = "Médio";
            }
            else
            {
                level.difficulty = "Difícil";
            }
        }
    }

    // =========================================================
    // APLICA A FASE
    // =========================================================

    public void ApplyLevel(int index)
    {
        if (gameManager == null)
        {
            Debug.LogError(
                "GameManager não atribuído."
            );

            return;
        }

        PuzzleLevel level =
            generatedLevels[index];

        // =====================================
        // TARGET DA VITÓRIA
        // =====================================

        gameManager.targetState =
            level.targetState;

        // =====================================
        // MOVIMENTOS
        // =====================================

        gameManager.maxMovimentos =
            level.maxMoves;

        // =====================================
        // DIFICULDADE
        // =====================================

        gameManager.dificuldade =
            level.difficulty;

        // =====================================
        // APLICA NOS JARROS
        // =====================================

        for (int i = 0;
             i < gameManager.options.Length;
             i++)
        {
            Jug jug =
                gameManager.options[i]
                .GetComponent<Jug>();

            jug.capacity =
                level.capacities[i];

            jug.currentVolume =
                level.startState[i];

            jug.UpdateVisual();
        }

        // =====================================
        // ATUALIZA UI
        // =====================================

        gameManager.AtualizarUIFase();

        gameManager.AtualizarMovimentos();
    }

    // =========================================================
    // SOLVER BFS
    // CALCULA MENOR NÚMERO DE MOVIMENTOS
    // =========================================================

    int SolveMinimumMoves(
        int[] capacities,
        int[] start,
        int[] target
    )
    {
        Queue<StateNode> queue =
            new Queue<StateNode>();

        HashSet<string> visited =
            new HashSet<string>();

        StateNode root =
            new StateNode(start, 0);

        queue.Enqueue(root);

        visited.Add(
            GetStateKey(start)
        );

        while (queue.Count > 0)
        {
            StateNode current =
                queue.Dequeue();

            // =====================================
            // ENCONTROU SOLUÇÃO
            // =====================================

            if (CompareStates(
                current.state,
                target))
            {
                return current.moves;
            }

            // =====================================
            // TESTA TODAS TRANSFERÊNCIAS
            // =====================================

            for (int from = 0;
                 from < capacities.Length;
                 from++)
            {
                for (int to = 0;
                     to < capacities.Length;
                     to++)
                {
                    if (from == to)
                        continue;

                    int[] next =
                        Transfer(
                            current.state,
                            capacities,
                            from,
                            to
                        );

                    string key =
                        GetStateKey(next);

                    if (!visited.Contains(key))
                    {
                        visited.Add(key);

                        queue.Enqueue(
                            new StateNode(
                                next,
                                current.moves + 1
                            )
                        );
                    }
                }
            }
        }

        // =====================================
        // IMPOSSÍVEL
        // =====================================

        return -1;
    }

    // =========================================================
    // COMPARA ESTADOS
    // =========================================================

    bool CompareStates(
        int[] a,
        int[] b
    )
    {
        for (int i = 0; i < a.Length; i++)
        {
            if (a[i] != b[i])
                return false;
        }

        return true;
    }

    // =========================================================
    // CONVERTE ESTADO EM STRING
    // =========================================================

    string GetStateKey(int[] state)
    {
        return string.Join(",", state);
    }

    // =========================================================
    // TRANSFERÊNCIA ENTRE POTES
    // =========================================================

    int[] Transfer(
        int[] state,
        int[] capacities,
        int from,
        int to
    )
    {
        int[] next =
            (int[])state.Clone();

        int freeSpace =
            capacities[to] - next[to];

        int amount =
            Mathf.Min(
                next[from],
                freeSpace
            );

        // movimento inválido
        if (amount <= 0)
            return next;

        next[from] -= amount;
        next[to] += amount;

        return next;
    }

    // =========================================================
    // NODE BFS
    // =========================================================

    class StateNode
    {
        public int[] state;

        public int moves;

        public StateNode(
            int[] s,
            int m
        )
        {
            state =
                (int[])s.Clone();

            moves = m;
        }
    }
}