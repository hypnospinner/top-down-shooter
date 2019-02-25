using UnityEngine;

public class InteractiveObject : MonoBehaviour
{
    protected PlayerInteractor _interactor;

    public PlayerInteractor Interactor { get => _interactor; set => _interactor = value; }

    public virtual void Interact(GameObject interactor)
    {
        Debug.Log("Base Interaction call");
    }
}
