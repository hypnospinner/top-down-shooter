using UnityEngine;

/*  
 *  TODO: Incapuslate unnessecary fields in their calses
 *  TODO: Implement Ability System support (bounded with Inventory (or interaction system)
 */

[RequireComponent(typeof(KinematicCharacterController))]
[RequireComponent(typeof(PlayerInputController))]
public class PlayerMovementController : MonoBehaviour
{
    #region Fields

    [Header("Player Stats")]

        [SerializeField] private float MovementSpeed;               // movement speed of player
        [SerializeField] private float RotationSpeed;               // speed of player rotation

    // private fields
    private KinematicCharacterController _characterController;      // responsible for all movement calculations
    private PlayerInputController _playerInput;                     // responsible for input

    #endregion

    #region Behaviour

    private void Awake()
    {
        _characterController = GetComponent<KinematicCharacterController>();
        _playerInput = GetComponent<PlayerInputController>();
    }

    private void FixedUpdate()
    {
        // moving player
        if (_playerInput.IsMoving)
            _characterController.MovePlayer(
                new Vector2(_playerInput.ForwardInput, _playerInput.RightInput), 
                MovementSpeed);

        // rotating player
        Vector3 lookDirection = _playerInput.PointerPosition - transform.position;
        lookDirection.y = 0;

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            Quaternion.LookRotation(lookDirection, Vector3.up),
            RotationSpeed * Time.fixedDeltaTime);
    }

    #endregion
}
