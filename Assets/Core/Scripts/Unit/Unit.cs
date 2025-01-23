using Unity.VisualScripting;
using UnityEngine;

public class Unit : MonoBehaviour
{

    private UnitMovement unitMovement;
    private AttackController attackController;

    [SerializeField] private GameObject indicator;
    private void Start()
    {
        if(UnitManager.Instance)
        {
            UnitManager.Instance.AddUnit(this);
        }

        unitMovement = GetComponent<UnitMovement>();
        attackController = GetComponent<AttackController>();
        SetMoveable(false);
        SetIndicator(false);
    }

    private void OnDestory()
    {
        if(UnitManager.Instance)
        {
            UnitManager.Instance.RemoveUnit(this);
        }
    }

    public void SetMoveable(bool moveable)
    {
        if(unitMovement)
            unitMovement.Moveable = moveable;
    }

    public void SetIndicator(bool flag)
    {
        if(indicator)
            indicator.SetActive(flag);
    }

    public void SetTargetToAttack(Transform target)
    {
        attackController.TargetToAttack = target;
    }
}
