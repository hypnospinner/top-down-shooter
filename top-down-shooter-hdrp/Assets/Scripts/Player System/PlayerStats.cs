using UnityEngine;

public delegate void PlayerStateHandler();

public class PlayerStats : MonoBehaviour
{
    public PlayerStateHandler OnPlayerDead;

    [SerializeField] private float movementSpeed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float maxHealth;

    private float _health;

    public float MaxHealth { get => maxHealth;  }
    public float Health
    {
        get => _health;
        set
        {
            _health = value;
            if (_health < 0) OnPlayerDead?.Invoke();
        }
    }
    public float MovementSpeed { get => movementSpeed; }
    public float RotationSpeed { get => rotationSpeed; }
}

public enum PlayerClass
{
    Pyramid,        // fast and furious
    Sphere,         // balanced soldier
    Cube,           // solid guy
    Octahedron      // mage
}