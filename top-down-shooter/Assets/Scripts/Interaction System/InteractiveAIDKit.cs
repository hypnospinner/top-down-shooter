using UnityEngine;

class InteractiveAIDKit : Interactive
{
    #region Fields

    [SerializeField] DamageType AIDType;

    #endregion

    #region Behaviour

    public override void Interact(GameObject interactor)
    {
        IDamagable damagable = interactor.GetComponent<IDamagable>();

        if (damagable != null ? damagable.ReceiveAID(AIDType) : false)
            DestroyInteractive();
    }

    #endregion
}
