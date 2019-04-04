using UnityEngine;
using UnityEngine.UI;

public delegate void AbilityStateChanged(Sprite icon);

public class AbilityController : MonoBehaviour
{
    #region Fields

    private bool _hasAbilty;                            // defines wether player has abilit or not
    private GameObject _abilityGameObject;              // reference to current picked up ability
    private PlayerStats _playerStats;                   // reference player stats component

    public AbilityStateChanged OnAbilityChanged;

    public PlayerStats PlayerStats
    {
        get => _playerStats;
        set => _playerStats = _playerStats == null ? value : _playerStats;
    }
    public bool HasAbilty { get => _hasAbilty; }

    #endregion

    #region Behaviour

    private void Awake()
    {
        _hasAbilty = false;
    }

    // inits new ability and removes old one if there was one
    public GameObject PickUpAbility(GameObject newAbility, PlayerClass playerClass)
    {
        _hasAbilty = true;

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

                OnAbilityChanged?.Invoke(ability.AbilityIcon);

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

                OnAbilityChanged?.Invoke(ability.AbilityIcon);

                // returning previous ability to be stored in interactive
                return temp;
            }
        }

        return newAbility;
    }

    public void DropAbility()
    {
        // TODO: should send request to game manager for re-spawning ability interactive
        if(_abilityGameObject != null)
            Destroy(_abilityGameObject);

        _hasAbilty = false;
    }
    #endregion
}
