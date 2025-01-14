using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class UnitFollowState : StateMachineBehaviour
{
    private AttackController attackController;
    private NavMeshAgent agent;
    private UnitMovement unitMovement;
    private float attackRange;

    private static readonly int isFollowID = Animator.StringToHash("IsFollow");
    private static readonly int isAttackID = Animator.StringToHash("IsAttack");


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       attackController = animator.transform.GetComponent<AttackController>();
       agent = animator.transform.GetComponent<NavMeshAgent>();
       unitMovement = animator.transform.GetComponent<UnitMovement>();
       Debug.Assert(attackController);
       Debug.Assert(agent);
       Debug.Assert(unitMovement);

       attackRange = attackController.GetAttackRange();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Should transition to Idle State?
        if(attackController.targetToAttack == null)
        {
            animator.SetBool(isFollowID, false);
        }
        else if(!unitMovement.isCommandedToMove)
        {
            // Move toward to enemy
            agent.SetDestination(attackController.targetToAttack.position);
            animator.transform.LookAt(attackController.targetToAttack);

            // Should transition to Attack State?
            float distanceFromTarget = Vector3.Distance(attackController.targetToAttack.position, animator.transform.position);
            if(distanceFromTarget < attackRange)
            {
                // stop to moving
                agent.SetDestination(animator.transform.position);
                animator.SetBool(isAttackID, true);
            }
        }
    }

}
