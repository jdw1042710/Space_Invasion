using Unity.VisualScripting;
using UnityEngine;

public class Unit : MonoBehaviour
{

    private UnitMovement unitMovement;
    private void Start()
    {
        if(UnitManager.Instance)
        {
            UnitManager.Instance.AddUnit(this);
        }

        unitMovement = GetComponent<UnitMovement>();
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
        unitMovement.moveable = moveable;
    }
}
