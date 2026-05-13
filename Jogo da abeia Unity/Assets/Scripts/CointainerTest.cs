using UnityEngine;

public class CointainerTest : MonoBehaviour
{


//tenho 3 objetos na ui que sao 3 imagens, precisod e um sistema quye usa as setas do teclado esquerda direta para selecionar estes objetos comecando com o 1.
//preca ativar o filho do objeto selecioando edesativar o filho dele quando seleciona ou sai dele


    [Header("Objetos selecionáveis")]
    public GameObject[] options;

    int currentIndex = 0;

int volumeAtual;

public int atualIDobjetoSelecionado;
public GameObject objetoSelecionado;


    void Start()
    {
        UpdateSelection();
    }

    void Update()
    {


if(Input.GetKeyDown(KeyCode.Space)){


//faz a acao de subtracao e atualizao dos valores

}

        // DIREITA
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            currentIndex++;

            // loop
            if (currentIndex >= options.Length)
            {
                currentIndex = 0;
            }

            UpdateSelection();
        }

        // ESQUERDA
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            currentIndex--;

            // loop
            if (currentIndex < 0)
            {
                currentIndex = options.Length - 1;
            }

            UpdateSelection();
        }
    }

    // ==========================================
    // ATUALIZA SELEÇÃO
    // ==========================================

    void UpdateSelection()
    {

Debug.Log(currentIndex);

objetoSelecionado = options[currentIndex];
atualIDobjetoSelecionado = objetoSelecionado.GetComponent<Jug>().jugID;
//


        for (int i = 0; i < options.Length; i++)
        {
            // pega o primeiro filho
            Transform child =
                options[i].transform.GetChild(0);

            // ativa apenas o selecionado
            child.gameObject.SetActive(i == currentIndex);
        }
    }

}





    // Start is called once before the first execution of Update after the MonoBehaviour is created
    //void Start() {   }

    // Update is called once per frame
    //void Update() {

//sistema de selecionar pote
// 0 - 0 - 0 - 0 - 0(vazio) 

//if (aperta x){

//reseta}


// if aperta enter pote a, checa o pote e permite selecionar outro pote before
// if aperta enter pote a e pote b selcionados e determianda condicao:
// if pote a > B subtraia a-B

//else pote b<a 


