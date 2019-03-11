using System.Collections;
using UnityEngine;

public delegate bool InputHandler();

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

    // initializing (there player should define 
    protected virtual void Awake()
    {
        _isReady = true;

        _inputController = GetComponentInParent<PlayerInputController>();
        if (_inputController == null)
            Debug.LogError("Player Input Controller is not set!!!");
    }

    // checking wether to reload or shoot 
    protected virtual void Update()
    {
        if (ReloadInput == null ? false : ReloadInput())
            StartCoroutine(Reload());

        if (FireInput == null ? false : FireInput())
            StartCoroutine(Fire());
    }

    // shooting
    protected abstract IEnumerator Fire();

    // reloading
    protected abstract IEnumerator Reload();
    
    #endregion
}