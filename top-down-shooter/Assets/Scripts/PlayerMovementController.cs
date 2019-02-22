using UnityEngine;

// TODO: implement physics interaction (calculating hit and hitting onjects depending on their mass)
// important to be sure it's calculated in write order

[RequireComponent(typeof(PlayerInputController))]
[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(CapsuleCollider))]
public class PlayerMovementController : MonoBehaviour
{
    #region Fields

    // pubcic fields

    [Header("Movement Values")]

        [SerializeField] private float MovementSpeed;                       // how quicly player moves
        [SerializeField] private float MovementAcceleration;                // how quickly players speed changes

    [Header("Player Collider Settings")]

        [SerializeField] private float PlayerHeight;                        // player capsule collider height
        [SerializeField] private float PlayerRadius;                        // player capsule collider radius
        [SerializeField] private float GroundingSphereRadius;               // sphere that is used for checking grounding

    [Header("Collision Detection Settings")]

        [SerializeField] private LayerMask ObstacleLayerMask;               // list of layers with obstacle objects

    [Header("Grounding Settings")]

        [SerializeField] private LayerMask GroundLayerMask;                 // list of layers with everything considered to be ground
        [SerializeField] [Range(0f, 90f)] private float MaxSlopeAngle;      // max slope angle player can stand
        [SerializeField] [Range(0f, 1f)] private float MaxStepHeight;       
        [SerializeField] Transform GroundingChecker;                        // transform for grounding sphere
        [SerializeField]
            [Range(0f, .2f)] private float AutoStickToGroundDistance;       // distance player firstly sticks to the ground befor all grounding computations

    [Header("Physics settings")]

        [SerializeField] private float PlayerMass;                          // mass for calculating impact
        [SerializeField] private LayerMask PhysicsLayerMask;                // layer mask for interaction with physical objects

    // private fields

    private PlayerInputController _inputController;                         // player input handler

    private CapsuleCollider _playerCollider;                                // collider for obstacle checking
    private SphereCollider _groundingCollider;                              // collider used for checking ground

    private Vector2 _oldVelocityDirection;                                  // velocity on the previous frame 
    private float _gravityVelocity;                                         // stores velocity of gravity

    private const float k_groundSticknessAccuracy = 0.01f;                  // minimum distance to be mesured for calculating grounding
    private const float k_gravity = 9.8f;

    #endregion

    #region Behaviour

    // Warning! This is setup for a buggy physics

    //public void PushPlayer(Vector3 direction, float forceValue)
    //{
    //    Debug.Log("Push Player Call");

    //    direction = new Vector3(direction.x, 0f, direction.z).normalized;
    //    if (!Mathf.Approximately(forceValue, 0f))
    //    {
    //        Debug.Log("Added push force");
    //        _push += direction * forceValue;
    //        _isPushed = true;
    //    }
    //}

    private void Awake()
    {
        _isPushed = false;

        _inputController = GetComponent<PlayerInputController>();
        if (_inputController == null)
            Debug.LogError("Input Controller is not set!!!");

        // colliders setup

        _groundingCollider = GetComponent<SphereCollider>();
        if (_groundingCollider != null)
        {
            _groundingCollider.radius = GroundingSphereRadius;
            _groundingCollider.center = transform.up * GroundingSphereRadius;
        }
        else Debug.LogError("Player Sphere Collider is not set!!!");

        _playerCollider = GetComponent<CapsuleCollider>();
        if (_playerCollider != null)
        {
            _playerCollider.height = PlayerHeight;
            _playerCollider.radius = PlayerRadius;
            _playerCollider.center = transform.up * (PlayerHeight / 2 + MaxStepHeight);
        }
        else Debug.LogError("Player Capsule Collider is not set!!!");

        // ground checker setup

        if (GroundingChecker != null)
        {
            GroundingChecker.SetParent(transform);
            GroundingChecker.localPosition = _groundingCollider.center;
        }
        else Debug.LogError("Grounding checker is not set!!!");
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

        transform.position += 
            (_movementDirection.y * transform.forward + 
            _movementDirection.x * transform.right) * 
            MovementSpeed * Time.fixedDeltaTime;

        _oldVelocityDirection = new Vector2(_inputController.RightInput, _inputController.ForwardInput);
    }

