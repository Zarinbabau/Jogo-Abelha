using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BotaoSelecionado : MonoBehaviour
{
    private Outline outline;
    private Button button;

    void Awake()
    {
        outline = GetComponent<Outline>();
        button = GetComponent<Button>();

        if (outline != null)
            outline.enabled = false;
    }

    void Update()
    {
        bool selecionado =
            EventSystem.current.currentSelectedGameObject ==
            gameObject;

        if (outline != null)
            outline.enabled = selecionado;
    }
}