using System.Collections.Generic;
using UnityEngine;

// TODO: Add support for highlighing interactive objects

[RequireComponent(typeof(SphereCollider))]
public class PlayerInteractor : MonoBehaviour
{
    #region Fields    

    [SerializeField] private float InteractionRadius;

    private SphereCollider _interactionZone;
    private List<InteractiveObject> _interactiveObjects;
    private InteractiveObject _currentInteractiveObject;
    private PlayerInputController _inputController;

    #endregion

    #region Behaviour 

    private void Awake()
    {
        _interactiveObjects = new List<InteractiveObject>();

        _inputController = transform.root.GetComponent<PlayerInputController>();
        if (_inputController == null)
            Debug.LogError("Player Input Controller is not set!!!");

        _interactionZone = GetComponent<SphereCollider>();
        if (_interactionZone != null)
        {
            _interactionZone.isTrigger = true;
            _interactionZone.radius = InteractionRadius;
            _interactionZone.center = new Vector3(0f, InteractionRadius / 3, InteractionRadius / 2);
        }
        else Debug.LogError("Interaction Zone Collider is not set!!!");
    }

    private void Update()
    {
        _currentInteractiveObject = FindCurrentInteractive();

        if (_inputController.IsInteracting)
            _currentInteractiveObject?.Interact(gameObject.transform.root.gameObject);
    }

    private InteractiveObject FindCurrentInteractive()
    {
        if(_interactiveObjects.Count > 0)
        {   
            InteractiveObject current = _interactiveObjects[0];
            float sqrDistance = (transform.position - current.transform.position).sqrMagnitude;

            foreach (var interactiveObject in _interactiveObjects)
            {
                float checkSqrDistance = (transform.position - interactiveObject.transform.position).sqrMagnitude;

                if(checkSqrDistance < sqrDistance)
                {
                    sqrDistance = checkSqrDistance;
                    current = interactiveObject;
                }
            }
            Debug.Log(current.gameObject);

            return current;
        }

        return null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("interactive"))
        {
            Debug.Log("Adding interactive");
            InteractiveObject interactive = other.GetComponent<InteractiveObject>();

            if (interactive != null)
                _interactiveObjects.Add(interactive);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("interactive"))
        {
            Debug.Log("Removing interactive");

            InteractiveObject interactive = other.GetComponent<InteractiveObject>();

            if (interactive != null)
            {
                _interactiveObjects.Remove(interactive);

                if (interactive == _currentInteractiveObject)
                    _currentInteractiveObject = FindCurrentInteractive();
            }
        }
    }
    #endregion
}
