using System.Collections;
using UnityEngine;

class AssaultRifle : Weapon
{
    #region Behaviour

    // initializing
    public override void InitializeWeapon()
    {
        base.InitializeWeapon();

        FireInput =
            () => (_inputController.LeftMouseButton == ButtonState.Down || 
            _inputController.LeftMouseButton == ButtonState.Hold) && 
            _manager.Stats.Energy > _weaponData.EnergyConsumption && 
            _isReady;
    }

    // shooting
    protected override IEnumerator Fire()
    {
        _isReady = false;

        _manager.Stats.Energy -= _weaponData.EnergyConsumption;

        Instantiate(_weaponData.ProjectilePrefab, Muzzle.position, Muzzle.rotation);

        yield return new WaitForSeconds(_weaponData.FireRate);

        _isReady = true;
    }

    #endregion
}


