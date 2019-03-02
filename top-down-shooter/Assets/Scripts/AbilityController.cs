using UnityEngine;

public class AbilityController : MonoBehaviour
{
    #region Fields

    private GameObject _abilityGameObject;
    private PlayerInputController _inputController;

    #endregion

    #region Behaviour

    public GameObject PickUpAbility(GameObject newAbility)
    {
        if(_abilityGameObject == null)
        {
            _abilityGameObject = Instantiate(newAbility, transform, true);
            _abilityGameObject.transform.localPosition = Vector3.zero;
            Ability ability = _abilityGameObject.GetComponent<Ability>();

            if(ability != null)
                ability.InitializeAbility(gameObject);

            return null;
        }
        else
        {
            GameObject temp = _abilityGameObject;

            _abilityGameObject = Instantiate(newAbility, transform, true);

            Ability ability = _abilityGameObject.GetComponent<Ability>();

            if (ability != null)
                ability.InitializeAbility(gameObject);

            return temp;
        }
    }

    #endregion
}
