using UnityEngine;

public class InteractiveWeapon : InteractiveObject
{
    [SerializeField] private GameObject StoredWeapon;

    public override void Interact(GameObject interactor)
    {
        WeaponController weaponController = interactor.GetComponentInChildren<WeaponController>();
        if (weaponController.PickNewWeapon(StoredWeapon, this))
        {
            _interactor.InteractedWithDestruction(this);
            Destroy(gameObject);
        }
    }

    public void SetWeapon(GameObject newWeapon)
    {
        StoredWeapon = newWeapon;
        StoredWeapon.transform.position = transform.position;
        StoredWeapon.SetActive(false);
    }
}
