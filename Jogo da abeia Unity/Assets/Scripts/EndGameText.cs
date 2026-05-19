using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class EndGameText : MonoBehaviour
{
    [SerializeField] private float tempoParaMenu = 5f;

    void Start()
    {
        Debug.Log("EndGame iniciou");

        StartCoroutine(VoltarAoMenu());
    }

    IEnumerator VoltarAoMenu()
    {
        Debug.Log("Timer começou");

        // Tempo real (ignora Time.timeScale)
        yield return new WaitForSecondsRealtime(tempoParaMenu);

        Debug.Log("Voltando ao menu");

        // Garante que o tempo volte ao normal
        Time.timeScale = 1f;

        SceneManager.LoadScene("Menu");
    }
}