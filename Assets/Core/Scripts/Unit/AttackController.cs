using Unity.VisualScripting;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    public Transform TargetToAttack;

    private UnitMovement unitMovement;

    [SerializeField] private float attackDamage = 1f;
    [SerializeField] private float meleeAttackRange = 1f;
    [SerializeField] private float rangeAttackRange = 5f;
    [SerializeField] private Transform projectileInstantiateTransform;
    private readonly static string fireSoundAssetAddress = "SFX/Projectile_Fire.wav";
    private AudioClip fireSound;
    private readonly static string meleeAttackSoundAssetAddress = "SFX/Melee_Hit.wav";
    private AudioClip meleeAttackSound;

    private readonly static string projectileAssetAddress = "VFX/Projectile_01.prefab";
    private GameObject projectilePrefab;
    private Projectile projectile;

    private void Awake()
    {
        unitMovement = GetComponent<UnitMovement>();
        Debug.Assert(unitMovement);
    }

    private void Start()
    {
        fireSound = AssetManager.Instance.GetAudio(fireSoundAssetAddress);
        Debug.Assert(fireSound);
        meleeAttackSound = AssetManager.Instance.GetAudio(meleeAttackSoundAssetAddress);
        Debug.Assert(meleeAttackSound);
        projectilePrefab = AssetManager.Instance.GetPrefab(projectileAssetAddress);
        Debug.Assert(projectilePrefab);
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
                SoundManager.Instance.Play(meleeAttackSound, SoundManager.sfxVolume);
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
            projectile = Instantiate(projectilePrefab, transform).GetComponent<Projectile>();
        }
        if(TargetToAttack.TryGetComponent(out Unit target))
        {
            projectile.Fire(projectileInstantiateTransform, target);
            SoundManager.Instance.Play(fireSound, SoundManager.sfxVolume);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, meleeAttackRange);
        Gizmos.DrawWireSphere(transform.position, rangeAttackRange);

    }
}
