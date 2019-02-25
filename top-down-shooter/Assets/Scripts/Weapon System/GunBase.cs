using System.Collections;
using UnityEngine;

public abstract class GunBase : MonoBehaviour
{
    #region Fields 

    [SerializeField] protected GameObject Projectile;
    [SerializeField] protected Transform Muzzle;

    // private variables
    protected delegate bool CheckStateDelegate();
    protected CheckStateDelegate _reloadChecker;
    protected CheckStateDelegate _attackChecker;
    protected PlayerInputController _inputController;
    protected bool _isReady;
    protected bool _ableToGetInput;

    public PlayerInputController InputController
    {
        get => _inputController;
        set
        {
            _ableToGetInput = value == null ? false : true;
            _inputController = value;
        }
    }

    #endregion

    #region Behaviour

    protected virtual void Awake()
    {
        _ableToGetInput = false;
        _isReady = true;
    }

    protected virtual void Update()
    {
        if (_reloadChecker())
            RunReload();

        if (_attackChecker())
            RunAttack();
    }

    protected abstract void RunReload();

    protected abstract void RunAttack();

    protected abstract IEnumerator Reload();

    protected abstract IEnumerator Attack();

    #endregion
}
