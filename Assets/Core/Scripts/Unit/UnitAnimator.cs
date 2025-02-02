using UnityEngine;

public class UnitAnimator : MonoBehaviour
{
    private UnitMovement unitMovement;
    private AttackController attackController;
    private Animator animator;

    private static readonly int isWalkID = Animator.StringToHash("IsWalk");
    private static readonly int isMeleeID = Animator.StringToHash("IsMelee");


    private void Awake()
    {
        unitMovement = GetComponent<UnitMovement>();
        Debug.Assert(unitMovement);
        attackController = GetComponent<AttackController>();
        Debug.Assert(attackController);
        animator = GetComponent<Animator>();
        Debug.Assert(animator);
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        animator.SetBool(isWalkID, unitMovement.GetVelocity().sqrMagnitude > 0);
        animator.SetBool(isMeleeID, attackController.IsTargetInMeleeAttackRange());
    }
}
