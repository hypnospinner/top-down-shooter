using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    #region Fields

    // privates
    private PlayerInputController _playerInputController;                   // encapsulates player inp
    private PlayerMovementController _playerMovementController;             // handles movement
    private WeaponController _weaponController;                             // handles weapon
    private AbilityController _abilityController;                           // handles ability 
    private KinematicCharacterController _kinematicCharacterController;     // movememnt physics calculations
    private PlayerDamageReveiver _playerDamageReceiver;                     // encapsulates damage receiving
    private PlayerStats _stats;                                             // handles player stats
    private CameraController _cameraController;                             // controller for camera (stalks player)
    private Transform _cameraTransform;                                     // camera game object transform
    private Camera _playerCamera;                                           // camera comoponent that look on player
                                                                            
    // properties
    public PlayerInputController PlayerInputController
        { get => _playerInputController; }
    public PlayerMovementController PlayerMovementController
        { get => _playerMovementController;}
    public AbilityController AbilityController
        { get => _abilityController; }
    public KinematicCharacterController KinematicCharacterController
        { get => _kinematicCharacterController;  }
    public PlayerStats Stats
        { get => _stats; }
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

    // initializes player components
    private void Awake()
    {
        // getting player stats controller
        _stats = GetComponent<PlayerStats>();
        _stats.InitializeComponent();

        // initializing ability controller
        _abilityController = GetComponent<AbilityController>();
        _abilityController.PlayerStats = Stats;

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
        _playerMovementController.PlayerStats = Stats;

        // initializing weapon controller
        _weaponController = GetComponentInChildren<WeaponController>();
        _weaponController.InputController = PlayerInputController;
        _weaponController.Manager = this;
        _weaponController.InitializeComponent();

        // initializing damage receiver
        _playerDamageReceiver = GetComponent<PlayerDamageReveiver>();
        _playerDamageReceiver.Stats = Stats;
        _playerDamageReceiver.InitializeComponent();
    }

    #endregion
}