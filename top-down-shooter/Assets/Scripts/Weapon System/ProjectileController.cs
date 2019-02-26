using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    #region Fields 

    [SerializeField] private float AttackDistance;      // distance that projectile will cover (afterwards it will be destroyed)
    [SerializeField] private float ProjectileSpeed;     // projectile movement speed (using racast for accuracy on high speed)
    [SerializeField] private LayerMask AttackMask;      // layer for hitbox colliders

    private float _stepMoveDistance;                    // precalculated distance of movement on 1 frame

    #endregion

    #region Behaviour

    private void Awake()
    {
        _stepMoveDistance = ProjectileSpeed * Time.fixedDeltaTime;
        Destroy(gameObject, AttackDistance / ProjectileSpeed);
    }

    private void FixedUpdate()
    {
        RaycastHit hit;

        if(Physics.Raycast(
            transform.position, 
            transform.forward, 
            out hit, 
            _stepMoveDistance, 
            AttackMask, 
            QueryTriggerInteraction.Ignore))
        {
            Debug.Log("Hit " + hit.transform.name);
            Destroy(gameObject);
        }

        transform.position += transform.forward * _stepMoveDistance;
    }

    #endregion

}
