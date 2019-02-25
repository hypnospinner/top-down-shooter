using System.Collections;
using UnityEngine;

public sealed class PistolController : GunBase
{
    #region Fields 

    [SerializeField] private float ReloadingTime;
    [SerializeField] private float FireRate;
    [SerializeField] private int ClipSize;
    [SerializeField] private int MaxAmmo;

    private int _clip;
    private int _ammo;

    #endregion

    #region Behaviour

    protected override void Awake()
    {
        base.Awake();

        _clip = ClipSize;
        _ammo = MaxAmmo;

        _reloadChecker = () => _ableToGetInput ? _inputController.RealoadingButton == ButtonState.Down : false;
        _attackChecker = () => _ableToGetInput ? _inputController.LeftMouseButton == ButtonState.Down : false;
    }

    protected override void RunAttack()
    {
        if (_isReady && _clip > 0)
            StartCoroutine(Attack());
    }

    protected override void RunReload()
    {
        if(_clip < ClipSize && _ammo > 0)
        {
            StartCoroutine(Reload());
        }
    }

    protected override IEnumerator Attack()
    {
        _isReady = false;
        _clip--;

        Instantiate(Projectile, Muzzle.position, Muzzle.rotation);

        yield return new WaitForSeconds(FireRate);

        _isReady = true;
    }

    protected override IEnumerator Reload()
    {
        _isReady = false;

        yield return new WaitForSeconds(ReloadingTime);

        if(_ammo > (ClipSize - _clip))
        {
            _ammo -= ClipSize - _clip;
            _clip = ClipSize;
        } else
        {
            _clip += _ammo;
            _ammo = 0;
        }

        _isReady = true;
    }

    #endregion
}
