using UnityEngine;
using UnityEngine.AI;

public class UnitIdleState : StateMachineBehaviour
{
    private UnitMovement unitMovement;
    private AttackController attackController;
    private static readonly int isWalkID = Animator.StringToHash("IsWalk");
    private static readonly int isFollowID = Animator.StringToHash("IsFollow");


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        unitMovement = animator.transform.GetComponent<UnitMovement>();
        Debug.Assert(unitMovement);
        attackController = animator.GetComponent<AttackController>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(unitMovement.GetVelocity().sqrMagnitude > 0)
        {
            animator.SetBool(isWalkID, true);
        }
        if(attackController.TargetToAttack)
        {
            animator.SetBool(isFollowID, true);
        }
    }
}
