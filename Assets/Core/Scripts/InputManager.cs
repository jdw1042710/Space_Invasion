using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    public bool LeftClickDown {get; private set;}
    public bool LeftClickHoding {get; private set;}
    public bool LeftClickUp {get; private set;}

    public bool RightClickDown {get; private set;}

    public bool WheelKeyDown {get; private set;}
    public bool WheelKeyHolding {get; private set;}

    public bool UpKeyHolding {get; private set;}
    public bool DownKeyHolding {get; private set;}

    public bool LeftKeyHolding {get; private set;}

    public bool RightKeyHolding {get; private set;}


    public bool LShiftHolding {get; private set;}
    public bool LCtrlHolding {get; private set;}

    public bool EscapeDown {get; private set;}

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
        LeftClickHoding = Input.GetMouseButton(0);
        LeftClickUp = Input.GetMouseButtonUp(0);
        RightClickDown = Input.GetMouseButtonDown(1);
        WheelKeyDown = Input.GetMouseButtonDown(2);
        WheelKeyHolding = Input.GetMouseButton(2);
        UpKeyHolding = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);
        DownKeyHolding = Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow);
        LeftKeyHolding = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow);
        RightKeyHolding = Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow);
        LShiftHolding = Input.GetKey(KeyCode.LeftShift);
        LCtrlHolding = Input.GetKey(KeyCode.LeftCommand);
        EscapeDown = Input.GetKeyDown(KeyCode.Escape);
    }
}
