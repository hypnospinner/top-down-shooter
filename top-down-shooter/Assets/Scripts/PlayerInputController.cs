using UnityEngine;

public class PlayerInputController : MonoBehaviour
{
    #region Fields

    [SerializeField] private Camera PlayerCamera;           // reference to camera for finding point in world that mouse points
    [SerializeField] private LayerMask PointerLayerMask;    // layer mask for pointing
    
    // private fields
    private Vector2 _movementInput;                         // WS - forward and AD - right input
    private bool _isMoving;                                 // wether we made any input or not
    private Pointer _pointer;                               // storing data about place we point

    // properties
    public float ForwardInput                               // public readonly WS input
        { get => _movementInput.y; }                        
    public float RightInput                                 // public readonly AD input
        { get => _movementInput.x; }                        
    public bool IsMoving                                    // public readonly if player made an input
        { get => _isMoving; }                                
    public Vector3 PointerPosition                          // public readonly place we point in our space
        { get => _pointer.hitPoint; }                       
    public GameObject PointerObject                         // public readonly gameobject we hit
        { get => _pointer.hitGameObject; }                  

    #endregion

    #region Behaviour

    private void Awake()
    {
        _movementInput = Vector2.zero;
        _isMoving = false;

        GetPointerPosition();

        if (PlayerCamera == null)
            Debug.Log("Player Camera for calculating pointer is not set!!");
    }

    private void Update()
    {
        GetMovementInput();

        GetPointerPosition();
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

    #endregion
}

public struct Pointer
{
    public readonly Vector3 hitPoint;
    public readonly GameObject hitGameObject;

    public Pointer(Vector3 hitPoint, GameObject hitGameObject)
    {
        this.hitPoint = hitPoint;
        this.hitGameObject = hitGameObject;
    }
}
