using UnityEngine;

[CreateAssetMenu(fileName = "New Damage Data", menuName = "Damage Data")]
public class DamageData : ScriptableObject
{
    #region Fields

    // fields
    [SerializeField] private DamageType damageType; // type of damage
    [SerializeField] private float damage;          // damage value

    // properties
    public DamageType DamageType { get => damageType; }
    public float Damage { get => damage; set => damage = value; }

    #endregion

    #region Behaviour

    // initializing damage data
    public void SetDamageData(DamageData damageData)
    {
        damageType = damageData.DamageType;
        damage = damageData.Damage;
    }

    #endregion
}
