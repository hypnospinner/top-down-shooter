using System;
using System.Collections;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    #region Fields 

    [SerializeField] protected Transform Muzzle;
    [SerializeField] protected WeaponData _weaponData;

    protected delegate bool InputHandler();
    protected InputHandler FireInput;
    protected InputHandler ReloadInput;
    
    protected PlayerInputController _inputController;
    protected bool _isReady;

    public virtual WeaponData WeaponData
    {
        get => _weaponData;
        set => _weaponData = _weaponData == null ? value : _weaponData;
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

        Debug.Log(FireInput());
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
    [SerializeField] protected float fireRate;                      // time span between shots
    [SerializeField] protected float reloadingTime;                 // time for reloading
    [SerializeField] protected int clipSize;                        // size of clip
    [SerializeField] protected int startAmmo;                       // on the start it can't have more than these amount of ammo

    // state fields
    protected int _ammo;     // current amount of total bullets                                          
    protected int _clip;     // current amount of projectiles in clip

    // properties
    public int Ammo
    {
        get => _ammo;
        set => _ammo = value >= 0 ? value : 0;
    }
    public int Clip
    {
        get => _clip;
        set => _clip = value >= 0 ? value : 0;
    }
    public float FireRate { get => fireRate; }
    public float ReloadingTime { get => reloadingTime; }
    public GameObject WeaponPrefab { get => weaponPrefab; }
    public GameObject ProjectilePrefab { get => projectilePrefab; }
    public int ClipSize { get => clipSize; }
    public int StartAmmo { get => startAmmo; }

    public void SetWeaponData(WeaponData weaponData)
    {
        fireRate = weaponData.fireRate;
        reloadingTime = weaponData.reloadingTime;
        clipSize = weaponData.clipSize;
        startAmmo = weaponData.startAmmo;

        _ammo = weaponData.Ammo;
        _clip = weaponData.Clip;
    }
}

