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
        var playerController = interactor.GetComponent<PlayerController>();

        if (playerController != null && playerController.ReceiveAID(AIDType))
            DestroyInteractive();
    }

    #endregion
}
