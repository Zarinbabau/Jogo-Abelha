using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Jug : MonoBehaviour
{
    public int jugID;

    public int capacity;
    public int currentVolume;

    public TMP_Text volumeText;

    [Header("Visual")]
    public Image recipienteImage;

    public Sprite spriteNormal;
    public Sprite spriteCheio;

    public Vector3 posicaoOriginal;

    void Start()
    {
        posicaoOriginal = transform.position;
    }
    void Update()
    {
        UpdateVisual();
    }

    public void UpdateVisual()
    {
        volumeText.text = currentVolume + "/" + capacity;

        if (currentVolume >= capacity)
        {
            recipienteImage.sprite = spriteCheio;
        }
        else
        {
            recipienteImage.sprite = spriteNormal;
        }
    }

}