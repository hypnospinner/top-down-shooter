using UnityEngine;

public class InteractiveAbility : Interactive
{
    #region Fields 

    [SerializeField] private GameObject abilityPrefab;      // prefab that would be instantiated in player

    private PlayerClass _playerClass;                       // reference to player class in order not to overcall GetComponent()

    #endregion

    #region Behaviour 

    private void Awake()
    {
        _playerClass = abilityPrefab.GetComponent<Ability>().PlayerClass;
    }

    // interaction
    public override void Interact(GameObject interactor)
    {
        AbilityController abilityController = interactor.GetComponent<AbilityController>();

        if(abilityController != null)
        {
            abilityPrefab = abilityController.PickUpAbility(abilityPrefab, _playerClass);
            if (abilityPrefab == null)
                DestroyInteractive();
        }
    }
    
    #endregion
}