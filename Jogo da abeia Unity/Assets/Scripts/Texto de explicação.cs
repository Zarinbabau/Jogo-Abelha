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
        Debug.Log("===== INTRO INICIADA =====");
        Debug.Log("Próxima fase: '" + proximaFase + "'");

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
                    "Colete os pólens da floresta,\n" +
                    "e entregue eles na colmeia,\n" +
                    "antes que o tempo acabe.\n\n" +
                    "Movimentação: W, A, D";
                break;

            case "Fase 3":
                textoIntro.text =
                    "FASE 2\n" +
                    "Organize a quantidade necessária\n" +
                    "de mel para cada favo\n\n" +
                    "Navegação: A e D\n" +
                    "Selecionar: Espaço\n\n" +
                    "Cuidado com a quantidade de movimentos";
                break;

            case "Fase 4 - v2":
                textoIntro.text =
                    "FASE 3\n\n" +
                    "Encontre e colete todos os\nméis perdidos pela colmeia\n\n" +
                    "Movimentação: W, A, D";
                break;

            default:
                Debug.LogError("Nome de fase não reconhecido: '" + proximaFase + "'");
                textoIntro.text = "Erro ao carregar a próxima fase.";
                break;
        }
    }

    IEnumerator CarregarFase()
    {
        float tempoDeEspera = 5f;

        if (proximaFase == "EndGame")
        {
            tempoDeEspera = 2.5f;
        }

        Debug.Log("Esperando " + tempoDeEspera + " segundos...");

        yield return new WaitForSecondsRealtime(tempoDeEspera);

        Debug.Log("Tentando carregar a cena: '" + proximaFase + "'");

        SceneManager.LoadScene(proximaFase);

        Debug.Log("Comando LoadScene executado.");
    }
}