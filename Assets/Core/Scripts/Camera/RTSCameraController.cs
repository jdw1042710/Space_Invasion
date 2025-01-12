using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;

public class RTSCameraController : MonoBehaviour
{
    public static RTSCameraController Instance;

    [Header("General")]

    public Transform followTransform;
    [SerializeField] private Transform cameraTransform;
    private Vector3 targetPos;

    [Header("Optional")]
    [SerializeField] private bool moveWithKeyboard;
    [SerializeField] private bool moveWithEdgeScrolling;
    [SerializeField] private bool moveWithMouseDrag;

    // mouse drag movement

    [SerializeField] private Vector3 dragStartPos;
    [SerializeField] private Vector3 dragCurPos;
    private Plane upPlane = new Plane(Vector3.up, Vector3.zero);

    [Header("Keyboard Movement")]
    [SerializeField] private float highSpeed = 0.05f;
    [SerializeField] private float normalSpeed = 0.01f;
    [SerializeField] private float movementSensitivity = 1f;
    private float movementSpeed;

    [Header("Edge Scrolling Movement")]
    [SerializeField] private float edgeSize = 50f;
    private bool isCursorSet = false;
    public Texture2D cursorArrowUp = null;
    public Texture2D cursorArrowDown = null;
    public Texture2D cursorArrowLeft = null;
    public Texture2D cursorArrowRight = null;

    CursorArrow curCursor = CursorArrow.DEFAULT;
    enum CursorArrow 
    {
        UP,
        DOWN,
        LEFT,
        RIGHT,
        DEFAULT,
    }

    private void Awake()
    {
        if(Instance && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        targetPos = transform.position;
        movementSpeed = normalSpeed;
        Cursor.lockState = CursorLockMode.Confined; // If we have an extra monitor we don't want to exit screen bounds
    }

    private void Update()
    {
        FollowTarget();
    }

    private void FollowTarget()
    {
        // for tracking someone
        if (followTransform != null)
        {
            transform.position = followTransform.position;
        }
        else
        {
            HandleCameraMovement();
        }

        if (InputManager.Instance && InputManager.Instance.EscapeDown)
        {
            followTransform = null;
        }
    }

    void HandleCameraMovement()
    {
        // Mouse Drag
        if (moveWithMouseDrag)
        {
            HandleMouseDragInput();
        }
        if (moveWithKeyboard)
        {
            HandleKeyboardInput();
        }
        // Edge Scrolling
        if (moveWithEdgeScrolling)
        {
            HandleEdgeScrolling();
        }

        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * movementSensitivity);
    }

    private void HandleKeyboardInput()
    {
        movementSpeed = (InputManager.Instance && InputManager.Instance.LCtrlHolding) ? highSpeed : normalSpeed;

        if (InputManager.Instance && InputManager.Instance.UpKeyHolding)
        {
            targetPos += (transform.forward * movementSpeed);
        }
        if (InputManager.Instance && InputManager.Instance.DownKeyHolding)
        {
            targetPos += (transform.forward * -movementSpeed);
        }
        if (InputManager.Instance && InputManager.Instance.RightKeyHolding)
        {
            targetPos += (transform.right * movementSpeed);
        }
        if (InputManager.Instance && InputManager.Instance.LeftKeyHolding)
        {
            targetPos += (transform.right * -movementSpeed);
        }
    }

    private void HandleEdgeScrolling()
    {
        // Move Right
        if (Input.mousePosition.x > Screen.width - edgeSize)
        {
            targetPos += (transform.right * movementSpeed);
            ChangeCursor(CursorArrow.RIGHT);
            isCursorSet = true;
        }
        // Move Left
        else if (Input.mousePosition.x < edgeSize)
        {
            targetPos += (transform.right * -movementSpeed);
            ChangeCursor(CursorArrow.LEFT);
            isCursorSet = true;
        }
        // Move Up
        else if (Input.mousePosition.y > Screen.height - edgeSize)
        {
            targetPos += (transform.forward * movementSpeed);
            ChangeCursor(CursorArrow.UP);
            isCursorSet = true;
        }
        // Move Down
        else if (Input.mousePosition.y < edgeSize)
        {
            targetPos += (transform.forward * -movementSpeed);
            ChangeCursor(CursorArrow.DOWN);
            isCursorSet = true;
        }
        else
        {
            ChangeCursor(CursorArrow.DEFAULT);
            isCursorSet = false;
        }
    }

    private void ChangeCursor(CursorArrow newCursor)
    {
        return;

        // Only change cursor if its not the same cursor
        if (curCursor != newCursor)
        {
            switch (newCursor)
            {
                case CursorArrow.UP:
                    Cursor.SetCursor(cursorArrowUp, Vector2.zero, CursorMode.Auto);
                    break;
                case CursorArrow.DOWN:
                    Cursor.SetCursor(cursorArrowDown, new Vector2(cursorArrowDown.width, cursorArrowDown.height), CursorMode.Auto); // So the Cursor will stay inside view
                    break;
                case CursorArrow.LEFT:
                    Cursor.SetCursor(cursorArrowLeft, Vector2.zero, CursorMode.Auto);
                    break;
                case CursorArrow.RIGHT:
                    Cursor.SetCursor(cursorArrowRight, new Vector2(cursorArrowRight.width, cursorArrowRight.height), CursorMode.Auto); // So the Cursor will stay inside view
                    break;
                case CursorArrow.DEFAULT:
                    Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
                    break;
            }

            curCursor = newCursor;
        }
    }



    private void HandleMouseDragInput()
    {
        InputManager inputManager = InputManager.Instance;
        if (inputManager && inputManager.WheelKeyDown 
        && !EventSystem.current.IsPointerOverGameObject())
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (upPlane.Raycast(ray, out float entry))
            {
                dragStartPos = ray.GetPoint(entry);
            }
        }
        if (inputManager && inputManager.WheelKeyHolding
        && !EventSystem.current.IsPointerOverGameObject())
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (upPlane.Raycast(ray, out float entry))
            {
                dragCurPos = ray.GetPoint(entry);
                targetPos = transform.position + dragStartPos - dragCurPos;
            }
        }
    }
}
