using UnityEngine;

public class UIFollowWorld : MonoBehaviour
{
    public Transform target;

    private Vector3 offset;

    void Start()
    {
        if (target == null) return;

        // calcula diferença inicial entre UI e player
        offset = transform.position - target.position;
    }

    void LateUpdate()
    {
        if (target == null) return;

        transform.position = target.position + offset;

        transform.rotation = Quaternion.identity;
    }
}