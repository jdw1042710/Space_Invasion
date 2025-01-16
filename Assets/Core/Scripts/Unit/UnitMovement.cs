using System.Runtime.CompilerServices;
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
        //check agent reached to dest
        bool hasPath = agent.hasPath;
        bool remainPath = agent.remainingDistance > agent.stoppingDistance;
        isCommandedToMove = hasPath && remainPath;
    }

    public void MoveTo(Vector3 position)
    {
        agent.SetDestination(position);
    }

    public void StopToMove()
    {
        agent.ResetPath();
    }
}
