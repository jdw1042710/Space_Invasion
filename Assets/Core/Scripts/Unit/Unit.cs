using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{

    private UnitMovement unitMovement;
    private AttackController attackController;

    [SerializeField] private GameObject indicator;

    public float MaxHealth
    {
        get
        {
            return maxHealth;
        }
    }

    [SerializeField]
    private float maxHealth = 10;
    public float Health
    {
        get
        {
            return health;
        }
        set
        {
            health = Mathf.Max(value, 0);
            foreach(var listeners in healthListeners)
            {
                listeners(health);
            }

            if(health <= 0)
            {
                DestroyUnit();
            }
        }
    }
    private float health;
    public delegate void healthListenerDelegate(float value);
    private List<healthListenerDelegate> healthListeners = new List<healthListenerDelegate>();

    private void Awake()
    {
        unitMovement = GetComponent<UnitMovement>();
        attackController = GetComponent<AttackController>();
        Health = maxHealth;
    }
    private void Start()
    {
        if(UnitManager.Instance)
        {
            UnitManager.Instance.AddUnit(this);
        }
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
        if(attackController)
            attackController.TargetToAttack = target;
    }

    public void GetDamaged(float damage)
    {
        Health -= damage;
    }

    private void DestroyUnit()
    {
        if(UnitManager.Instance)
        {
            UnitManager.Instance.RemoveUnit(this);
        }
        Destroy(gameObject);
    }

    public void AddHealthListener(healthListenerDelegate callback)
    {
        healthListeners.Add(callback);
    }
}
