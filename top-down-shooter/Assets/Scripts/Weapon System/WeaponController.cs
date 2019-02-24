using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    private List<GameObject> _weapons;
    private int _weaponSlots;

    private void Awake()
    {
        _weaponSlots = 1;

        _weapons = new List<GameObject>();
        for (int i = 0; i < transform.childCount; i++)
            _weapons.Add(transform.GetChild(i).gameObject);
    }

    public void AddWeapon(GameObject weapon)
    {
        if (_weaponSlots < _weapons.Count)
        {
            _weapons.Add(weapon);
            weapon.transform.SetParent(transform);
            weapon.transform.localPosition = new Vector3(0, 0, 0);
        }
    }
}
