using UnityEngine;

public class LineCollider : MonoBehaviour
{
    public void LookAtPosition(Vector3 position)
    {
        Vector3 lookDir = position - transform.position;
        var angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0,0,angle);
    }
}
