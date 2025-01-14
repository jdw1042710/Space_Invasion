using UnityEngine;

public class UnitAttackState : StateMachineBehaviour
{
    private AttackController attackController;

    private float attackRange;

    private static readonly int isAttackID = Animator.StringToHash("IsAttack");


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       attackController = animator.transform.GetComponent<AttackController>();
       Debug.Assert(attackController);
       attackRange = attackController.GetAttackRange();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Should transition to Idle State?
        if(attackController.targetToAttack == null)
        {
            animator.SetBool(isAttackID, false);
        }
        else
        {
            // Should transition to Follow State?
            float distanceFromTarget = Vector3.Distance(attackController.targetToAttack.position, animator.transform.position);
            if(distanceFromTarget > attackRange)
            {
                animator.SetBool(isAttackID, false);
            }
        }
    }

}
