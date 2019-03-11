﻿using UnityEngine;

[RequireComponent(typeof(KinematicCharacterController))]
[RequireComponent(typeof(PlayerInputController))]
public class PlayerMovementController : MonoBehaviour
{
    #region Fields

    private KinematicCharacterController _characterController;      // responsible for all movement calculations
    private PlayerInputController _playerInput;                     // responsible for input
    private PlayerController _playerController;                     // stats of player
    private Vector3 _lookDirection;                                 // where player should look at

    public Vector3 LookDirection { get => _lookDirection; }

    #endregion

    #region Behaviour

    // initializing
    private void Awake()
    {
        _characterController = GetComponent<KinematicCharacterController>();
        if (_characterController == null)
            Debug.Log("Kinematic Character Controller is not set!!!");

        _playerInput = GetComponent<PlayerInputController>();
        if (_characterController == null)
            Debug.Log("Player Input Controller is not set!!!");

        _playerController = GetComponent<PlayerController>();
        if (_characterController == null)
            Debug.Log("Player Controller is not set!!!");
    }

    // moving player
    private void FixedUpdate()
    {
        if (_playerInput.IsMoving)
            _characterController.MovePlayer(
                new Vector3(_playerInput.RightInput, 0f, _playerInput.UpInput), 
                _playerController.MovementSpeed);

        _characterController.StickPlayerToTheGround();
        _characterController.ResolveCollision();

        // rotating player
        _lookDirection = _playerInput.PointerPosition - transform.position;
        _lookDirection.y = 0;

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            Quaternion.LookRotation(_lookDirection, Vector3.up),
            _playerController.RotationSpeed * Time.fixedDeltaTime);
    }

    #endregion
}
