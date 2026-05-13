using UnityEngine;
using System.Collections;

public class CointainerTest : MonoBehaviour
{
    public bool MoverPotes = true;

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

                CheckVictory();
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

        StartCoroutine(VictoryRoutine());
    }

    // =====================================
    // CORROTINA DE VITÓRIA
    // =====================================

    IEnumerator VictoryRoutine()
    {
        hasWon = true;
        MoverPotes = false;

        Debug.Log("VITÓRIA!");

        yield return new WaitForSeconds(1.5f);

        // aqui você pode:
        // - carregar próxima fase
        // - ou resetar

        MoverPotes = true;
        hasWon = false;

        Debug.Log("Jogo liberado novamente");
    }

    // =====================================
    // VISUAL SELEÇÃO
    // =====================================

    void UpdateSelection()
    {
        for (int i = 0; i < options.Length; i++)
        {
            Transform child = options[i].transform.GetChild(0);

            child.gameObject.SetActive(i == currentIndex);
        }
    }
}