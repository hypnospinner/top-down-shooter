using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon Data", menuName = "WeaponData")]
public class WeaponData : ScriptableObject
{
    #region Fields

    // parameters
    [SerializeField] protected GameObject weaponPrefab;             // prefab of weapon for weapon data
    [SerializeField] protected GameObject projectilePrefab;         // projectile prefab for weapon
    [SerializeField] protected float fireRate;                      // time span between shots
    [SerializeField] protected float reloadingTime;                 // time for reloading
    [SerializeField] protected int clipSize;                        // size of clip
    [SerializeField] protected int clip;                            // current amount of projectiles in clip
    [SerializeField] protected int startAmmo;                       // on the start it can't have more than these amount of ammo
    [SerializeField] protected int ammo;                            // current amount of total bullets                                          

    // properties
    public int Ammo
    {
        get => ammo;
        set => ammo = value >= 0 ? value : 0;
    }
    public int Clip
    {
        get => clip;
        set => clip = value >= 0 ? value : 0;
    }

    public float FireRate { get => fireRate; }
    public float ReloadingTime { get => reloadingTime; }
    public GameObject WeaponPrefab { get => weaponPrefab; }
    public GameObject ProjectilePrefab { get => projectilePrefab; }
    public int ClipSize { get => clipSize; }
    public int StartAmmo { get => startAmmo; }

    #endregion

    #region Behaviour

    // for loading data for new weapon data object
    public void SetWeaponData(WeaponData weaponData)
    {
        weaponPrefab = weaponData.WeaponPrefab;
        projectilePrefab = weaponData.ProjectilePrefab;
        fireRate = weaponData.FireRate;
        reloadingTime = weaponData.ReloadingTime;
        clipSize = weaponData.ClipSize;
        startAmmo = weaponData.StartAmmo;

        ammo = weaponData.Ammo;
        clip = weaponData.Clip;
    }

    #endregion
}
