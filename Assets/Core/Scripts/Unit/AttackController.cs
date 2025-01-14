using UnityEngine;

public class AttackController : MonoBehaviour
{
    public Transform targetToAttack;

    [SerializeField] private float attackRange = 5f;

    public float GetAttackRange()
    {
        return attackRange;
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Enemy") && targetToAttack == null)
        {
            targetToAttack = other.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Enemy") && targetToAttack == other.transform)
        {
            targetToAttack = null;
        }
    }
}
