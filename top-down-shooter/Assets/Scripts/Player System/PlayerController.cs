using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Fields 

    [SerializeField] private float _health;
    [SerializeField] private float _movementSpeed;
    [SerializeField] private float _rotationSpeed;

    private PlayerInputController _inputController;

    public float Health { get => _health; }
    public float MovementSpeed { get => _movementSpeed; }
    public float RotationSpeed { get => _rotationSpeed; }

    #endregion

    #region Behaviour 

    private void Awake()
    {
        _inputController = GetComponent<PlayerInputController>();
        if (_inputController == null)
            Debug.LogError("Player Input Controller is not set!!!");
    }

    #endregion
}