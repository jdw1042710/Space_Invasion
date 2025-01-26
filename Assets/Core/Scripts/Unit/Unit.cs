using System.Collections.Generic;
using Unity.VisualScripting;
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
        }
    }
    private float health;
    public delegate void healthListenerDelegate(float value);
    private List<healthListenerDelegate> healthListeners = new List<healthListenerDelegate>();

    private void Awake()
    {
        unitMovement = GetComponent<UnitMovement>();
        attackController = GetComponent<AttackController>();
        SetMoveable(false);
        SetIndicator(false);
        Health = maxHealth;
    }
    private void Start()
    {
        if(UnitManager.Instance)
        {
            UnitManager.Instance.AddUnit(this);
        }
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

    public void GetDamaged(float damage)
    {
        Health -= damage;
    }

    public void AddHealthListener(healthListenerDelegate callback)
    {
        healthListeners.Add(callback);
    }
}
