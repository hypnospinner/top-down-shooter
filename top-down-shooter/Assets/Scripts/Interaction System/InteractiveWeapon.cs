using UnityEngine;

class InteractiveWeapon : InteractiveObject
{
    private WeaponData _weaponData;

    public override void Interact(GameObject interactor)
    {
        _weaponData =
            interactor.GetComponentInParent<WeaponController>().PickWeapon(_weaponData);

        if (_weaponData == null)
            Destroy();
    }
}
