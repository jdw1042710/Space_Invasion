using UnityEngine;

public class AttackController : MonoBehaviour
{
    public Transform TargetToAttack;

    private UnitMovement unitMovement;

    [SerializeField] private float attackRange = 5f;

    private void Awake()
    {
        unitMovement = GetComponent<UnitMovement>();
        Debug.Assert(unitMovement);
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Enemy") && TargetToAttack == null)
        {
            TargetToAttack = other.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Enemy") && TargetToAttack == other.transform)
        {
            TargetToAttack = null;
        }
    }

    public bool IsTargetInAttackRange()
    {
        if (!TargetToAttack) return false;
        float distanceFromTarget = Vector3.Distance(TargetToAttack.position, transform.position);
        return distanceFromTarget < attackRange;
    }

    public void FollowTarget()
    {
        if(!unitMovement.isCommandedToMove && TargetToAttack)
        {
            // Move toward to enemy
            Vector3 positionToAttack = TargetToAttack.GetComponent<Collider>().ClosestPoint(transform.position);
            unitMovement.MoveTo(positionToAttack);
            transform.LookAt(TargetToAttack);

            if(IsTargetInAttackRange())
            {
                // stop to moving
                unitMovement.StopToMove();
            }
        }
    }

    public void AttackTarget()
    {
        if (!unitMovement.isCommandedToMove && IsTargetInAttackRange())
        {
            // Attack Process
        }
    }
}
