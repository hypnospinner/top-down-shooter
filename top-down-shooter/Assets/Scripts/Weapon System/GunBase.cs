using System.Collections;
using UnityEngine;

public abstract class GunBase : MonoBehaviour
{
    #region Fields 

    // delegates
    protected delegate bool CheckForButtonState(ButtonState buttonState);

    // private variables
    protected PlayerInputController _inputController;
    protected bool _isReady;

    #endregion

    #region Behaviour

    protected void Awake()
    {
        _isReady = true;

        _inputController = transform.root.GetComponent<PlayerInputController>();
        if (_inputController == null)
            Debug.LogError("PLayer Input Controller is not set!!!");
    }

    protected abstract void RunReload(CheckForButtonState ButtonStateChecker);

    protected abstract void RunAttack(CheckForButtonState ButtonStateChecker);

    protected abstract IEnumerator Reload();

    protected abstract IEnumerator Attack();

    #endregion
}
