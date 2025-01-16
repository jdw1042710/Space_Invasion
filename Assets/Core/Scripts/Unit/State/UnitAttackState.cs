using UnityEngine;

public class UnitAttackState : StateMachineBehaviour
{
    private AttackController attackController;
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
        attackController.AttackTarget();
        if (!attackController.IsTargetInAttackRange())
        {
            animator.SetBool(isAttackID, false);
        }
    }

}
