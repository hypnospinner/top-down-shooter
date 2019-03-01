using System;
using UnityEngine;

public sealed class PlayerInputController : MonoBehaviour
{
    #region Fields

    [SerializeField] private Camera PlayerCamera;           // reference to camera for finding point in world that mouse points
    [SerializeField] private LayerMask PointerLayerMask;    // layer mask for pointing
    
    // private fields
    private Vector2 _movementInput;                         // WS - forward and AD - right input
    private bool _isMoving;                                 // wether we made any input or not
    private Pointer _pointer;                               // storing data about place we point
    private bool _isInteracting;                            // wether player is attempting to interact with interactive object
    private ButtonState _leftMouseButton;                   // stands for left mouse button input
    private ButtonState _rightMouseButton;                  // stands for right mouse button input
    private ButtonState _reloadingButton;                   // stands for reloading button input
    private MouseWheelState _mouseWheel;                    // stands for mouse wheel input -1 - back, 0 - no move, 1 - forward
    private bool _isContextInteracting;                     // wether player uses context interaction possiblity

    // properties (actually available input fields)
    public float ForwardInput                               // public readonly WS input
        { get => _movementInput.y; }                        
    public float RightInput                                 // public readonly AD input
        { get => _movementInput.x; }                        
    public bool IsMoving                                    // public readonly if player made an input
        { get => _isMoving; }                                
    public Vector3 PointerPosition                          // public readonly place we point in our space
        { get => _pointer.HitPoint; }                       
    public GameObject PointerObject                         // public readonly gameobject we hit
        { get => _pointer.HitGameObject; }
    public bool IsInteracting                               // public readonly if player is trying to interact
        { get => _isInteracting; }
    public ButtonState LeftMouseButton                      // public readonly for LMB
        { get => _leftMouseButton; }
    public ButtonState RightMouseButton                     // public readonly for RMB
        { get => _rightMouseButton; }
    public ButtonState RealoadingButton                     // public readonly for realoding button (R)
        { get => _reloadingButton; }
    public MouseWheelState MouseWheel                       // public readonly for MMB
        { get => _mouseWheel; }
    public bool IsContextInteracting                        // public readonly for Q
        { get => _isContextInteracting; }


    #endregion

    #region Behaviour

    private void Awake()
    {
        _movementInput = Vector2.zero;
        _isMoving = false;

        GetPointerPosition();

        GetWeaponInput();

        if (PlayerCamera == null)
            Debug.Log("Player Camera for calculating pointer is not set!!");
    }

    private void Update()
    {
        GetMovementInput();

        GetPointerPosition();

        GetInterctionInput();

        GetWeaponInput();
    }

    private void GetMovementInput()
    {
        _movementInput.x = Input.GetAxisRaw("Horizontal");
        _movementInput.y = Input.GetAxisRaw("Vertical");

        float movementMagnitude = _movementInput.magnitude;

        _isMoving = movementMagnitude > 0f ? true : false;

        if (movementMagnitude > 1f)
        {
            _movementInput.Normalize();
            movementMagnitude = 1f;
        }
    }

    private void GetPointerPosition()
    {
        RaycastHit hit;
        Ray ray = PlayerCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 100f, PointerLayerMask, QueryTriggerInteraction.Ignore))
            _pointer = new Pointer(hit.point, hit.transform.gameObject);
    }

    private void GetInterctionInput()
    {
        // TODO: Use Input.GetButtonDown (add new Button)
        _isInteracting = Input.GetKeyDown(KeyCode.E) ? true : false;
        _isContextInteracting = Input.GetKeyDown(KeyCode.Q) ? true : false;
    }

    private void GetWeaponInput()
    {
        _leftMouseButton =
            Input.GetMouseButtonDown(0)             ?   ButtonState.Down :
            Input.GetMouseButton(0)                 ?   ButtonState.Hold :
            Input.GetMouseButtonUp(0)               ?   ButtonState.Release : 
                                                        ButtonState.Up;

        _rightMouseButton =
            Input.GetMouseButtonDown(1)             ?   ButtonState.Down :
            Input.GetMouseButton(1)                 ?   ButtonState.Hold :
            Input.GetMouseButtonUp(1)               ?   ButtonState.Release :
                                                        ButtonState.Up;
        
        // TODO: get rid of KeyCode...
        _reloadingButton =
            Input.GetKeyDown(KeyCode.R)             ?   ButtonState.Down :
            Input.GetKey(KeyCode.R)                 ?   ButtonState.Hold :
            Input.GetKeyUp(KeyCode.R)               ?   ButtonState.Release :
                                                        ButtonState.Up;

        _mouseWheel =
            Input.GetMouseButtonDown(2)             ?   MouseWheelState.Down :
            Input.GetMouseButton(2)                 ?   MouseWheelState.Hold :
            Input.GetMouseButtonUp(2)               ?   MouseWheelState.Release :
            Input.GetAxis("Mouse ScrollWheel") > 0f ?   MouseWheelState.ScrollForward :
            Input.GetAxis("Mouse ScrollWheel") < 0f ?   MouseWheelState.ScrollBackward :
                                                        MouseWheelState.Up;
    }

    #endregion
}

public struct Pointer
{
    public readonly Vector3 HitPoint;
    public readonly GameObject HitGameObject;

    public Pointer(Vector3 hitPoint, GameObject hitGameObject)
    {
        this.HitPoint = hitPoint;
        this.HitGameObject = hitGameObject;
    }
}

public enum ButtonState
{
    Down,               // button was pressed on this frame 
    Hold,               // button was pressed on this and previous frames
    Release,            // button was un pressed on this frame
    Up                  // button is not pressed
}

public enum MouseWheelState
{
    ScrollForward,      // scrolling mouse wheel forward
    ScrollBackward,     // scrolling mouse whell backward
    Down,               // button was pressed on this frame 
    Hold,               // button was pressed on this and previous frames
    Release,            // button was un pressed on this frame
    Up                  // button is not pressed
}