using UnityEngine;

[CreateAssetMenu(fileName = "New Shogun Weapon Data", menuName = "WeaponData/Shotgun")]
public class ShotgunWeaponData : WeaponData
{
    #region Fields

    [SerializeField] private int fireDensity;                           // how many bullets should be spawned when player shots
    [SerializeField] private float positionScatter;                     // how bullet should be moved when shooting
    [SerializeField] private float rotationScatter;                     // max angle that bullet can rotate

    public int FireDensity { get => fireDensity; }
    public float PositionScatter { get => positionScatter; }
    public float RotationScatter { get => rotationScatter; }

    #endregion

    #region Behaviour

    public override object Clone()
    {
        ShotgunWeaponData clone = CreateInstance<ShotgunWeaponData>();
        clone.fireRate = FireRate;
        clone.weaponPrefab = WeaponPrefab;
        clone.projectilePrefab = ProjectilePrefab;
        clone.energyConsumption = EnergyConsumption;
        clone.weaponIcon = WeaponIcon;
        clone.fireDensity = FireDensity;
        clone.positionScatter = PositionScatter;
        clone.rotationScatter = RotationScatter;

        return clone;
    }

    public override bool Equals(object other)
    {
        if (other is ShotgunWeaponData)
        {
            var weaponData = other as ShotgunWeaponData;

            return weaponData.FireRate.Equals(FireRate) &&
                weaponData.EnergyConsumption.Equals(EnergyConsumption) &&
                weaponData.WeaponPrefab.Equals(WeaponPrefab) &&
                weaponData.ProjectilePrefab.Equals(ProjectilePrefab) &&
                weaponData.WeaponIcon.Equals(WeaponIcon) &&
                weaponData.FireDensity.Equals(FireDensity) &&
                weaponData.PositionScatter.Equals(PositionScatter) &&
                weaponData.RotationScatter.Equals(RotationScatter);
        }
        else return false;
    }

    #endregion
}
