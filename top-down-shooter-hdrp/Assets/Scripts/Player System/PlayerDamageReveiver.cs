using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: handle null reference exception when picking AID kit
public class PlayerDamageReveiver : MonoBehaviour, IDamagable
{
    #region Fields 

    // fields
    private PlayerInputController _inputController;         // reference to input controller
    private Dictionary<DamageType, int> _AIDKits;           // current amount of AID kits
    private Stack<DamageType> _AIDStack;                    // special queue for continuous damage (maybe it will be better to write custom collection)
    private PlayerStats _stats;                              // reference to player state values

    public PlayerStats Stats
    {
        get => _stats;
        set => _stats = _stats == null ? value : _stats;
    }

    #endregion

    #region Behaviour 

    public void InitializeComponent()
    {
        _AIDStack = new Stack<DamageType>();
        _AIDStack.Push(DamageType.Instant);

        // seems to be not very good
        _AIDKits = new Dictionary<DamageType, int>();
        _AIDKits[DamageType.Instant] = 0;
        _AIDKits[DamageType.ContinuousFire] = 0;
    }

    // controls aid kit usage
    private void Update()
    {
        if (_AIDStack.Peek() == DamageType.Instant)
        {
            if (Stats.Health < Stats.MaxHealth && _AIDKits[DamageType.Instant] > 0)
            {
                _AIDKits[DamageType.Instant]--;
                Stats.Health += Stats.MaxHealth / 5;
                Stats.Health = Mathf.Clamp(Stats.Health, 0f, Stats.MaxHealth);
                _AIDStack.Pop();
            }
        }

        if (Input.GetKeyDown(KeyCode.K))
            Stats.Health -= 10f;
    }

    // called when someone attempts to damage player
    public void ReceiveDamage(DamageData damageData)
    {
        switch (damageData.DamageType)
        {
            case DamageType.Instant:

                Stats.Health -= damageData.Damage;
                Stats.Health = Mathf.Clamp(Stats.Health, 0f, Stats.MaxHealth);
                break;
            default:
                if (!_AIDStack.Contains(damageData.DamageType))
                    _AIDStack.Push(damageData.DamageType);
                StartCoroutine(DealContinuousDamage(damageData));
                break;
        }
    }

    // calculating continuous damage
    private IEnumerator DealContinuousDamage(DamageData damageData)
    {
        float damagePerFrame = damageData.Damage;

        while (damageData.Damage > 0)
        {
            if(_AIDStack.Peek() == damageData.DamageType && 
                _AIDKits[damageData.DamageType] > 0 && 
                _inputController.IsContextInteracting)
            {
                _AIDStack.Pop();
                _AIDKits[damageData.DamageType]--;
                Stats.Health += Stats.MaxHealth / 5;
                Stats.Health = Mathf.Clamp(Stats.Health, 0f, Stats.MaxHealth);
                yield break;
            }

            Stats.Health -= damagePerFrame * Time.deltaTime;
            damageData.Damage -= damagePerFrame * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        List<DamageType> temp = new List<DamageType>(_AIDStack.ToArray());
        temp.Remove(damageData.DamageType);
        _AIDStack = new Stack<DamageType>(temp);
    }

    // picking up aid
    public bool ReceiveAID(DamageType kitType)
    {
        _AIDKits[kitType]++;
        return true;
    }

    #endregion
}