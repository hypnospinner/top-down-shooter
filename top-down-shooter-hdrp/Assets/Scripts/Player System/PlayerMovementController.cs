using UnityEngine;

[RequireComponent(typeof(KinematicCharacterController))]
[RequireComponent(typeof(PlayerInputController))]
public class PlayerMovementController : MonoBehaviour
{
    #region Fields

    private KinematicCharacterController _characterController;      // responsible for all movement calculations
    private PlayerInputController _playerInput;                     // responsible for input
    private PlayerStats _playerStats;                               // reference on player data and values
    private Vector3 _lookDirection;                                 // where player should look at

    public Vector3 LookDirection { get => _lookDirection; }
    public KinematicCharacterController CharacterController
    {
        get => _characterController;
        set => _characterController = _characterController == null ? value : _characterController;
    }
    public PlayerInputController PlayerInput
    { 
        get => _playerInput; 
        set => _playerInput = _playerInput == null ? value : _playerInput; 
    }
    public PlayerStats PlayerStats
    {
        get => _playerStats;
        set => _playerStats = _playerStats == null ? value : _playerStats;
    }

    #endregion

    #region Behaviour

    // moving player
    private void FixedUpdate()
    {
        if (_playerInput.IsMoving)
            _characterController.MovePlayer(
                new Vector3(_playerInput.RightInput, 0f, _playerInput.UpInput), 
                _playerStats.MovementSpeed);

        _characterController.StickPlayerToTheGround();
        _characterController.ResolveCollision();

        // rotating player
        _lookDirection = _playerInput.PointerPosition - transform.position;
        _lookDirection.y = 0;

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            Quaternion.LookRotation(_lookDirection, Vector3.up),
            _playerStats.RotationSpeed * Time.fixedDeltaTime);
    }

    #endregion
}
