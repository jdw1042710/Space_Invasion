using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitMovement : MonoBehaviour
{
    private Unit unit;
    private Camera camera;
    private NavMeshAgent agent;
    private AttackController attackController;
    private PathIndicator pathIndicator;
    private LayerMask ground = 1 << 3;

    public bool Moveable
    {
        get
        {
            return moveable;
        }
        set
        {
            moveable = value;
            pathIndicator.ShouldRender = value;
        }
    }
    private bool moveable = false;
    public bool IsCommandedToMove {get; private set;} = false;
    private Vector3 commandedPosition;

    private List<Action> destReachingEventListeners = new List<Action>();

    private void Awake()
    {
        unit = GetComponent<Unit>();
        Debug.Assert(unit);
        camera = Camera.main;
        agent = GetComponent<NavMeshAgent>();
        Debug.Assert(agent);
        attackController = GetComponent<AttackController>();
        Debug.Assert(attackController);
        pathIndicator = GetComponent<PathIndicator>();
        Debug.Assert(pathIndicator);
    }

    private void Start()
    {
        InputManager inputManager = InputManager.Instance;
        Debug.Assert(inputManager);
    }

    private void FixedUpdate()
    {
        if(IsCommandedToMove)
        {
            bool hasPath = agent.hasPath;
            bool remainPath = IsRemainPath();
            IsCommandedToMove = hasPath && remainPath;
        }
        CheckDestinationReached();
    }

    public void MoveTo(Vector3 position, bool isForce = false)
    {
        agent.SetDestination(position);
        pathIndicator.DrawLine(position);
        if(isForce)
        {
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

    private bool IsRemainPath()
    {
        float remainingDistance = (transform.position - commandedPosition).magnitude;
        return remainingDistance > agent.stoppingDistance;
    }

    private void CheckDestinationReached()
    {
        if(!agent.hasPath) return;
        if(!IsRemainPath())
        {
            foreach(var listener in destReachingEventListeners)
                listener();
            StopToMove();
        }
    }

    public void AddReachingDestEventListener(Action listener)
    {
        destReachingEventListeners.Add(listener);
    }

    public void RemoveReachingDestEventListener(Action listener)
    {
        destReachingEventListeners.Remove(listener);
    }

}
