using UnityEngine;

public class Sapo : MonoBehaviour
{
    [SerializeField] Transform firePoint;
    [SerializeField] LineController lineController;


    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 direction = mousePos - firePoint.position;
            lineController.ThrowLineRenderer(direction);
        }
    }
}
