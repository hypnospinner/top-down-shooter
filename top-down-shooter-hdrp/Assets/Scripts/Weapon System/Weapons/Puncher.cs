using System.Collections;
using UnityEngine;

public class Puncher : Weapon
{
    private ShotgunWeaponData _sWeaponData;

    public override void InitializeWeapon()
    {
        base.InitializeWeapon();

        FireInput =
            () => (_inputController.LeftMouseButton == ButtonState.Down ||
            _inputController.LeftMouseButton == ButtonState.Hold) &&
            _manager.Stats.Energy > _weaponData.EnergyConsumption &&
            _isReady;

        _sWeaponData = _weaponData as ShotgunWeaponData;
    }

    protected override IEnumerator Fire()
    {
        _isReady = false;

        _manager.Stats.Energy -= _sWeaponData.EnergyConsumption;

        for (int i = 0; i < _sWeaponData.FireDensity; i++)
        {
            // generating scatter
            Vector3 scatteredPosition = Muzzle.position +
                Muzzle.up * Random.Range(-_sWeaponData.PositionScatter, _sWeaponData.PositionScatter) +
                Muzzle.right * Random.Range(-_sWeaponData.PositionScatter, _sWeaponData.PositionScatter);

            Vector3 scatteredRotation = Muzzle.rotation.eulerAngles;

            scatteredRotation.x += Random.Range(-_sWeaponData.RotationScatter, _sWeaponData.RotationScatter);
            scatteredRotation.y += Random.Range(-_sWeaponData.RotationScatter, _sWeaponData.RotationScatter);

            // instantiating bullet
            Instantiate(_sWeaponData.ProjectilePrefab, scatteredPosition, Quaternion.Euler(scatteredRotation.x, scatteredRotation.y, scatteredRotation.z));
        }

        yield return new WaitForSeconds(_sWeaponData.FireRate);

        _isReady = true;
    }
}
