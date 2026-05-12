using UnityEngine;
using TMPro;

public class GameManagement : MonoBehaviour
{
    [Header("Provetas (IDs 0 a 4)")]
    public GameObject[] provetas;

    [Header("Arte associada a cada proveta")]
    public GameObject[] artes;

    [Header("Valores das provetas")]
    public int[] valores = { 1, 3, 5, 7, 4 };

    private int selectedIndex = 0;

    void Start()
    {
        UpdateSelection();
    }

    void Update()
    {
        HandleInput();
    }

    void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            selectedIndex--;
            if (selectedIndex < 0)
                selectedIndex = provetas.Length - 1;

            UpdateSelection();
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            selectedIndex++;
            if (selectedIndex >= provetas.Length)
                selectedIndex = 0;

            UpdateSelection();
        }
    }

    void UpdateSelection()
    {
        for (int i = 0; i < provetas.Length; i++)
        {
            bool isSelected = (i == selectedIndex);

            // ativa arte
            if (artes != null && i < artes.Length)
                artes[i].SetActive(isSelected);

            // atualiza texto dentro do prefab
            TMP_Text txt = provetas[i].GetComponentInChildren<TMP_Text>();

            if (txt != null)
            {
                txt.text = valores[i].ToString();
            }

            // opcional: destacar proveta selecionada
            provetas[i].transform.localScale = isSelected ? Vector3.one * 1.1f : Vector3.one;
        }
    }

    public int GetSelectedValue()
    {
        return valores[selectedIndex];
    }
}