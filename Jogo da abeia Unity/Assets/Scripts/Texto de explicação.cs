using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class IntroFase : MonoBehaviour
{
    [Header("UI")]
    public TMP_Text textoIntro;

    // Guarda qual será a próxima fase
    public static string proximaFase;

    void Start()
    {
        MostrarTexto();

        StartCoroutine(CarregarFase());
    }

    void MostrarTexto()
    {
        switch (proximaFase)
        {
            case "Fase 1":
                textoIntro.text =
                    "FASE 1\n\n" +
                    "Colete os pólens da floresta\n" +
                    "antes que o tempo acabe.";
                break;

            case "Fase 3":
                textoIntro.text =
                    "FASE 3\n\n" +
                    "Organize a quantidade de mel nos potes\n" +
                    "use o menor número de movimentos possíveis.";
                break;

            case "Fase 4":
                textoIntro.text =
                    "FASE 4\n\n" +
                    "Atravesse o labirinto para entregar o mel\n" +
                    "à abelha rainha antes que o tempo acabe.";
                break;

            case "EndGame":
                textoIntro.text =
                    "Parabéns, você completou toda a jornada do mel!!";
                break;
        }
    }

    IEnumerator CarregarFase()
    {
        float tempoDeEspera = 5f;

        // Tempo menor para tela final
        if (proximaFase == "EndGame")
        {
            tempoDeEspera = 2.5f;
        }

        yield return new WaitForSeconds(tempoDeEspera);

        SceneManager.LoadScene(proximaFase);
    }
}