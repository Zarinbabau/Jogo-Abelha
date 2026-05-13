using UnityEngine;
using TMPro;

public class Jug : MonoBehaviour
{
void Update(){
UpdateVisual();

}

    public int jugID;

    public int capacity;
    public int currentVolume;

    public TMP_Text volumeText;

    public void UpdateVisual()
    {
        volumeText.text =
            currentVolume + "/" + capacity;
    }
}