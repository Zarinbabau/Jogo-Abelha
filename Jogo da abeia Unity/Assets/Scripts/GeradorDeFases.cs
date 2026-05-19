using System.Collections.Generic;
using UnityEngine;

public class GeradorDeFases : MonoBehaviour
{
    [System.Serializable]
    public class PuzzleLevel
    {
        public int[] capacities;
        public int[] startState;
        public int[] targetState;

        public int minimumMoves;
        public int maxMoves;
    }

    public CointainerTest gameManager;

    public List<PuzzleLevel> levels = new();

    public int currentLevelIndex = 0;

    void Start()
    {
        Calcular();
        ApplyLevel(currentLevelIndex);
    }

    // =========================================================
    // CALCULA MOVIMENTOS REAIS (BFS)
    // =========================================================

    void Calcular()
    {
        foreach (var level in levels)
        {
            level.minimumMoves =
                Solve(level.capacities, level.startState, level.targetState);

            level.maxMoves = level.minimumMoves + 3;
        }
    }

    // =========================================================
    // APLICA FASE
    // =========================================================

    public void ApplyLevel(int index)
    {
        currentLevelIndex = index;

        PuzzleLevel level = levels[index];

        gameManager.targetState = level.targetState;
        gameManager.maxMovimentos = level.maxMoves;

        for (int i = 0; i < gameManager.options.Length; i++)
        {
            Jug j = gameManager.options[i].GetComponent<Jug>();

            j.capacity = level.capacities[i];
            j.currentVolume = level.startState[i];
            j.UpdateVisual();
        }

        gameManager.AtualizarUIFase();
        gameManager.AtualizarMovimentos();
    }

    // =========================================================
    // BFS SOLVER REAL
    // =========================================================

    int Solve(int[] cap, int[] start, int[] target)
    {
        Queue<StateNode> queue = new Queue<StateNode>();
        HashSet<string> visited = new HashSet<string>();

        queue.Enqueue(new StateNode(start, 0));
        visited.Add(GetKey(start));

        while (queue.Count > 0)
        {
            StateNode current = queue.Dequeue();

            if (IsGoal(current.state, target))
                return current.moves;

            for (int from = 0; from < cap.Length; from++)
            {
                for (int to = 0; to < cap.Length; to++)
                {
                    if (from == to) continue;

                    int[] next = Transfer(current.state, cap, from, to);

                    string key = GetKey(next);

                    if (!visited.Contains(key))
                    {
                        visited.Add(key);
                        queue.Enqueue(new StateNode(next, current.moves + 1));
                    }
                }
            }
        }

        return -1;
    }

    // =========================================================
    // UTILITÁRIOS
    // =========================================================

    bool IsGoal(int[] a, int[] b)
    {
        for (int i = 0; i < a.Length; i++)
            if (a[i] != b[i]) return false;

        return true;
    }

    string GetKey(int[] state)
    {
        return string.Join(",", state);
    }

    int[] Transfer(int[] state, int[] cap, int from, int to)
    {
        int[] next = (int[])state.Clone();

        int free = cap[to] - next[to];
        int amount = Mathf.Min(next[from], free);

        if (amount <= 0)
            return next;

        next[from] -= amount;
        next[to] += amount;

        return next;
    }

    class StateNode
    {
        public int[] state;
        public int moves;

        public StateNode(int[] s, int m)
        {
            state = (int[])s.Clone();
            moves = m;
        }
    }
}