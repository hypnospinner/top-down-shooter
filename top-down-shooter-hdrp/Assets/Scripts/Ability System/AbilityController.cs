using UnityEngine;

public class AbilityController : MonoBehaviour
{
    #region Fields

    private GameObject _abilityGameObject;              // reference to current picked up ability
    private PlayerStats _playerStats;                   // reference player stats component

    public PlayerStats PlayerStats
    {
        get => _playerStats;
        set => _playerStats = _playerStats == null ? value : _playerStats;
    }

    #endregion

    #region Behaviour

    // inits new ability and removes old one if there was one
    public GameObject PickUpAbility(GameObject newAbility, PlayerClass playerClass)
    {
        if (_playerStats.PlayerClass == playerClass)
        {

            if (_abilityGameObject == null)
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

        return newAbility;
    }

    #endregion
}
