using UnityEngine;

public class Interactive : MonoBehaviour
{
    public delegate void InteractiveObjectStateHandler(Interactive interactiveObject);
    public event InteractiveObjectStateHandler OnDestroy;

    public virtual void Interact(GameObject interactor) { }

    protected void DestroyInteractive()
    {
        OnDestroy?.Invoke(this);
        Destroy(gameObject);
    }
}
