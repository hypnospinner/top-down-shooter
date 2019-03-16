using UnityEngine;

public sealed class PlayerInputController : MonoBehaviour
{
    #region Fields

    [HideInInspector] public bool Blocked;                  // defines if we block getting input or not

    [SerializeField] private LayerMask PointerLayerMask;    // layer mask for pointing
    
    // private fields
    private Camera _playerCamera;                           // reference to camera for finding point in world that mouse points
    private Vector2 _movementInput;                         // WS - forward and AD - right input
    private bool _isMoving;                                 // wether we made any input or not
    private Pointer _pointer;                               // storing data about place we point
    private bool _isInteracting;                            // wether player is attempting to interact with interactive object
    private ButtonState _leftMouseButton;                   // stands for left mouse button input
    private ButtonState _rightMouseButton;                  // stands for right mouse button input
    private ButtonState _reloadingButton;                   // stands for reloading button input
    private MouseWheelState _mouseWheel;                    // stands for mouse wheel input -1 - back, 0 - no move, 1 - forward
    private bool _isContextInteracting;                     // wether player uses context interaction possiblity
    private ButtonState _abilityButton;                     // stands for SPACE button state

    // properties (actually available input fields)
    public Camera PlayerCamera                              // public once initialized ref on Camera component
    {
        get => _playerCamera;
        set => _playerCamera = _playerCamera == null ? value : _playerCamera;
    }
    public float UpInput                                    // public readonly WS input
    {
        get => _movementInput.y;
        private set => _movementInput.y = value;
    }                        
    public float RightInput                                 // public readonly AD input
    {
        get => _movementInput.x;
        private set => _movementInput.x = value;
    }                        
    public bool IsMoving                                    // public readonly if player made an input
    {
        get => _isMoving;
        private set => _isMoving = value;
    }                                
    public Vector3 PointerPosition                          // public readonly place we point in our space
    {
        get => _pointer.HitPoint;
    }
    public GameObject PointerObject                         // public readonly gameobject we hit
    {
        get => _pointer.HitGameObject;
    }
    public bool IsInteracting                               // public readonly if player is trying to interact
    {
        get => _isInteracting;
        private set => _isInteracting = value;
    }
    public ButtonState LeftMouseButton                      // public readonly for LMB
    {
        get => _leftMouseButton;
        private set => _leftMouseButton = value;
    }
    public ButtonState RightMouseButton                     // public readonly for RMB
    {
        get => _rightMouseButton;
        private set => _rightMouseButton = value;
    }
    public ButtonState ReloadingButton                      // public readonly for realoding button (R)
    {
        get => _reloadingButton;
        private set => _reloadingButton = value;
    }
    public MouseWheelState MouseWheel                       // public readonly for MMB
    {
        get => _mouseWheel;
        private set => _mouseWheel = value;
    }
    public bool IsContextInteracting                        // public readonly for Q
    {
        get => _isContextInteracting;
        private set => _isContextInteracting = value;
    }
    public ButtonState AbilityButton                        // public readonly for SPACE
    {
        get => _abilityButton;
        private set => _abilityButton = value;
    }

    #endregion

    #region Behaviour

    // initializing and setting default values

    public void InitializeComponent()
    {
        _movementInput = Vector2.zero;
        _isMoving = false;

        if (_playerCamera == null)
            Debug.Log("Player Camera for calculating pointer is not set!!");

        GetWeaponInput();
        GetPointerPosition();
    }

    // getting input
    private void Update()
    {
        GetMovementInput();

        GetPointerPosition();

        GetInterctionInput();

        GetWeaponInput();

        if (Blocked)
        {
            UpInput = 0f;
            RightInput = 0f;
            IsMoving = false;
            IsInteracting = false;
            LeftMouseButton = ButtonState.Up;
            RightMouseButton = ButtonState.Up;
            ReloadingButton = ButtonState.Up;
            MouseWheel = MouseWheelState.Up;
            IsContextInteracting = false;
            AbilityButton = ButtonState.Up;
        }
    }

    // getting movement direction
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

    // getting look at direction
    private void GetPointerPosition()
    {
        RaycastHit hit;
        Ray ray = _playerCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 100f, PointerLayerMask, QueryTriggerInteraction.Ignore))
            _pointer = new Pointer(hit.point, hit.transform.gameObject);
    }

    // Q and E input
    private void GetInterctionInput()
    {
        _isInteracting = Input.GetButtonDown("Interact") ? true : false;
        _isContextInteracting = Input.GetButtonDown("ContextInteract") ? true : false;
    }

    // Mouse input
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
        
        _reloadingButton =
            Input.GetButtonDown("Reload")           ?   ButtonState.Down :
            Input.GetButton("Reload")               ?   ButtonState.Hold :
            Input.GetButtonUp("Reload")             ?   ButtonState.Release :
                                                        ButtonState.Up;

        _mouseWheel =
            Input.GetMouseButtonDown(2)             ?   MouseWheelState.Down :
            Input.GetMouseButton(2)                 ?   MouseWheelState.Hold :
            Input.GetMouseButtonUp(2)               ?   MouseWheelState.Release :
            Input.GetAxis("Mouse ScrollWheel") > 0f ?   MouseWheelState.ScrollForward :
            Input.GetAxis("Mouse ScrollWheel") < 0f ?   MouseWheelState.ScrollBackward :
                                                        MouseWheelState.Up;

        _abilityButton =
            Input.GetButtonDown("Ability")          ?   ButtonState.Down :
            Input.GetButton("Ability")              ?   ButtonState.Hold :
            Input.GetButtonUp("Ability")            ?   ButtonState.Release :
                                                        ButtonState.Up;
    }

    #endregion
}

public struct Pointer
{
    #region Fields

    public readonly Vector3 HitPoint;           // where player was hit
    public readonly GameObject HitGameObject;   // what object was hit (for targeting)

    #endregion

    #region Behaviour

    // constructor
    public Pointer(Vector3 hitPoint, GameObject hitGameObject)
    {
        HitPoint = hitPoint;
        HitGameObject = hitGameObject;
    }

    #endregion
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