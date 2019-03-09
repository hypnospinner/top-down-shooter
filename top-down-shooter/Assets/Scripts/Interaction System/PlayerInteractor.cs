using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class PlayerInteractor : MonoBehaviour
{
    #region Fields    

    [SerializeField] private float InteractionRadius;                       // size for Interactive Zone
                                                                            
    private Interactive.InteractiveStateHandler _removeInteractive;   // what happens when obj is destroyed
    private SphereCollider _interactionZone;                                // sphere collider 
    private List<Interactive> _interactiveObjects;                          // interactive objects close to player
    private Interactive _currentInteractiveObject;                          // gameObject that player is going to interact with
    private PlayerInputController _inputController;                         // reference to Player Input Controller

    #endregion

    #region Behaviour 

    // initializing component
    private void Awake()
    {
        _interactiveObjects = new List<Interactive>();

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

        _removeInteractive = interactive => _interactiveObjects.Remove(interactive);
    }

    // finding closest interactive and interacting with it if there is input for that
    private void Update()
    {
        _currentInteractiveObject = FindCurrentInteractive();

        if (_inputController.IsInteracting)
            _currentInteractiveObject?.Interact(gameObject.transform.root.gameObject);
    }

    // returns closest interactive obj
    private Interactive FindCurrentInteractive()
    {
        if(_interactiveObjects.Count > 0)
        {   
            Interactive current = _interactiveObjects[0];
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

            return current;
        }

        return null;
    }

    // adds to interactive obj list if this is interactive obj
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("interactive"))
        {
            Interactive interactive = other.GetComponent<Interactive>();

            if (interactive != null)
            {
                _interactiveObjects.Add(interactive);
                interactive.OnDestroy += _removeInteractive;
            }
        }
    }

    // removes from interactive obj list if this is interactive obj
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("interactive"))
        {
            Interactive interactive = other.GetComponent<Interactive>();

            if (interactive != null)
            {
                _interactiveObjects.Remove(interactive);
                interactive.OnDestroy -= _removeInteractive;
                if (interactive == _currentInteractiveObject)
                    _currentInteractiveObject = FindCurrentInteractive();
            }
        }
    }

    #endregion
}
