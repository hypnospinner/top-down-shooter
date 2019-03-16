using UnityEngine;

class InteractiveAIDKit : Interactive
{
    #region Fields

    [SerializeField] DamageType AIDType;        // AID kit type

    #endregion

    #region Behaviour
    
    // interaction
    public override void Interact(GameObject interactor)
    {
        var damageReceiver = interactor.GetComponent<PlayerDamageReveiver>();

        if (damageReceiver != null && damageReceiver.ReceiveAID(AIDType))
            DestroyInteractive();
    }

    #endregion
}
