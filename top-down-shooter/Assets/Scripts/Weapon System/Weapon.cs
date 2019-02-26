using System;
using System.Collections;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    #region Fields 

    [SerializeField] private WeaponData data;

    public virtual WeaponData Data
    {
        get => data;
        set => data = data == null ? value : data;
    }

    #endregion

    #region Behavour

    protected abstract IEnumerator Fire();

    protected abstract IEnumerator Reload();
    
    #endregion
}

[Serializable]
public class WeaponData
{
    // parameters
    [SerializeField] protected GameObject weaponPrefab;
    [SerializeField] protected GameObject projectile;
    [SerializeField] protected float fireRate;
    [SerializeField] protected float reloadingTime;
    [SerializeField] protected int clipSize;
    [SerializeField] protected int startAmmo;

    // state fields
    protected int _ammo;
    protected int _clip;

    // properties
    public int Ammo
    {
        get => _ammo;
        set
        {
            if (value >= 0)
                _ammo = value;
        }
    }
    public int Clip
    {
        get => _clip;
        set
        {
            if (value >= 0)
                _clip = value;
        }
    }
    public float FireRate { get => fireRate; }
    public float ReloadingTime { get => reloadingTime; }
    public GameObject WeaponPrefab { get => weaponPrefab; }
    public GameObject Projectile { get => projectile; }

    public WeaponData(WeaponData weaponData)
    {
        fireRate = weaponData.fireRate;
        reloadingTime = weaponData.reloadingTime;
        clipSize = weaponData.clipSize;
        startAmmo = weaponData.startAmmo;

        _ammo = weaponData.Ammo;
        _clip = weaponData.Clip;
    }
}

