using UnityEngine;
using UnityEngine.SceneManagement;

namespace top_down_shooter
{
    public class GameManager : MonoBehaviour
    {
        #region Fields

        [SerializeField] private GameObject PlayerPrefab;               // instantiated in the bginning of the game
        [SerializeField] private Transform PlayerSpawn;                 // place where player should be instantiated

        private PlayerManager _playerManager;                     // for storing data about everything that should happen

        #endregion

        #region Behaviour

        private void Awake()
        {
            if (PlayerPrefab != null && PlayerSpawn != null)
            {
                GameObject player = Instantiate(PlayerPrefab, PlayerSpawn.position, Quaternion.identity) as GameObject;

                _playerManager = player.GetComponent<PlayerManager>();

                _playerManager.PlayerState.OnPlayerDead +=
                    () => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
            else Debug.Log("Game Manager disable to build scene");
        }

        #endregion
    }
}