using System.Collections;
using UnityEngine;

public class HyperDashAbility : Ability
{
    [SerializeField][Range(0f, .5f)] private float DashTime;
    [SerializeField][Range(0f, 5f)] private float DashDistance;

    private PlayerInputController _inputController;
    private KinematicCharacterController _characterController;
    public Transform _playerTransform;

    private float speed;

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

        _playerTransform = playerGameObject.transform;

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

        while (timer > 0f)
        {
            float angle;
            Vector3 rotationDir;
            _playerTransform.rotation.ToAngleAxis(out angle, out rotationDir);

            Debug.Log(angle);

            Vector3 direction = Quaternion.Euler(0f, angle, 0f) * _playerTransform.forward;

            _characterController.MovePlayer(direction, speed);
            timer -= Time.deltaTime;
            yield return null;
        }

        _inputController.Blocked = false;

        yield break;
    }
}
