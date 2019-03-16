using UnityEngine;

public delegate void PlayerStateHandler();

public class PlayerStats : MonoBehaviour
{
    #region Fields

    public PlayerStateHandler OnPlayerDead;

    [SerializeField] private float movementSpeed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float maxHealth;
    [SerializeField] private PlayerClass playerClass;

    private float _health;
    private GameObject _playerGraphics;

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
    public float MovementSpeed
    { get => movementSpeed; }
    public float RotationSpeed
    { get => rotationSpeed; }
    public PlayerClass PlayerClass
    
{ get => playerClass; set => playerClass = value; }

    #endregion
}

public enum PlayerClass
{
    Pyramid,        // fast and furious
    Sphere,         // balanced soldier
    Cube,           // solid guy
    Octahedron      // mage
}