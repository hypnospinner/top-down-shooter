using UnityEngine;

public class PenetratingProjectileController : ProjectileController
{
    #region Fields

    [SerializeField] private int PenetrationPower;

    #endregion

    #region Behaviour

    protected override void FixedUpdate()
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
            if(--PenetrationPower == 0)
                Destroy(gameObject);
        }
        transform.position += transform.forward * _stepMoveDistance;
    }

    #endregion
}
