using UnityEngine;

public delegate void PlayerStateHandler();

public class PlayerStats : MonoBehaviour
{
    #region Fields

    public PlayerStateHandler OnPlayerDead;                 // called to inform game that player is dead by now
                                                            
    [SerializeField] private float movementSpeed;           // how quickly player moves
    [SerializeField] private float rotationSpeed;           // how quickly player rotates
    [SerializeField] private float maxHealth;               // max value for player health
    // TODO: should be encapulated
    [SerializeField] private float energy;                  // current player nenergy weapon 
    [SerializeField] private PlayerClass playerClass;       // player type
    [SerializeField] private GameObject playerGraphics;     // graphics child game object of player
                                                            
    private float _health;                                  // current health value

    // properties
    public float MaxHealth
    { get => maxHealth;  }
    public float Health
    {
        get => _health;
        set
        {
            _health = value;
            if (_health < 0) OnPlayerDead?.Invoke();
        }
    }
    public float Energy
    {
        get => energy;
        set => energy = value >= 0f ? value : 0f;
    }
    public float MovementSpeed
    { get => movementSpeed; }
    public float RotationSpeed
    { get => rotationSpeed; }
    public PlayerClass PlayerClass
    {
        get => playerClass;
        set => playerClass = value;
    }
    public GameObject PlayerGraphics
    {
        get => playerGraphics;
        set => playerGraphics = value;
    }

    #endregion

    #region Behaviour

    // resetting GFX
    public void SetGFX(GameObject gfxPrefab)
    {
        Destroy(playerGraphics);

        playerGraphics = Instantiate(gfxPrefab, transform, false) as GameObject;
        playerGraphics.transform.localPosition = Vector3.zero;
    }

    #endregion
}

public enum PlayerClass
{
    Pyramid,        // fast and furious
    Sphere,         // balanced soldier
    Cube,           // solid guy
    Octahedron      // mage
}