using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    public bool LeftClickDown {get; private set;}
    public bool RightClickDown {get; private set;}
    public bool LShiftHolding {get; private set;}

    private void Awake()
    {
        if(Instance && Instance != this)
        {
            Destroy(gameObject);
        }
        Instance = this;
    }

    private void Update()
    {
        LeftClickDown = Input.GetMouseButtonDown(0);
        RightClickDown = Input.GetMouseButtonDown(1);
        LShiftHolding = Input.GetKey(KeyCode.LeftShift);
    }
}