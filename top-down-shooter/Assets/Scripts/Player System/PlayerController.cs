using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDamagable
{
    #region Fields 

    // fields
    [SerializeField] private float _health;
    [SerializeField] private float _movementSpeed;
    [SerializeField] private float _rotationSpeed;

    private PlayerInputController _inputController;
    private List<DamageData> _continuousDamagers;

    // properties
    public float Health { get => _health; }
    public float MovementSpeed { get => _movementSpeed; }
    public float RotationSpeed { get => _rotationSpeed; }

    #endregion

    #region Behaviour 

    private void Awake()
    {
        _inputController = GetComponent<PlayerInputController>();
        if (_inputController == null)
            Debug.LogError("Player Input Controller is not set!!!");

        _continuousDamagers = new List<DamageData>();
    }

    public void ReceiveDamage(DamageData damageData)
    {
        switch (damageData.DamageType)
        {
            case DamageType.Instant:
                _health -= damageData.Damage;
                break;
            case DamageType.Continuous:
                StartCoroutine(DealContinuousDamage(damageData));
                break;
        }
    }

    private IEnumerator DealContinuousDamage(DamageData damageData)
    {
        float damagePerFrame = damageData.Damage;

        while (damageData.Damage > 0)
        {
            _health -= damagePerFrame * Time.deltaTime;
            damageData.Damage -= damagePerFrame * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }

    #endregion
}

public interface IDamagable
{
    void ReceiveDamage(DamageData damage);
}

[CreateAssetMenu(fileName = "New Damage Data", menuName = "Damage Data")]
public class DamageData : ScriptableObject
{
    [SerializeField] private DamageType damageType;
    [SerializeField] private ContinuousDamageType continousDamageType;
    [SerializeField] private float damage;

    public DamageType DamageType { get => damageType; }
    public float Damage { get => damage; set => damage = damageType == DamageType.Continuous ? value : damage; }
    public ContinuousDamageType ContinousDamageType { get => continousDamageType; }

    public void SetDamageData(DamageData damageData)
    {
        damageType = damageData.DamageType;
        continousDamageType = damageData.ContinousDamageType;
        damage = damageData.Damage;
    }
}

public enum DamageType
{
    Instant,        // damage is dealt instantly
    Continuous      // damage is dealt  contimuously (over time)
}

public enum ContinuousDamageType
{
    Fire,           // is healed by Anti-Fire AID kit
    Freeze          // is healed by Anti-Freeze AID kit
} 