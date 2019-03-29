using UnityEngine;

public class WeaponController : MonoBehaviour
{
    #region Fields 

    // fields
    private PlayerInputController _inputController;     // reference to input controller
    private PlayerManager _manager;                     // reference to player manager
    private Weapon weapon;                              // reference to current weapon
    private GameObject weaponGameObject;                // reference to current weapon game object

    // properties
    public PlayerInputController InputController
    {
        get => _inputController;
        set => _inputController = _inputController == null ? value : _inputController;
    }
    public PlayerManager Manager
    {
        get => _manager;
        set => _manager = _manager == null ? value : _manager;
    }

    #endregion

    #region Behaviour

    // component initialization called by Manager
    public void InitializeComponent() { return; }

    // getting new weapon
    public WeaponData PickWeapon(WeaponData weaponData)
    {
        if (weaponGameObject == null)
        {
            weaponGameObject =
                Instantiate(weaponData.WeaponPrefab,
                transform.position,
                transform.rotation,
                transform) as GameObject;

            weapon = weaponGameObject.GetComponent<Weapon>();
            weapon.WeaponData = weaponData.Clone() as WeaponData;
            weapon.InputController = _inputController;
            weapon.Manager = _manager;
            weapon.InitializeWeapon();

            return null;
        }
        else
        {
            if (weaponData.Equals(weapon.WeaponData))
                return weaponData;

            Destroy(weaponGameObject);

            weaponGameObject =
                Instantiate(weaponData.WeaponPrefab,
                    transform.position,
                    transform.rotation,
                    transform) as GameObject;

            var dropedWeaponData = weapon.WeaponData;

            weapon = weaponGameObject.GetComponent<Weapon>();
            weapon.WeaponData = weaponData.Clone() as WeaponData;
            weapon.Manager = _manager;
            weapon.InputController = _inputController;
            weapon.InitializeWeapon();

            return dropedWeaponData;
        }
    }

    #endregion
}