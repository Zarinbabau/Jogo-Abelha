using UnityEngine;
using TMPro;

public class Jug : MonoBehaviour
{


public int jugID;

    [Header("Valores")]
    public int capacity;
    public int currentVolume;

    [Header("UI")]
    public TMP_Text volumeText;

    GameManager gm;

    void Start()
    {
        gm = FindFirstObjectByType<GameManager>();

        UpdateVisual();
    }

    void OnMouseDown()
    {
        gm.SelectJug(this);
    }

    public void UpdateVisual()
    {
        volumeText.text =
            currentVolume + "/" + capacity;
    }
}