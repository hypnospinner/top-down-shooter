using UnityEngine;

class InteractiveWeapon : InteractiveObject
{
    [SerializeField] private WeaponData _weaponData;

    public override void Interact(GameObject interactor)
    {
        _weaponData =
            interactor.GetComponentInChildren<WeaponController>().PickWeapon(_weaponData);

        if (_weaponData == null)
            Destroy();
    }
}
