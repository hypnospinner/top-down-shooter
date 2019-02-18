using UnityEngine;

public class PlayerInputController : MonoBehaviour
{
    private Vector2 _movementInput;
    private bool _isMoving;

    public float ForwardInput { get => _movementInput.y; }
    public float RightInput { get => _movementInput.x; }
    public bool IsMoving { get => _isMoving; }

    private void Awake()
    {
        _movementInput = Vector2.zero;
        _isMoving = false;
    }

    private void Update()
    {
        _movementInput.x = Input.GetAxisRaw("Horizontal");
        _movementInput.y = Input.GetAxisRaw("Vertical");

        float movementMagnitude = _movementInput.magnitude;

        _isMoving = movementMagnitude > 0f ? true : false;

        if(movementMagnitude > 1f)
        {
            _movementInput.Normalize();
            movementMagnitude = 1f;
        }
    }

}
