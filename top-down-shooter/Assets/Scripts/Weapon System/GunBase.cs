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

    #endregion

    #region Behaviour

    protected virtual void Awake()
    {
        _isReady = true;

        _inputController = transform.root.GetComponent<PlayerInputController>();
        if (_inputController == null)
            Debug.LogError("PLayer Input Controller is not set!!!");
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
