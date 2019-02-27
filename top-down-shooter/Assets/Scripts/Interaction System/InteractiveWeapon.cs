using UnityEngine;

class InteractiveWeapon : InteractiveObject
{
    [SerializeField] private WeaponData weaponDataPrefab;
    public WeaponData _weaponData;

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
}
