using UnityEngine;

public class InteractiveAltar : Interactive
{
    #region Fields

    [SerializeField] private PlayerClass _playerClass;
    [SerializeField] private GameObject _playerGFXPrefab;

    #endregion

    #region Behaviour

    public override void Interact(GameObject interactor)
    {
        var manager = interactor.GetComponent<PlayerManager>();

        if (manager.PlayerState.PlayerClass != _playerClass)
        {
            manager.PlayerState.PlayerClass = _playerClass;

            manager.PlayerState.SetGFX(_playerGFXPrefab);

            if (manager.AbilityController.HasAbilty)
                manager.AbilityController.DropAbility();
        }
    }

    #endregion
}
