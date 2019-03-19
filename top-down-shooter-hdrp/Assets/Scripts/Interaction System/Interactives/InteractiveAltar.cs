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

        if (manager.Stats.PlayerClass != _playerClass)
        {
            manager.Stats.PlayerClass = _playerClass;

            manager.Stats.SetGFX(_playerGFXPrefab);

            if (manager.AbilityController.HasAbilty)
                manager.AbilityController.DropAbility();
        }
    }

    #endregion
}
