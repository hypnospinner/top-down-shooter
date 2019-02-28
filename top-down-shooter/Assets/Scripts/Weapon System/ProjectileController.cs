using UnityEngine;

[RequireComponent(typeof(DamageSender))]
public class ProjectileController : MonoBehaviour
{
    #region Fields 

    [SerializeField] private float AttackDistance;      // distance that projectile will cover (afterwards it will be destroyed)
    [SerializeField] private float ProjectileSpeed;     // projectile movement speed (using racast for accuracy on high speed)
    [SerializeField] private LayerMask AttackMask;      // layer for hitbox colliders

    private float _stepMoveDistance;                    // precalculated distance of movement on 1 frame
    private DamageSender _damageSender;                 // stores data about damage

    #endregion

    #region Behaviour

    private void Awake()
    {
        _damageSender = GetComponent<DamageSender>();
        _stepMoveDistance = ProjectileSpeed * Time.fixedDeltaTime;
        Destroy(gameObject, AttackDistance / ProjectileSpeed);
    }

    private void FixedUpdate()
    {
        RaycastHit hit;

        if (Physics.Raycast(
            transform.position,
            transform.forward,
            out hit,
            _stepMoveDistance,
            AttackMask,
            QueryTriggerInteraction.Ignore))
        {

            Destroy(gameObject);
        }
        transform.position += transform.forward * _stepMoveDistance;
    }

    #endregion

}
