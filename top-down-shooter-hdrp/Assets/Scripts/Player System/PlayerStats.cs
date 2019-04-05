using UnityEngine;

public delegate void PlayerStateHandler();
public delegate void PlayerPropertyChanged(float newValue);

public class PlayerStats : MonoBehaviour
{
    #region Fields

    public PlayerStateHandler OnPlayerDead;                 // called to inform game that player is dead by now
    public PlayerStateHandler OnPlayerClassChanged;         // informs that player class is changed
    public PlayerPropertyChanged OnHealthChanged;           // informs that health value is changed
    public PlayerPropertyChanged OnEnergyChanged;           // informs that energy value is changed

    [SerializeField] private float movementSpeed;           // how quickly player moves
    [SerializeField] private float rotationSpeed;           // how quickly player rotates
    [SerializeField] private float maxHealth;               // max value for player health
    [SerializeField] private float StartEnergy;             // how much energy player should have on start
    [SerializeField] private PlayerClass playerClass;       // player type
    [SerializeField] private GameObject playerGraphics;     // graphics child game object of player
                                                            
    private float _energy;                                  // current player nenergy weapon 
    private float _health;                                  // current health value

    // properties
    public float MaxHealth => maxHealth; 
    public float Health
    {
        get => _health;
        set
        {
            _health = Mathf.Clamp(value, -1f, 100f);
            OnHealthChanged?.Invoke(_health);
            if (_health < 0) OnPlayerDead?.Invoke();
        }
    }
    public float Energy
    {
        get => _energy;
        set
        {
            _energy = value >= 0 ? value : 0;
            OnEnergyChanged?.Invoke(_energy);
        }
    }
    public float MovementSpeed => movementSpeed;
    public float RotationSpeed => rotationSpeed;
    public PlayerClass PlayerClass
    {
        get => playerClass;
        set
        {
            if(playerClass != value)
            {
                playerClass = value;
                OnPlayerClassChanged?.Invoke();
            }
        }
    }

    #endregion

    #region Behaviour

    public void InitializeComponent()
    {
        _health = MaxHealth;
        _energy = StartEnergy;

        OnEnergyChanged?.Invoke(_energy);
        OnHealthChanged?.Invoke(_health);
    }

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