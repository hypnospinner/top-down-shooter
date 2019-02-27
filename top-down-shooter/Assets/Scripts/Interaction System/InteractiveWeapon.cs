using UnityEngine;

class InteractiveWeapon : InteractiveObject
{
    #region Fields 

    [SerializeField] private WeaponData weaponDataPrefab;   // stores prefab for weapon
    private WeaponData _weaponData;                         // stores actual value stored in 
    
    #endregion
    
    #region Behaviour    

    private void Awake()
    {
        if (weaponDataPrefab == null)
            Debug.LogError("Weapon Data Prefab is null!!!");

        _weaponData = ScriptableObject.CreateInstance<WeaponData>();
        _weaponData.SetWeaponData(weaponDataPrefab);
    }

    public override void Interact(GameObject interactor)
    {
        _weaponData =
            interactor.GetComponentInChildren<WeaponController>().PickWeapon(_weaponData);

        if (_weaponData == null)
            Destroy();
    }
    
    #endregion
}
