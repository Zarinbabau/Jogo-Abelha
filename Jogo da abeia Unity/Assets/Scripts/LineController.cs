using UnityEngine;

public class LineController : MonoBehaviour
{
    private LineRenderer lineRenderer;
    [SerializeField] private Transform origin;

    [SerializeField] private float speed = 10;
    [SerializeField] private float maxDistance = 2;

    [SerializeField] LineCollider lineCollider;

    private Vector3 direction;

    private LineRendererState state;
    public LineRendererState State
    {
        get { return state; }
        set
        {
            state = value;
            if (state == LineRendererState.Disabled)
            {
                lineCollider.gameObject.SetActive(false);
                lineRenderer.enabled = false;
            }
            else
            {
                lineCollider.gameObject.SetActive(true);
                lineRenderer.gameObject.SetActive(true);
            }
        }
    }
        
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
    }

    public void ThrowLineRenderer(Vector2 aimDirection) 
    {
        if (state != LineRendererState.Disabled) return;

        lineCollider.gameObject.SetActive(true);
        lineRenderer.enabled = true;

        State = LineRendererState.Going;
        
        direction = aimDirection;
        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, origin.position);
        lineRenderer.SetPosition(1, origin.position);

    }

    // Update is called once per frame
    void Update()
    {
        if (!lineRenderer.enabled) return;
        lineRenderer.SetPosition(0, origin.position);
        lineCollider.transform.position = lineRenderer.GetPosition(1);
        var distance = Vector3.Distance(origin.position, lineRenderer.GetPosition(1));

        switch (State)
        {
            case LineRendererState.Going:
                if ((maxDistance - distance) <= 0.001f)
                {
                    State = LineRendererState.GoingBack;
                }
                else
                {
                    Vector3 finalPoint = direction.normalized * maxDistance + origin.position;
                    lineCollider.LookAtPosition(finalPoint);
                    MoveTowardsTarget(finalPoint);
                }
                break;

            case LineRendererState.GoingBack:
                if (distance <= 0.0001f)
                {
                    State = LineRendererState.Disabled;
                }
                else
                {
                    MoveTowardsTarget(origin.position);
                }
                break;
        }
    }

    private void MoveTowardsTarget(Vector2 target)
    {
        var step = Time.deltaTime * speed;
        Vector2 pos = Vector2.MoveTowards(lineRenderer.GetPosition(1), target, step);
        lineRenderer.SetPosition(1, pos);
    }

}

public enum LineRendererState
{
    Disabled, 
    Going,
    GoingBack
}