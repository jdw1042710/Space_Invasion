using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public static UnitManager Instance{get; set;}

    private Camera camera;

    private List<Unit> allUnits = new List<Unit>();
    [SerializeField] private List<Unit> selectedUnits = new List<Unit>();

    private LayerMask ground = 1 << 3;
    private LayerMask clickable = 1 << 6;

    private void Awake()
    {
        if(Instance && Instance != this)
        {
            Destroy(gameObject);
        }
        Instance = this;
    }

    private void Start()
    {
        camera = Camera.main;
    }
    
    private void Update()
    {
        if(InputManager.Instance && InputManager.Instance.LeftClickDown)
        {
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, clickable))
            {
                Unit unit = hit.collider.gameObject.GetComponent<Unit>();
                SelectUnit(unit, InputManager.Instance.LShiftHolding);
            }
            else
            {
                if(!InputManager.Instance.LShiftHolding)
                    UnSelectAll();
            }
        }
    }

    public void AddUnit(Unit unit)
    {
        allUnits.Add(unit);
    }

    public void RemoveUnit(Unit unit)
    {
        allUnits.Remove(unit);
    }

    private void SelectUnit(Unit unit, bool IsMultiSelection)
    {
        bool isContained = selectedUnits.Contains(unit);
        if(!IsMultiSelection)
        {
            UnSelectAll();
        }
        if(!isContained)
        {
            selectedUnits.Add(unit);
            unit.SetMoveable(true);
        }
        else
        {
            UnSelectUnit(unit);
        }
    }

    private void SelectUnits(List<Unit> units, bool IsMultiSelection)
    {
        if(IsMultiSelection)
        {
            selectedUnits.AddRange(units);
        }
        else
        {
            selectedUnits = units;
        }

        foreach(Unit unit in selectedUnits)
        {
            unit.SetMoveable(true);
        }
    }

        private void UnSelectUnit(Unit unit)
    {
        unit.SetMoveable(false);
        selectedUnits.Remove(unit);
    }

    private void UnSelectAll()
    {
        foreach(Unit unit in selectedUnits)
        {
            unit.SetMoveable(false);
        }
        selectedUnits.Clear();
    }
}
