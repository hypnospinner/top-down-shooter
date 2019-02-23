using UnityEngine;

public class InteractiveObject : MonoBehaviour
{
    public virtual void Interact(GameObject interactor)
    {
        Debug.Log("Base Interaction call");
    }
}
