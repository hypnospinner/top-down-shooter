using UnityEngine;

[CreateAssetMenu(fileName = "New Damage Data", menuName = "Damage Data")]
public class DamageData : ScriptableObject
{
    [SerializeField] private DamageType damageType;
    [SerializeField] private float damage;
    public DamageType DamageType { get => damageType; }
    public float Damage { get => damage; set => damage = value; }


    public void SetDamageData(DamageData damageData)
    {
        damageType = damageData.DamageType;
        damage = damageData.Damage;
    }
}
