using UnityEngine;

public delegate void InteractiveStateHandler(Interactive interactiveObject);

public class Interactive : MonoBehaviour
{
    #region Fields

    public event InteractiveStateHandler OnDestroy;     // event called when interactive is going to be destroyed

    #endregion

    #region Behaviour

    // called when player attemptes to interact
    public virtual void Interact(GameObject interactor) { }

    // notifies everyone who should know that interactive is going to be destroyed and destroyes it
    protected void DestroyInteractive()
    {
        OnDestroy?.Invoke(this);
        Destroy(gameObject);
    }

    #endregion
}
