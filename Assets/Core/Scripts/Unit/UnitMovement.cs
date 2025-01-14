using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class UnitMovement : MonoBehaviour
{
    private Unit unit;
    private Camera camera;
    private NavMeshAgent agent;
    private LayerMask ground = 1 << 3;

    public bool moveable = false;
    public bool isCommandedToMove = false;

    private void Awake()
    {
        unit = GetComponent<Unit>();
        camera = Camera.main;
        agent = GetComponentInChildren<NavMeshAgent>();
    }

    private void Update()
    {
        if(!moveable) return;
        InputManager inputManager = InputManager.Instance;
        Debug.Assert(inputManager);
        // move command
        if(inputManager.RightClickDown && !inputManager.LeftClickHoding)
        {
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, ground))
            {
                agent.SetDestination(hit.point);
                isCommandedToMove = false;
                unit.SetTargetToAttack(null);
            }
        }

        //check agent reached to dest
        isCommandedToMove = agent.hasPath && (agent.remainingDistance > agent.stoppingDistance);
    }
}
