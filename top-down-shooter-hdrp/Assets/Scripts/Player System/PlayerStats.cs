using UnityEngine;

public delegate void PlayerStateHandler();

public class PlayerStats : MonoBehaviour
{
    #region Fields

    public PlayerStateHandler OnPlayerDead;

    [SerializeField] private float movementSpeed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float maxHealth;
    [SerializeField] private float energy;
    [SerializeField] private PlayerClass playerClass;
    [SerializeField] private GameObject playerGraphics;

    private float _health;

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