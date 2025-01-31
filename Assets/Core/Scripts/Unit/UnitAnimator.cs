using UnityEngine;

public class UnitAnimator : MonoBehaviour
{
    private UnitMovement unitMovement;
    private Animator animator;

    private static readonly int isWalkID = Animator.StringToHash("IsWalk");

    public bool isWalk;

    private void Awake()
    {
        unitMovement = GetComponent<UnitMovement>();
        Debug.Assert(unitMovement);
        animator = GetComponent<Animator>();
        Debug.Assert(animator);
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        isWalk = unitMovement.GetVelocity().sqrMagnitude > 0;
        animator.SetBool(isWalkID, unitMovement.GetVelocity().sqrMagnitude > 0);
    }
}
