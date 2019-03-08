using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public delegate bool InputHandler();                        // delegate for handling events

public abstract class Weapon : MonoBehaviour
{
    #region Fields 

    [SerializeField] protected Transform Muzzle;            // place where projectiles are instantiated
    [SerializeField] protected WeaponData _weaponData;      // stores state and parameters of weapon
                                                            
    protected InputHandler FireInput;                       // logical equation for deciding wether we should fire or not
    protected InputHandler ReloadInput;                     // logical equation for deciding wether we should reload or not
                                                            
    protected PlayerInputController _inputController;       // input handler reference
    protected bool _isReady;                                // state of the weapon

    // properties
    public virtual WeaponData WeaponData
    {
        get => _weaponData;
        set => _weaponData = value;
    }

    #endregion

    #region Behavour

    protected virtual void Awake()
    {
        _isReady = true;

        _inputController = GetComponentInParent<PlayerInputController>();
        if (_inputController == null)
            Debug.LogError("Player Input Controller is not set!!!");
    }

    protected virtual void Update()
    {
        if (ReloadInput == null ? false : ReloadInput())
            StartCoroutine(Reload());

        if (FireInput == null ? false : FireInput())
            StartCoroutine(Fire());
    }

    protected abstract IEnumerator Fire();

    protected abstract IEnumerator Reload();
    
    #endregion
}

[Serializable]
[CreateAssetMenu(fileName = "New Weapon Data", menuName = "WeaponData")]
public class WeaponData : ScriptableObject
{
    // parameters
    [SerializeField] protected GameObject weaponPrefab;             // prefab of weapon for weapon data
    [SerializeField] protected GameObject projectilePrefab;         // projectile prefab for weapon
    [SerializeField] protected Color weaponIcon;                    // weapon icon
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
    public Color WeaponIcon { get => weaponIcon; }

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

    public override string ToString()
    {
        return clip.ToString();
    }
}

