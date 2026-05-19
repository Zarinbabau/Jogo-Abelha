using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public GameObject[] slots;

    public void Init(Player player)
    {
        Debug.Log("Init UI chamando...");
        player.OnInventarioChanged += Atualizar;
    }

    void Atualizar(int qtd)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            Transform vazio = slots[i].transform.Find("Vazio");

            if (vazio != null)
            {
                vazio.gameObject.SetActive(i >= qtd);
            }
        }
    }
}