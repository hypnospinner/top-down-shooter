using UnityEngine;

[RequireComponent(typeof(DamageSender))]
public class ProjectileController : MonoBehaviour
{
    #region Fields 

    [SerializeField] protected float AttackDistance;      // distance that projectile will cover (afterwards it will be destroyed)
    [SerializeField] protected float ProjectileSpeed;     // projectile movement speed (using racast for accuracy on high speed)
    [SerializeField] protected LayerMask AttackMask;      // layer for hitbox colliders

    protected float _stepMoveDistance;                    // precalculated distance of movement on 1 frame
    protected DamageSender _damageSender;                 // stores data about damage

    #endregion

    #region Behaviour

    // initializing
    protected virtual void Awake()
    {
        _damageSender = GetComponent<DamageSender>();
        _stepMoveDistance = ProjectileSpeed * Time.fixedDeltaTime;
        Destroy(gameObject, AttackDistance / ProjectileSpeed);
    }

    // checking for hit
    protected virtual void FixedUpdate()
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
            _damageSender.SendDamageTo(hit.transform.gameObject);
            Destroy(gameObject);
        }
        transform.position += transform.forward * _stepMoveDistance;
    }

    #endregion

}
