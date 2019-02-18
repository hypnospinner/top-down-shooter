using UnityEngine;

[RequireComponent(
    typeof(PlayerInputController),
    typeof(CapsuleCollider))]
public class PlayerMovementController : MonoBehaviour
{
    #region Fields

    [Header("Movement Values")]

    [SerializeField] private float MovementSpeed;
    [SerializeField] private float MovementAcceleration;

    [Header("Player Collider Settings")]

    [SerializeField] private float PlayerHeight;
    [SerializeField] private float PlayerRadius;

    [Header("Collision Detection Settings")]

    [SerializeField] private LayerMask ObstacleLayerMask;

    [Header("Grounding Settings")]

    [SerializeField] private LayerMask GroundLayerMask;

    private PlayerInputController _inputController;
    private CapsuleCollider _playerCollider;
    private Vector2 _oldVelocityDirection;

    #endregion

    #region Behaviour

    private void Awake()
    {
        _inputController = GetComponent<PlayerInputController>();

        _playerCollider = GetComponent<CapsuleCollider>();
        _playerCollider.height = PlayerHeight;
        _playerCollider.radius = PlayerRadius;
    }

    private void FixedUpdate()
    {
        MovePlayer();
        StickPlayerToTheGround();
        ResolveCollision();
    }

    private void MovePlayer()
    {
        Vector2 _movementDirection = (Vector2.Lerp(
            new Vector2(_inputController.RightInput, _inputController.ForwardInput),
            _oldVelocityDirection,
            Time.fixedDeltaTime * MovementAcceleration)).normalized;

        transform.position += (
            transform.forward * _movementDirection.y +
            transform.right * _movementDirection.x
            ) * MovementSpeed * Time.fixedDeltaTime;

        _oldVelocityDirection = new Vector2(_inputController.RightInput, _inputController.ForwardInput);
    }

    private void StickPlayerToTheGround()
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, -transform.up);

        if (Physics.Raycast(ray, out hit, PlayerHeight * 1.5f / 2, GroundLayerMask))
            transform.position = hit.point + transform.up * PlayerHeight / 2;
        else
            transform.position -= transform.up * 9.8f * Time.fixedDeltaTime;
    }

    private void ResolveCollision()
    {
        Collider[] colliders = Physics.OverlapCapsule(
            transform.position + transform.up * (PlayerHeight - 2 * PlayerRadius) / 2,
            transform.position - transform.up * (PlayerHeight - 2 * PlayerRadius) / 2,
            PlayerRadius, ObstacleLayerMask);

        foreach (Collider collider in colliders)
        {
            Vector3 collisionResolveDirection;
            float collisionResolveDistance;

            if (Physics.ComputePenetration(
                _playerCollider, transform.position, transform.rotation,
                collider, collider.transform.position, collider.transform.rotation,
                out collisionResolveDirection, out collisionResolveDistance
                ))
                transform.position += collisionResolveDistance * collisionResolveDirection;
        }
    }

    #endregion
}
