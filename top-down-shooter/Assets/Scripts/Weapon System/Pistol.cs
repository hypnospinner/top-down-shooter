using System.Collections;
using UnityEngine;

class Pistol : Weapon
{
    protected override void Awake()
    {
        base.Awake();

        ReloadInput = 
            () => _inputController.RealoadingButton == ButtonState.Down && 
            _weaponData.Clip < _weaponData.ClipSize &&
            _weaponData.Ammo > 0 && 
            _isReady;

        FireInput =
            () => _inputController.LeftMouseButton == ButtonState.Down &&
            // _weaponData.Clip > 0 && 
            _isReady;
    }

    protected override IEnumerator Fire()
    {
        _isReady = false;

        _weaponData.Clip--;

        Instantiate(_weaponData.ProjectilePrefab, Muzzle.position, Muzzle.rotation);

        yield return new WaitForSeconds(_weaponData.FireRate);

        _isReady = true;
    }

    protected override IEnumerator Reload()
    {
        _isReady = false;

        yield return new WaitForSeconds(_weaponData.ReloadingTime);

        if(_weaponData.Ammo > _weaponData.ClipSize)
        {
            _weaponData.Ammo -= _weaponData.ClipSize;
            _weaponData.Clip = _weaponData.ClipSize;
        }
        else
        {
            _weaponData.Clip = _weaponData.Ammo;
            _weaponData.Ammo = 0;
        }
    }
}

