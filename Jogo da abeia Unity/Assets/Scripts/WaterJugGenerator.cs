using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// GERADOR DE FASES - WATER JUG PUZZLE
/// 
/// REGRAS:
/// - Apenas o primeiro vaso começa cheio
/// - Não pode jogar água fora
/// - Apenas transferir entre vasos
/// - Vitória = atingir distribuição exata
/// 
/// O sistema:
/// - Gera 3 fases crescentes
/// - Garante solução possível
/// - Calcula menor quantidade de movimentos
/// - Cria ranking:
///     Ouro   = solução ótima
///     Prata  = média
///     Bronze = demorada
/// </summary>

public class WaterJugGenerator : MonoBehaviour
{
    [System.Serializable]
    public class PuzzleLevel
    {
        public int[] capacities;
        public int[] startState;
        public int[] targetState;

        public int minimumMoves;
        public int mediumMoves;
        public int slowMoves;

        public string difficulty;
    }

    public List<PuzzleLevel> generatedLevels = new List<PuzzleLevel>();

    public void GenerateAllLevels()
{
    GenerateLevels();

    for (int i = 0; i < generatedLevels.Count; i++)
    {
        PrintLevel(generatedLevels[i], i + 1);
    }
}

    // =========================================================
    // GERA AS 3 FASES
    // =========================================================

    void GenerateLevels()
    {
        generatedLevels.Clear();

        generatedLevels.Add(
            GeneratePuzzle(
                2,
                4,
                8,
                "Fácil"
            )
        );

        generatedLevels.Add(
            GeneratePuzzle(
                3,
                5,
                10,
                "Médio"
            )
        );

        // Fase difícil fixa
        generatedLevels.Add(
            CreateHardcodedHardPuzzle()
        );
    }

    // =========================================================
    // FASE DIFÍCIL
    // =========================================================

    PuzzleLevel CreateHardcodedHardPuzzle()
    {
        PuzzleLevel level = new PuzzleLevel();

        level.capacities = new int[] { 10, 7, 4 };

        level.startState = new int[]
        {
            10, 0, 0
        };

        level.targetState = new int[]
        {
            5, 5, 0
        };

        level.minimumMoves =
            SolveMinimumMoves(
                level.capacities,
                level.startState,
                level.targetState
            );

        level.mediumMoves = level.minimumMoves + 4;
        level.slowMoves = level.minimumMoves + 8;

        level.difficulty = "Difícil";

        return level;
    }

    // =========================================================
    // GERA PUZZLES ALEATÓRIOS
    // =========================================================

    PuzzleLevel GeneratePuzzle(
        int jugCount,
        int minCapacity,
        int maxCapacity,
        string difficulty
    )
    {
        PuzzleLevel level = new PuzzleLevel();

        bool valid = false;

        while (!valid)
        {
            level.capacities = GenerateCapacities(
                jugCount,
                minCapacity,
                maxCapacity
            );

            int totalWater = level.capacities[0];

            level.startState = new int[jugCount];
            level.startState[0] = totalWater;

            level.targetState =
                GenerateRandomTarget(
                    level.capacities,
                    totalWater
                );

            int moves =
                SolveMinimumMoves(
                    level.capacities,
                    level.startState,
                    level.targetState
                );

            if (moves > 0)
            {
                level.minimumMoves = moves;
                level.mediumMoves = moves + 3;
                level.slowMoves = moves + 6;

                level.difficulty = difficulty;

                valid = true;
            }
        }

        return level;
    }

    // =========================================================
    // GERA CAPACIDADES ALEATÓRIAS
    // =========================================================

    int[] GenerateCapacities(
        int count,
        int min,
        int max
    )
    {
        int[] caps = new int[count];

        for (int i = 0; i < count; i++)
        {
            caps[i] = Random.Range(min, max + 1);
        }

        // garante recipiente principal maior
        System.Array.Sort(caps);
        System.Array.Reverse(caps);

        return caps;
    }

    // =========================================================
    // GERA OBJETIVO ALEATÓRIO
    // =========================================================

    int[] GenerateRandomTarget(
        int[] capacities,
        int totalWater
    )
    {
        int[] target = new int[capacities.Length];

        int remaining = totalWater;

        for (int i = 0; i < capacities.Length; i++)
        {
            if (i == capacities.Length - 1)
            {
                target[i] = remaining;
            }
            else
            {
                int max =
                    Mathf.Min(
                        capacities[i],
                        remaining
                    );

                target[i] =
                    Random.Range(0, max + 1);

                remaining -= target[i];
            }
        }

        return target;
    }

    // =========================================================
    // SOLVER BFS
    // GARANTE SOLUÇÃO
    // E CALCULA MENOR CAMINHO
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

        visited.Add(GetStateKey(start));

        while (queue.Count > 0)
        {
            StateNode current = queue.Dequeue();

            if (CompareStates(
                current.state,
                target
            ))
            {
                return current.moves;
            }

            for (int from = 0; from < capacities.Length; from++)
            {
                for (int to = 0; to < capacities.Length; to++)
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

        return -1;
    }

    // =========================================================
    // TRANSFERÊNCIA
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

        int free =
            capacities[to] - next[to];

        int amount =
            Mathf.Min(
                next[from],
                free
            );

        next[from] -= amount;
        next[to] += amount;

        return next;
    }

    // =========================================================
    // UTILIDADES
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

    string GetStateKey(int[] state)
    {
        return string.Join(",", state);
    }

    void PrintLevel(
        PuzzleLevel level,
        int index
    )
    {
        Debug.Log(
            "\n======================" +
            "\nFASE " + index +
            "\nDificuldade: " + level.difficulty +
            "\nCapacidades: " +
            string.Join(",", level.capacities) +
            "\nInicial: " +
            string.Join(",", level.startState) +
            "\nObjetivo: " +
            string.Join(",", level.targetState) +
            "\nMovimentos mínimos: " +
            level.minimumMoves +
            "\nPrata até: " +
            level.mediumMoves +
            "\nBronze até: " +
            level.slowMoves +
            "\n======================"
        );
    }

    // =========================================================
    // NÓ BFS
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
            state = (int[])s.Clone();
            moves = m;
        }
    }
}