using System.Collections;
using UnityEngine;

public class HyperDashAbility : Ability
{
    #region Fields

    [SerializeField][Range(0f, .5f)] private float DashTime;
    [SerializeField][Range(0f, 5f)] private float DashDistance;

    private PlayerInputController _inputController;
    private KinematicCharacterController _characterController;
    public PlayerMovementController _movementController;

    private float speed;

    #endregion

    #region Behaviour

    private void Awake()
    {
        AbilityTrigger = () => _inputController.AbilityButton == ButtonState.Down;
    }

    public override void InitializeAbility(GameObject playerGameObject)
    {
        _inputController = playerGameObject.GetComponent<PlayerInputController>();
        if (_inputController == null)
            Debug.Log("Ability failed to get Input Controller");

        _characterController = playerGameObject.GetComponent<KinematicCharacterController>();
        if (_characterController == null)
            Debug.Log("Ability failed to get Kinematic Character Controller");

        _movementController = playerGameObject.GetComponent<PlayerMovementController>();
        if (_movementController == null)
            Debug.Log("Ability failed to get Player Movement Controller");

        speed = DashDistance / DashTime;
    }

    public override void ExecuteAbility()
    {
        StartCoroutine(DashForward());
    }

    private IEnumerator DashForward()
    {
        _inputController.Blocked = true;
        float timer = DashTime;

        Vector3 direction = _inputController.PointerPosition - _inputController.transform.position;

        while (timer > 0f)
        {
            _characterController.MovePlayer(direction, speed);
            timer -= Time.deltaTime;
            yield return null;
        }

        _inputController.Blocked = false;

        yield break;
    }

    #endregion
}
