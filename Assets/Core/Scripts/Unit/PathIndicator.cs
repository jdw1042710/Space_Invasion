using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class PathIndicator : MonoBehaviour
{
    [SerializeField] 
    private float originOffsetY = 0.3f; // Depends on your model

    private UnitMovement unitMovement;
    private LineRenderer lineRenderer;
    private Vector3 destination;

    public bool ShouldRender;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        Debug.Assert(lineRenderer);
        unitMovement = GetComponent<UnitMovement>();
        unitMovement.AddReachingDestEventListener(EraseLine);
    }

    public void DrawLine(Vector3 dest)
    {
        destination = dest;
        ShouldRender = true;
    }

    public void EraseLine()
    {
        destination = Vector3.zero;
        ShouldRender = false;
    }

    private void FixedUpdate()
    {
        if (ShouldRender && destination != Vector3.zero)
        {
            lineRenderer.positionCount = 2;
            Vector3 startPosition = new Vector3(
                transform.position.x, 
                transform.position.y + originOffsetY, 
                transform.position.z);
            lineRenderer.SetPosition(0, startPosition);
            lineRenderer.SetPosition(1, destination);
        }
        else
        {
            lineRenderer.positionCount = 0;
        }
    }

    private void OnDestroy()
    {
        unitMovement.RemoveReachingDestEventListener(EraseLine);
    }
}