    private void StickPlayerToTheGround()
    {
        //checking if there is any plane that player can stand on

        RaycastHit hit;

        bool canStandOnSlope = false;

        // gettign colliders that we hit after performing move
        Collider[] groundColliders = Physics.OverlapSphere(
            GroundingChecker.position, 
            GroundingSphereRadius, 
            GroundLayerMask, 
            QueryTriggerInteraction.Ignore
            );

        foreach(var groundCollider in groundColliders)
        {
            // getting direction of closest point in collider that we hit from the center of grounding sphere

            Vector3 closestPoint = groundCollider.ClosestPoint(GroundingChecker.position);

            Vector3 directionToCollider = closestPoint
                 - GroundingChecker.position;

            float distanceToCollider = directionToCollider.magnitude;

            // trying to calculate angle of normal to surface relative to transform up vector

            if (Physics.Raycast(
                GroundingChecker.position,
                directionToCollider,
                out hit,
                distanceToCollider + 0.1f,
                GroundLayerMask))
            {

                // calculating angle
                float angle = Mathf.Abs(Vector3.Angle(hit.normal, transform.up));

                // if angle is less than max slope angle we can stand on this slope
                canStandOnSlope = angle < MaxSlopeAngle ? true : canStandOnSlope;

                float moveDistance = GroundingSphereRadius - distanceToCollider;

                // if we can stand on slope -> stick to surface with normal vector
                if (canStandOnSlope && moveDistance > k_groundSticknessAccuracy)
                    transform.position += hit.normal * moveDistance;
            }
        }

        // falling if there is nothing for player to stand on
        if (!canStandOnSlope)
            transform.position += (_gravityVelocity -= k_gravity * Time.fixedDeltaTime) * transform.up * Time.fixedDeltaTime;
        else _gravityVelocity = 0f;

        // getting ground colliders that we hit after performing move
        groundColliders = Physics.OverlapSphere(
            GroundingChecker.position,
            GroundingSphereRadius,
            GroundLayerMask,
            QueryTriggerInteraction.Ignore
            );

        // resolving collision for each ground collider
        foreach(var groundCollider in groundColliders)
        {
            Vector3 collisionResolveDirection;
            float collisionResolveDistance;

            if (Physics.ComputePenetration(
                _groundingCollider,
                GroundingChecker.position,
                GroundingChecker.rotation,
                groundCollider,
                groundCollider.transform.position,
                groundCollider.transform.rotation,
                out collisionResolveDirection,
                out collisionResolveDistance
                ))
                transform.position += collisionResolveDistance * collisionResolveDirection;
        }

    }

    private void ResolveCollision()
    {
        // getting obstacle colliders that we hit after performing move
        Collider[] colliders = Physics.OverlapCapsule(
            transform.position + _playerCollider.center - (transform.up * (PlayerHeight - 2 * PlayerRadius) / 2),
            transform.position + _playerCollider.center + (transform.up * (PlayerHeight - 2 * PlayerRadius) / 2),
            PlayerRadius, 
            ObstacleLayerMask | PhysicsLayerMask,
            QueryTriggerInteraction.Ignore
            );

        // resolving collision for each collider
        foreach (Collider collider in colliders)
        {
            Vector3 collisionResolveDirection;
            float collisionResolveDistance;

            if (Physics.ComputePenetration(
                _playerCollider,
                transform.position,
                transform.rotation,
                collider,
                collider.transform.position,
                collider.transform.rotation,
                out collisionResolveDirection,
                out collisionResolveDistance
                ))
            {
                // apply force to physical objects
                if (SatisfiesMask(collider.gameObject, PhysicsLayerMask))
                {
                    Rigidbody colliderRigidbody = collider.GetComponent<Rigidbody>();

                    colliderRigidbody?.AddForce(
                        -collisionResolveDirection.normalized * MovementSpeed * PlayerMass / colliderRigidbody.mass,
                        ForceMode.Force);
                }
                
                transform.position += collisionResolveDistance * collisionResolveDirection;
            }
        }
    }

    // Warning! This is setup for a buggy physics

    //private void CalculatePush()
    //{
    //    if (_isPushed)
    //    {

    //        _push -= _push.normalized * PlayerMass * PushFriction * Time.fixedDeltaTime;
    //        Debug.Log("Calculating Push: " + _push.magnitude);

    //        if (Mathf.Approximately(_push.sqrMagnitude, 0f))
    //        {
    //            _isPushed = false;
    //            _push = Vector3.zero;
    //            return;
    //        }
    //        transform.position += _push * Time.fixedDeltaTime;
    //    }
    //}

    private bool SatisfiesMask(GameObject gameObject, LayerMask mask)
    {
        // the power for 2
        int gameObjectLayer = gameObject.layer;
        int convertedToMask = 1;

        for (int i = 0; i < gameObjectLayer; i++)
            convertedToMask = convertedToMask << 1;

        return (convertedToMask & mask.value) != 0;
    }

    #endregion
}
