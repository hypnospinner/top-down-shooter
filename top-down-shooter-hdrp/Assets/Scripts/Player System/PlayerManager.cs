using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    #region Fields

    // privates
    private PlayerInputController _playerInputController;
    private PlayerMovementController _playerMovementController;
    private WeaponController _weaponController;
    private AbilityController _abilityController;
    private KinematicCharacterController _kinematicCharacterController;
    private PlayerDamageReveiver _playerDamageReceiver;
    private PlayerStats _playerStats;
    private CameraController _cameraController;
    private Transform _cameraTransform;
    private Camera _playerCamera;

    // properties
    public PlayerInputController PlayerInputController
        { get => _playerInputController; }
    public PlayerMovementController PlayerMovementController
        { get => _playerMovementController;}
    public AbilityController AbilityController
        { get => _abilityController; }
    public KinematicCharacterController KinematicCharacterController
        { get => _kinematicCharacterController;  }
    public PlayerStats PlayerState
        { get => _playerStats; }
    public WeaponController WeaponController
        { get => _weaponController; }
    public Transform CameraTransform
        { get => _cameraTransform; }
    public Camera PlayerCamera
        { get => _playerCamera; }
    public CameraController CameraController
        { get => _cameraController; }
    public PlayerDamageReveiver PlayerDamageReceiver
        { get => _playerDamageReceiver; }

    #endregion

    #region Behaviour

    private void Awake()
    {
        // getting player stats controller
        _playerStats = GetComponent<PlayerStats>();
        
        // initializing ability controller
        _abilityController = GetComponent<AbilityController>();
        _abilityController.PlayerStats = PlayerState;

        _kinematicCharacterController = GetComponent<KinematicCharacterController>();

        // initializing player camera
        for (int i = 0; i < transform.childCount; i++)
        {
            Camera cam = transform.GetChild(i).GetComponent<Camera>();

            if (cam != null)
            {
                _cameraTransform = transform.GetChild(i);
                _cameraTransform.parent = null;

                _playerCamera = cam;
                
                // initializing camera controller
                _cameraController = _cameraTransform.GetComponent<CameraController>();
                _cameraController.Target = transform;
                _cameraController.InitializeComponent();
                break;
            }
        }

        // initializing input system
        _playerInputController = GetComponent<PlayerInputController>();
        _playerInputController.PlayerCamera = PlayerCamera;
        _playerInputController.InitializeComponent();

        // initializing movememnt controller
        _playerMovementController = GetComponent<PlayerMovementController>();
        _playerMovementController.PlayerInput = PlayerInputController;
        _playerMovementController.CharacterController = KinematicCharacterController;
        _playerMovementController.PlayerStats = PlayerState;

        // initializing weapon controller
        _weaponController = GetComponentInChildren<WeaponController>();
        _weaponController.InputController = PlayerInputController;
        _weaponController.InitializeComponent();

        // initializing damage receiver
        _playerDamageReceiver = GetComponent<PlayerDamageReveiver>();
        _playerDamageReceiver.PlayerStats = PlayerState;
        _playerDamageReceiver.InitializeComponent();
    }

    #endregion
}