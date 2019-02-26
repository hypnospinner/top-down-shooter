﻿using UnityEngine;

class WeaponController : MonoBehaviour
{
    private PlayerInputController _inputController;
    private GameObject[] _weapons;
    private int _activeWeaponIndex;
    private int _weaponCount;

    private void Awake()
    {
        _activeWeaponIndex = -1;

        _weapons = new GameObject[2];

        // initializing weapons
        _inputController = GetComponentInParent<PlayerInputController>();
        if (_inputController == null)
            Debug.LogError("Player Input Conroller is not set!!!");

        _weaponCount = transform.childCount;

        for(int i = 0; i < _weaponCount; i++)
        {
            _weapons[i] = transform.GetChild(i).gameObject;

            if(i == 0)
                _activeWeaponIndex = 0;

            if (i == 1)
                _weapons[i].SetActive(false);
        }
    }

    private void Update()
    {
        if (_inputController.MouseWheel == MouseWheelState.ScrollForward||
            _inputController.MouseWheel == MouseWheelState.ScrollBackward)
            SwitchWeapons();
    }

    public WeaponData PickWeapon(WeaponData weaponData)
    {
        switch(_weaponCount)
        {
            case 0:
                _weapons[0] = 
                    Instantiate(weaponData.WeaponPrefab, 
                    transform.position, 
                    transform.rotation, 
                    transform) as GameObject;
                Weapon weapon = _weapons[0].GetComponent<Weapon>();
                weapon.WeaponData = ScriptableObject.CreateInstance<WeaponData>();
                weapon.WeaponData.SetWeaponData(weaponData);
                _weaponCount = 1;
                _activeWeaponIndex = 0;
                return null;

            case 1:
                _weapons[1] = 
                    Instantiate(weaponData.WeaponPrefab, 
                    transform.position, 
                    transform.rotation, 
                    transform) as GameObject;
                Weapon secondWeapon = _weapons[1].GetComponent<Weapon>();
                secondWeapon.WeaponData = ScriptableObject.CreateInstance<WeaponData>();
                secondWeapon.WeaponData.SetWeaponData(weaponData);
                _weaponCount++;
                SwitchWeapons();
                return null;

            case 2:
                WeaponData dropedWeaponData =
                    ScriptableObject.CreateInstance<WeaponData>();

                dropedWeaponData.SetWeaponData(_weapons[_activeWeaponIndex].GetComponent<Weapon>().WeaponData);

                Destroy(_weapons[_activeWeaponIndex]);

                _weapons[_activeWeaponIndex] = Instantiate(weaponData.WeaponPrefab, transform, false) as GameObject;

                var activeWeaponData = _weapons[_activeWeaponIndex].GetComponent<Weapon>().WeaponData = 
                    ScriptableObject.CreateInstance<WeaponData>();
                activeWeaponData.SetWeaponData(weaponData);
                return dropedWeaponData;

            default: break;
        }

        // TODO: This return can cause problems. Better create enum for _weaponCount int order to have a full match in switch
        return null;
    }

    private void SwitchWeapons()
    {
        if (_weaponCount < 2)
            return;

        int newIndex = _activeWeaponIndex == 0 ? 1 : 0;

        _weapons[_activeWeaponIndex].SetActive(false);
        _weapons[newIndex].SetActive(true);

        _activeWeaponIndex = newIndex;
    }
}

