using UnityEngine;

class InteractiveHealth : Interactive
{
    #region Fields

    [SerializeField] public float StoredHealth;

    #endregion

    #region Behaviour
    
    // interaction
    public override void Interact(GameObject interactor)
    {
        var manager = interactor.GetComponent<PlayerManager>();
        manager.PlayerDamageReceiver.RecieveHealth(StoredHealth);
        DestroyInteractive();
    }

    #endregion
}
