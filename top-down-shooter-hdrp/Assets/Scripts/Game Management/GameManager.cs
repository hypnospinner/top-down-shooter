using UnityEngine;
using UnityEngine.SceneManagement;

namespace top_down_shooter
{
    public class GameManager : MonoBehaviour
    {
        #region Fields

        [SerializeField] private GameObject PlayerPrefab;               // instantiated in the bginning of the game
        [SerializeField] private Transform PlayerSpawn;                 // place where player should be instantiated
        [SerializeField] private GameObject PlayerUIControllerPrefab;   // prefab for UI controller 

        private PlayerManager _playerManager;                           // for storing data about everything that should happen
        private PlayerUIManager _UIManager;                             // UI controller

        #endregion

        #region Behaviour

        private void Awake()
        {
            if (PlayerPrefab != null && PlayerSpawn != null && PlayerUIControllerPrefab != null)
            {
                GameObject player = Instantiate(PlayerPrefab, PlayerSpawn.position, Quaternion.identity) as GameObject;
                GameObject UIManager = Instantiate(PlayerUIControllerPrefab) as GameObject;
                _playerManager = player.GetComponent<PlayerManager>();
                _UIManager = UIManager.GetComponent<PlayerUIManager>();

                _playerManager.Stats.OnPlayerDead +=
                    () => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

                _playerManager.Stats.OnEnergyChanged += _UIManager.UpdateEnergy;
                _playerManager.Stats.OnHealthChanged += _UIManager.UpdateHealth;
                _playerManager.Stats.OnPlayerClassChanged += _playerManager.AbilityController.DropAbility;

                _playerManager.WeaponController.OnWeaponChanged += _UIManager.SetWeaponIcon;
                _playerManager.AbilityController.OnAbilityChanged += _UIManager.SetAbilityIcon;
            }
            else Debug.Log("Game Manager enable to build scene");
        }

        #endregion
    }
}