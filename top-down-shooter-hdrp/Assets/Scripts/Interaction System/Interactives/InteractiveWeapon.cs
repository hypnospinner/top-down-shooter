using UnityEngine;

class InteractiveWeapon : Interactive
{
    #region Fields 

    [SerializeField] private WeaponData weaponDataPrefab;   // stores prefab for weapon
    
    #endregion
    
    #region Behaviour    

    // interaction
    public override void Interact(GameObject interactor)
    {
        var weaponData =
            interactor.GetComponentInChildren<WeaponController>().PickWeapon(weaponDataPrefab);

        if (weaponData == null)
            DestroyInteractive();
        else weaponDataPrefab = weaponData;
    }
    
    #endregion
}
