using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public static UnitManager Instance{get; set;}

    private Camera camera;

    private UnitSelectionBox selectionBox;

    private List<Unit> allUnits = new List<Unit>();
    private List<Unit> selectedUnits = new List<Unit>();

    private LayerMask ground = 1 << 3;
    private LayerMask clickable = 1 << 6;

    private bool isCursorHoveredOnEnemy = false;

    private void Awake()
    {
        if(Instance && Instance != this)
        {
            Destroy(gameObject);
        }
        Instance = this;

        selectionBox = GetComponentInChildren<UnitSelectionBox>();
    }

    private void Start()
    {
        camera = Camera.main;
    }
    
    private void Update()
    {
        UpdateSelectedUnits();
        UpdateSelectionBox();
    }

    public void AddUnit(Unit unit)
    {
        allUnits.Add(unit);
    }

    public void RemoveUnit(Unit unit)
    {
        allUnits.Remove(unit);
    }

    private void SelectUnit(Unit unit)
    {
        selectedUnits.Add(unit);
        unit.SetMoveable(true);
        unit.SetIndicator(true);
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
            SelectUnit(unit);
        }
        else
        {
            UnSelectUnit(unit);
        }
    }

    private void SelectUnits(List<Unit> units, bool IsMultiSelection)
    {
        if(!IsMultiSelection)
        {
            UnSelectAll();
        }
        foreach(Unit unit in units)
        {
            SelectUnit(unit);
        }
    }

    private void SelectUnitByRect(Rect rect)
    {
        List<Unit> units = new List<Unit>();
        foreach(var unit in allUnits)
        {
            if(rect.Contains(camera.WorldToScreenPoint(unit.transform.position)))
            {
                units.Add(unit);
            }
        }
        SelectUnits(units, InputManager.Instance.LShiftHolding);
    }

    private void UnSelectUnit(Unit unit)
    {
        unit.SetMoveable(false);
        unit.SetIndicator(false);
        selectedUnits.Remove(unit);
    }

    private void UnSelectAll()
    {
        foreach(Unit unit in selectedUnits)
        {
            unit.SetMoveable(false);
            unit.SetIndicator(false);
        }
        selectedUnits.Clear();
    }

    private void UpdateSelectedUnits()
    {
        InputManager inputManager = InputManager.Instance;
        if(!inputManager) return;
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit = new RaycastHit();
        // Selection By Clicking
        if(inputManager.LeftClickDown)
        {
            if(Physics.Raycast(ray, out hit, Mathf.Infinity, clickable) && hit.collider.CompareTag("Ally"))
            {
                Unit unit = hit.collider.gameObject.GetComponent<Unit>();
                SelectUnit(unit, InputManager.Instance.LShiftHolding);
            }
        }
        // Selection Box
        if(inputManager.LeftClickHoding)
        {
            if(selectionBox.IsBoxVisualized())
                SelectUnitByRect(selectionBox.GetBox());
        }

        // UnSelection
        if(inputManager.LeftClickUp)
        {
            if(!Physics.Raycast(ray, out hit, Mathf.Infinity, clickable)
            && !selectionBox.IsBoxVisualized()
            && !InputManager.Instance.LShiftHolding)
            {
                UnSelectAll();
            }
        }

        // Moving && Attacking
        
        if(inputManager.RightClickDown &&!inputManager.LeftClickHoding)
        {
            isCursorHoveredOnEnemy = selectedUnits.Count > 0 
                                    && Physics.Raycast(ray, out hit, Mathf.Infinity, clickable)
                                    && hit.collider.CompareTag("Enemy");
            if(isCursorHoveredOnEnemy)
            {
                //Attack
                Transform target = hit.transform;
                foreach(var unit in selectedUnits)
                {
                    //StopToMove
                    if(unit.TryGetComponent<UnitMovement>(out var unitMovement))
                        unitMovement.StopToMove();
                    if(unit.TryGetComponent<AttackController>(out var controller))
                        controller.TargetToAttack = target;
                }
            }
            else if(Physics.Raycast(ray, out hit, Mathf.Infinity, ground))
            {
                // Moving
                foreach(var unit in selectedUnits)
                {
                    if(unit.TryGetComponent<UnitMovement>(out UnitMovement unitMovement))
                        unitMovement.MoveTo(hit.point, true);
                }
            }
        }
    }

    private void UpdateSelectionBox()
    {
        InputManager inputManager = InputManager.Instance;
        if(!inputManager) return;
        if(inputManager.LeftClickDown)
        {
            selectionBox.EnterDrawing();
        }
        if(inputManager.LeftClickHoding)
        {
            selectionBox.Draw();
        }
        if(inputManager.LeftClickUp)
        {
            selectionBox.Clear();
        }
    }
}
