using UnityEngine;

public class AttackController : MonoBehaviour
{
    public Transform TargetToAttack;

    private UnitMovement unitMovement;

    [SerializeField] private float attackDamage = 1f;
    [SerializeField] private float meleeAttackRange = 1f;
    [SerializeField] private float rangeAttackRange = 5f;
    [SerializeField] private Transform projectileInstantiateTransform;
    private Projectile projectile;

    private void Awake()
    {
        unitMovement = GetComponent<UnitMovement>();
        Debug.Assert(unitMovement);
    }

    private void OnTriggerStay(Collider other)
    {
        if(!other.CompareTag("Ally") && !other.CompareTag("Enemy")) return;
        if(!transform.CompareTag(other.tag) && TargetToAttack == null)
        {
            TargetToAttack = other.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(!other.CompareTag("Ally") && !other.CompareTag("Enemy")) return;
        if(!transform.CompareTag(other.tag) && TargetToAttack == other.transform)
        {
            TargetToAttack = null;
        }
    }

    public bool IsTargetInAttackRange()
    {
        if (!TargetToAttack) return false;
        float distanceFromTarget = Vector3.Distance(TargetToAttack.position, transform.position);
        return distanceFromTarget < rangeAttackRange;
    }

    public bool IsTargetInMeleeAttackRange()
    {
        if (!TargetToAttack) return false;
        float distanceFromTarget = Vector3.Distance(TargetToAttack.position, transform.position);
        return distanceFromTarget < meleeAttackRange;
    }

    public void FollowTarget()
    {
        if(!unitMovement.IsCommandedToMove && TargetToAttack)
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

    public void MeleeAttack()
    {
        if (IsTargetInMeleeAttackRange())
        {
            // Attack Process
            if(TargetToAttack.TryGetComponent(out Unit target))
            {
                target.GetDamaged(attackDamage);
            }
        }
    }
    public void RangeAttack()
    {
        if (IsTargetInAttackRange())
        {
            // Attack Process
            FireProjectile();
        }
    }

    private void FireProjectile()
    {
        // Instantiate Projectile
        if(!projectile)
        {
            GameObject projectileObj = AssetManager.Instance.GetObject("VFX/Projectile_01.prefab");
            projectile = Instantiate(projectileObj, transform).GetComponent<Projectile>();
        }
        if(TargetToAttack.TryGetComponent(out Unit target))
        {
            projectile.Fire(projectileInstantiateTransform, target);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, meleeAttackRange);
        Gizmos.DrawWireSphere(transform.position, rangeAttackRange);

    }
}
