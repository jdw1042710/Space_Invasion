using UnityEngine;

public class UnitFollowState : StateMachineBehaviour
{
    private UnitMovement unitMovement;
    private AttackController attackController;

    private static readonly int isFollowID = Animator.StringToHash("IsFollow");
    private static readonly int isAttackID = Animator.StringToHash("IsAttack");


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        unitMovement = animator.GetComponent<UnitMovement>();
        Debug.Assert(unitMovement);
        attackController = animator.transform.GetComponent<AttackController>();
        Debug.Assert(attackController);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        attackController.FollowTarget();
        // Transition
        if(unitMovement.IsCommandedToMove || !attackController.TargetToAttack)
        {
            animator.SetBool(isFollowID, false);
        }
        else if(!unitMovement.IsCommandedToMove && attackController.IsTargetInAttackRange())
        {
            animator.SetBool(isAttackID, true);
        }
    }

}
