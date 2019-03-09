using UnityEngine;

public class AbilityController : MonoBehaviour
{
    #region Fields

    private GameObject _abilityGameObject;              // reference to current picked up ability

    #endregion

    #region Behaviour

    // inits new ability and removes old one if there was one
    public GameObject PickUpAbility(GameObject newAbility)
    {
        if(_abilityGameObject == null)
        {
            _abilityGameObject = Instantiate(newAbility, transform, true);
            _abilityGameObject.transform.localPosition = Vector3.zero;
            Ability ability = _abilityGameObject.GetComponent<Ability>();

            // initializing picked up ability with player gameObject
            if (ability != null)
                ability.InitializeAbility(gameObject);

            return null;
        }
        else
        {
            // swapping abilities
            GameObject temp = _abilityGameObject;

            _abilityGameObject = Instantiate(newAbility, transform, true);

            Ability ability = _abilityGameObject.GetComponent<Ability>();

            // initializing picked up ability with player gameObject
            if (ability != null)
                ability.InitializeAbility(gameObject);
            
            // returning previous ability to be stored in interactive
            return temp;
        }
    }

    #endregion
}
