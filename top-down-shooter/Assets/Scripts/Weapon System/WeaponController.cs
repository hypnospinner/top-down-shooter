using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    private PlayerInputController _inputController;
    private GameObject[] _weapons;
    private GameObject _activeWeapon;
    private int _activeWeaponIndex;
    private int _weaponsCount;

    private void Awake()
    {
        _inputController = GetComponentInParent<PlayerInputController>();
        if (_inputController == null)
            Debug.LogError("Player Input Controller is not set!!!");

        _weaponsCount = transform.childCount;
        _weapons = new GameObject[2];

        for (int i = 0; i < transform.childCount; i++)
        {
            _activeWeaponIndex = 0;
            _weapons[i] =  transform.GetChild(i).gameObject;
            if (i == 1)
            {
                _activeWeapon = _weapons[i];
                _weapons[i].SetActive(true);
            }
            else
                _weapons[i].SetActive(false);
        }

        _activeWeaponIndex = 0;
    }

    private void Update()
    {
        if (_inputController.MouseWheel == MouseWheelState.Down ||
            _inputController.MouseWheel == MouseWheelState.Up)
            ChangeWeapon();
    }

    public bool PickNewWeapon(GameObject newWeapon, InteractiveWeapon interactiveWeapon)
    {
        // TODO: support for smart selection in case of trying to equip weapon player alredy has

        switch(_weaponsCount)
        {
            case 0:
                _weapons[0] = newWeapon;
                _activeWeaponIndex = 0;
                _activeWeapon = _weapons[0];
                _activeWeapon.SetActive(true);
                _activeWeapon.transform.SetParent(transform);
                _activeWeapon.transform.localPosition = Vector3.zero;
                _activeWeapon.transform.localRotation = Quaternion.identity;
                _activeWeapon.transform.localScale = new Vector3(1f, 1f, 1f);
                _activeWeapon.GetComponent<GunBase>().InputController = _inputController;
                return true;

            case 1:
                _weapons[1] = newWeapon;
                _weapons[1].SetActive(false);
                _weapons[1].transform.SetParent(transform);
                _weapons[1].transform.localPosition = Vector3.zero;
                _weapons[1].transform.localRotation = Quaternion.identity;
                _weapons[1].transform.localScale = new Vector3(1f, 1f, 1f);
                _weapons[1].GetComponent<GunBase>().InputController = _inputController;
                ChangeWeapon();
                return true;

            case 2:
                _activeWeapon.GetComponent<GunBase>().InputController = null;
                interactiveWeapon.SetWeapon(_weapons[_activeWeaponIndex]);
                _weapons[_activeWeaponIndex] = newWeapon;
                _activeWeapon = _weapons[_activeWeaponIndex];
                _activeWeapon.SetActive(true);
                _activeWeapon.transform.SetParent(transform);
                _activeWeapon.transform.localPosition = Vector3.zero;
                _activeWeapon.transform.localRotation = Quaternion.identity;
                _activeWeapon.transform.localScale = new Vector3(1f, 1f, 1f);
                _activeWeapon.GetComponent<GunBase>().InputController = _inputController;
                return false;

            default:
                return false;
        }

    }

    private void ChangeWeapon()
    {
        if(_weaponsCount < 2)
            return;

        int newActiveWeaponIndex = _activeWeaponIndex == 0 ? 1 : 0;
        _activeWeapon.SetActive(false);
        _activeWeapon = _weapons[newActiveWeaponIndex];
        _activeWeapon.SetActive(true);
        _activeWeaponIndex = newActiveWeaponIndex;
    }
}