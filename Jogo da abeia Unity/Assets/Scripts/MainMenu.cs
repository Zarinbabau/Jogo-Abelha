using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        // Define a primeira fase do jogo
        IntroFase.proximaFase = "Fase 1";

        // Carrega a cena de introdução
        SceneManager.LoadSceneAsync("Intro");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}