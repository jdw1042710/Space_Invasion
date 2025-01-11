using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class UnitMovement : MonoBehaviour
{
    private Camera camera;
    private NavMeshAgent agent;
    private LayerMask ground = 1 << 3;

    public bool moveable = false;

    private void Awake()
    {
        camera = Camera.main;
        agent = GetComponentInChildren<NavMeshAgent>();
    }

    private void Update()
    {
        if(!moveable) return;
        if(InputManager.Instance 
        && InputManager.Instance.RightClickDown
        && !InputManager.Instance.LeftClickHoding)
        {
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, ground))
            {
                agent.SetDestination(hit.point);
            }
        }
    }
}
