using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuTeclado : MonoBehaviour
{
    [Header("Botões")]
    public Button botaoComecar;
    public Button botaoSair;

    private int indiceSelecionado = 0;
    private Button[] botoes;

    void Start()
    {
        botoes = new Button[]
        {
            botaoComecar,
            botaoSair
        };

        AtualizarSelecao();
    }

    void Update()
    {
        // Navegar para cima
        if (Input.GetKeyDown(KeyCode.W))
        {
            indiceSelecionado--;

            if (indiceSelecionado < 0)
                indiceSelecionado = botoes.Length - 1;

            AtualizarSelecao();
        }

        // Navegar para baixo
        if (Input.GetKeyDown(KeyCode.S))
        {
            indiceSelecionado++;

            if (indiceSelecionado >= botoes.Length)
                indiceSelecionado = 0;

            AtualizarSelecao();
        }

        // Confirmar
        if (Input.GetKeyDown(KeyCode.Space))
        {
            botoes[indiceSelecionado].onClick.Invoke();
        }
    }

    void AtualizarSelecao()
    {
        EventSystem.current.SetSelectedGameObject(
            botoes[indiceSelecionado].gameObject
        );
    }
}