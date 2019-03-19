using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon Data", menuName = "WeaponData")]
public class WeaponData : ScriptableObject, ICloneable
{
    #region Fields

    // parameters
    [SerializeField] protected GameObject weaponPrefab;             // prefab of weapon for weapon data
    [SerializeField] protected GameObject projectilePrefab;         // projectile prefab for weapon
    [SerializeField] protected float fireRate;                      // time span between shots
    [SerializeField] protected float energyConsumption;             // energy that weapon requires to perform a shot

    // properties
    public float FireRate { get => fireRate; }
    public float EnergyConsumption { get => energyConsumption; }
    public GameObject WeaponPrefab { get => weaponPrefab; }
    public GameObject ProjectilePrefab { get => projectilePrefab; }

    #endregion

    #region Behaviour

    public object Clone()
    {
        WeaponData weaponData = CreateInstance<WeaponData>();
        weaponData.fireRate = FireRate;
        weaponData.weaponPrefab = WeaponPrefab;
        weaponData.projectilePrefab = ProjectilePrefab;
        weaponData.energyConsumption = EnergyConsumption;

        return weaponData;
    }

    public override bool Equals(object other)
    {
        if (other is WeaponData)
        {
            WeaponData weaponData = other as WeaponData;

            return weaponData.FireRate.Equals(FireRate)&&
                weaponData.EnergyConsumption.Equals(EnergyConsumption) &&
                weaponData.WeaponPrefab.Equals(WeaponPrefab) &&
                weaponData.ProjectilePrefab.Equals(ProjectilePrefab);
        }
        else return false;
    }

    #endregion
}
