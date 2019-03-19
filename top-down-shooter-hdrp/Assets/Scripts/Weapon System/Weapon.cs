using System.Collections;
using UnityEngine;

public delegate bool InputHandler();

public abstract class Weapon : MonoBehaviour
{
    #region Fields 

    [SerializeField] protected Transform Muzzle;            // place where projectiles are instantiated
    [SerializeField] protected WeaponData _weaponData;      // stores state and parameters of weapon

    protected InputHandler FireInput;                       // logical equation for deciding wether we should fire or not
    protected PlayerInputController _inputController;       // input handler reference
    protected bool _isReady;                                // state of the weapon
    protected PlayerManager _manager;                       // reference to player manager component

    // properties
    public virtual WeaponData WeaponData
    {
        get => _weaponData;
        set => _weaponData = value;
    }
    public PlayerManager Manager
    {
        get => _manager;
        set => _manager = _manager == null ? value : _manager;
    }
    public PlayerInputController InputController
    {
        get => _inputController;
        set => _inputController = _inputController == null ? value : _inputController;
    }

    #endregion

    #region Behavour

    // initializing
    public virtual void InitializeWeapon()
    {
        _isReady = true;
    }

    // checking wether to reload or shoot 
    protected virtual void Update()
    {
        if (FireInput == null ? false : FireInput())
            StartCoroutine(Fire());
    }

    // shooting
    protected abstract IEnumerator Fire();

    #endregion
}