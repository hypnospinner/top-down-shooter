using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon Data", menuName = "WeaponData/Basic")]
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

    // for memberwise cloning
    public virtual object Clone()
    {
        WeaponData clone = CreateInstance<WeaponData>();
        clone.fireRate = FireRate;
        clone.weaponPrefab = WeaponPrefab;
        clone.projectilePrefab = ProjectilePrefab;
        clone.energyConsumption = EnergyConsumption;

        return clone;
    }

    // value equality check
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

[CreateAssetMenu(fileName ="New Shogun Weapon Data", menuName = "WeaponData/Shotgun")]
public class ShotgunWeaponData : WeaponData
{
    #region Fields

    [SerializeField] private int fireDensity;                           // how many bullets should be spawned when player shots
    [SerializeField] private float positionScatter;                     // how bullet should be moved when shooting
    [SerializeField] private float rotationScatter;                     // max angle that bullet can rotate

    public int FireDensity { get => fireDensity; }
    public float PositionScatter { get => positionScatter; }
    public float RotationScatter { get => rotationScatter; }

    #endregion

    #region Behaviour

    public override object Clone()
    {
        ShotgunWeaponData clone = CreateInstance<ShotgunWeaponData>();
        clone.fireRate = FireRate;
        clone.weaponPrefab = WeaponPrefab;
        clone.projectilePrefab = ProjectilePrefab;
        clone.energyConsumption = EnergyConsumption;
        clone.fireDensity = FireDensity;
        clone.positionScatter = PositionScatter;
        clone.rotationScatter = RotationScatter;

        return clone;
    }

    public override bool Equals(object other)
    {
        if (other is ShotgunWeaponData)
        {
            var weaponData = other as ShotgunWeaponData;

            return weaponData.FireRate.Equals(FireRate) &&
                weaponData.EnergyConsumption.Equals(EnergyConsumption) &&
                weaponData.WeaponPrefab.Equals(WeaponPrefab) &&
                weaponData.ProjectilePrefab.Equals(ProjectilePrefab) &&
                weaponData.FireDensity.Equals(FireDensity) && 
                weaponData.PositionScatter.Equals(PositionScatter) &&
                weaponData.RotationScatter.Equals(RotationScatter);
        }
        else return false;
    }

    #endregion
}
