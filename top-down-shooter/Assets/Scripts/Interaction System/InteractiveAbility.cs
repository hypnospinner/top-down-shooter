using UnityEngine;

public class InteractiveAbility : Interactive
{
    #region Fields 

    [SerializeField] private GameObject abilityPrefab;

    #endregion
    
    #region Behaviour 

    public override void Interact(GameObject interactor)
    {
        AbilityController abilityController = interactor.GetComponent<AbilityController>();

        if(abilityController != null)
        {
            abilityPrefab = abilityController.PickUpAbility(abilityPrefab);
            if (abilityPrefab == null)
                DestroyInteractive();
        }
    }
    
    #endregion
}