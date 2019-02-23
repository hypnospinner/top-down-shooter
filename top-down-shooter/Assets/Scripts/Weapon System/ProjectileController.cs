using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    #region Fields 

    [SerializeField] private float AttackDistance;
    [SerializeField] private float ProjectileSpeed;
    [SerializeField] private LayerMask AttackMask;

    private float _stepMoveDistance;

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
