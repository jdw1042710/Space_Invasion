using UnityEngine;
using UnityEngine.AI;

public class UnitMovement : MonoBehaviour
{
    private Unit unit;
    private Camera camera;
    private NavMeshAgent agent;
    private AttackController attackController;
    private LayerMask ground = 1 << 3;

    public bool Moveable = false;
    public bool IsCommandedToMove {get; private set;} = false;
    private Vector3 commandedPosition;


    private void Awake()
    {
        unit = GetComponent<Unit>();
        camera = Camera.main;
        agent = GetComponent<NavMeshAgent>();
        attackController = GetComponent<AttackController>();
    }

    private void Update()
    {
        if(!Moveable) return;
        InputManager inputManager = InputManager.Instance;
        Debug.Assert(inputManager);
        //check agent reached to dest
    }

    private void FixedUpdate()
    {
        if(IsCommandedToMove)
        {
            bool hasPath = agent.hasPath;
            float remainingDistance = (transform.position - commandedPosition).magnitude;
            bool remainPath = remainingDistance > agent.stoppingDistance;
            IsCommandedToMove = hasPath && remainPath;
        }
    }

    public void MoveTo(Vector3 position, bool isForce = false)
    {
        agent.SetDestination(position);
        if(isForce)
        {
            Debug.Log("Foce Move");
            IsCommandedToMove = true;
            commandedPosition = position;
            if(attackController)
                attackController.TargetToAttack = null;
        }
    }

    public void StopToMove()
    {
        agent.ResetPath();
    }

    public Vector3 GetVelocity()
    {
        return agent.velocity;
    }
}
