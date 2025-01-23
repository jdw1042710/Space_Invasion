using System.Data.Common;
using UnityEngine;

public class UnitAttackState : StateMachineBehaviour
{
    private AttackController attackController;
    private UnitMovement unitMovement;
    private static readonly int isAttackID = Animator.StringToHash("IsAttack");


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        attackController = animator.transform.GetComponent<AttackController>();
        Debug.Assert(attackController);
        unitMovement = animator.transform.GetComponent<UnitMovement>();
        Debug.Assert(unitMovement);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        attackController.AttackTarget();
        if(unitMovement.IsCommandedToMove
        || (!attackController.IsTargetInAttackRange() && stateInfo.normalizedTime >= 1.0f))
        {
            animator.SetBool(isAttackID, false);
        }
    }
}
