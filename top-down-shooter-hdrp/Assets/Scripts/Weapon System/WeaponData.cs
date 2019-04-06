﻿using System;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Weapon Data", menuName = "WeaponData/Basic")]
public class WeaponData : ScriptableObject, ICloneable
{
    #region Fields

    // parameters
    [SerializeField] protected GameObject weaponPrefab;             // prefab of weapon for weapon data
    [SerializeField] protected GameObject projectilePrefab;         // projectile prefab for weapon
    [SerializeField] protected float fireRate;                      // time span between shots
    [SerializeField] protected float energyConsumption;             // energy that weapon requires to perform a shot
    [SerializeField] protected Sprite weaponIcon;                    // weapon sign

    // properties
    public float FireRate => fireRate;
    public float EnergyConsumption => energyConsumption;
    public GameObject WeaponPrefab => weaponPrefab;
    public GameObject ProjectilePrefab => projectilePrefab;
    public Sprite WeaponIcon => weaponIcon;

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
        clone.weaponIcon = WeaponIcon;

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
                weaponData.ProjectilePrefab.Equals(ProjectilePrefab) &&
                weaponData.WeaponIcon.Equals(WeaponIcon);
        }
        else return false;
    }

    #endregion
}
