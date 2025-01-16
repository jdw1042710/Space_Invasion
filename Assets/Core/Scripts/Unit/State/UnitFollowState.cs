using UnityEngine;

public class UnitFollowState : StateMachineBehaviour
{
    private AttackController attackController;

    private static readonly int isFollowID = Animator.StringToHash("IsFollow");
    private static readonly int isAttackID = Animator.StringToHash("IsAttack");


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       attackController = animator.transform.GetComponent<AttackController>();
       Debug.Assert(attackController);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        attackController.FollowTarget();
        // Transition
        if(!attackController.TargetToAttack)
        {
            animator.SetBool(isFollowID, false);
        }
        else if(attackController.IsTargetInAttackRange())
        {
            animator.SetBool(isAttackID, true);
        }
    }

}
