using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class KinematicCharacterController: MonoBehaviour
{
    #region Fields

    // pubcic fields

    [Header("Collision Detection Settings")]

        [SerializeField] private LayerMask ObstacleLayerMask;               // list of layers with obstacle objects   

    [Header("Grounding Settings")]

        [SerializeField] private float GroundingSphereRadius;               // sphere that is used for checking grounding
        [SerializeField] private LayerMask GroundLayerMask;                 // list of layers with everything considered to be ground
        [SerializeField] [Range(0f, 90f)] private float MaxSlopeAngle;      // max slope angle player can stand
        [SerializeField] [Range(0f, 1f)] private float MaxStepHeight;       // simple way to prevent climbing steps
        [SerializeField] Transform GroundingChecker;                        // transform for grounding sphere
        [SerializeField]
        [Range(0f, .2f)] private float AutoStickToGroundDistance;           // distance player firstly sticks to the ground befor all grounding computations

    [Header("Physics settings")]

        [SerializeField] private float PushForce;                           // how power that is applied when player hits physical obj
        [SerializeField] private float PlayerMass;                          // mass for calculating impact
        [SerializeField] private LayerMask PhysicsLayerMask;                // layer mask for interaction with physical objects

    // private fields

    private SphereCollider _groundingCollider;                              // collider used for checking ground

    private float _gravityVelocity;                                         // stores velocity of gravity

    private const float k_groundSticknessAccuracy = 0.01f;                  // minimum distance to be mesured for calculating grounding
    private const float k_gravity = 9.8f;                                   // gravity acceleration

    #endregion

    #region Behaviour

    private void Awake()
    {
        // colliders setup
        _groundingCollider = GetComponent<SphereCollider>();
        if (_groundingCollider != null)
        {
            _groundingCollider.radius = GroundingSphereRadius;
            _groundingCollider.center = transform.up * GroundingSphereRadius;
        }
        else Debug.LogError("Player Sphere Collider is not set!!!");

        // ground checker setup
        if (GroundingChecker != null)
        {
            GroundingChecker.SetParent(transform);
            GroundingChecker.localPosition = _groundingCollider.center;
        }
        else Debug.LogError("Grounding checker is not set!!!");
    }

    public void MovePlayer(Vector3 direction, float velocity)
    {
        direction.Normalize();

        transform.position += direction * velocity * Time.fixedDeltaTime;
    }

    public void StickPlayerToTheGround()
    {
        //checking if there is any plane that player can stand on
        RaycastHit hit;

        bool canStandOnSlope = false;

        // gettign colliders that we hit after performing move
        Collider[] groundColliders = Physics.OverlapSphere(
            GroundingChecker.position,
            GroundingSphereRadius,
            GroundLayerMask,
            QueryTriggerInteraction.Ignore);

        foreach (var groundCollider in groundColliders)
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
            QueryTriggerInteraction.Ignore);

        // resolving collision for each ground collider
        foreach (var groundCollider in groundColliders)
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
                out collisionResolveDistance))
                transform.position += collisionResolveDistance * collisionResolveDirection;
        }

    }

    public void ResolveCollision()
    {
        Collider[] colliders = Physics.OverlapSphere(
            GroundingChecker.position,
            GroundingSphereRadius,
            ObstacleLayerMask | PhysicsLayerMask,
            QueryTriggerInteraction.Ignore);

        // resolving collision for each collider
        foreach (Collider collider in colliders)
        {
            Vector3 collisionResolveDirection;
            float collisionResolveDistance;

            if (Physics.ComputePenetration(
                _groundingCollider,
                GroundingChecker.position,
                GroundingChecker.rotation,
                collider,
                collider.transform.position,
                collider.transform.rotation,
                out collisionResolveDirection,
                out collisionResolveDistance))
            {
                // apply force to physical objects
                if (SatisfiesMask(collider.gameObject, PhysicsLayerMask))
                {
                    Rigidbody colliderRigidbody = collider.GetComponent<Rigidbody>();

                    colliderRigidbody?.AddForce(
                        -collisionResolveDirection.normalized * PushForce * PlayerMass / colliderRigidbody.mass,
                        ForceMode.Force);
                }

                transform.position +=
                    collisionResolveDistance * 
                    collisionResolveDirection;

                Debug.DrawRay(transform.position,  collisionResolveDistance * collisionResolveDirection, Color.yellow, 10f);
            }
        }
    }

    private bool SatisfiesMask(GameObject gameObject, LayerMask mask)
    {
        int gameObjectLayer = gameObject.layer;
        int convertedToMask = 1;

        for (int i = 0; i < gameObjectLayer; i++)
            convertedToMask = convertedToMask << 1;

        return (convertedToMask & mask.value) != 0;
    }

    #endregion
}