using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class UnitMovement : MonoBehaviour
{
    private Camera camera;
    private NavMeshAgent agent;
    [SerializeField] private LayerMask ground;

    private void Awake()
    {
        camera = Camera.main;
        agent = GetComponentInChildren<NavMeshAgent>();
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(1))
        {
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, ground))
            {
                agent.SetDestination(hit.point);
            }
        }
    }
}
