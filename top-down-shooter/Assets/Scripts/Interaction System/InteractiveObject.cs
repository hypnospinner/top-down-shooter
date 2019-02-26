using UnityEngine;

public class InteractiveObject : MonoBehaviour
{
    public delegate void InteractiveObjectStateHandler(InteractiveObject interactiveObject);
    public event InteractiveObjectStateHandler OnDestroy;

    public virtual void Interact(GameObject interactor) { }

    protected void Destroy()
    {
        OnDestroy?.Invoke(this);
        Destroy(gameObject);
    }
}
