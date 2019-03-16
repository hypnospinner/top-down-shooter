using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerDamageReveiver : MonoBehaviour, IDamagable
{
    #region Fields 

    // fields
    private PlayerInputController _inputController;         // reference to input controller
    private Dictionary<DamageType, int> _AIDKits;           // current amount of AID kits
    private Stack<DamageType> _AIDStack;                    // special queue for continuous damage (maybe it will be better to write custom collection)
    private PlayerStats _playerStats;

    public PlayerStats PlayerStats
    {
        get => _playerStats;
        set => _playerStats = _playerStats == null ? value : _playerStats;
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
            if (PlayerStats.Health < PlayerStats.MaxHealth && _AIDKits[DamageType.Instant] > 0)
            {
                _AIDKits[DamageType.Instant]--;
                PlayerStats.Health += PlayerStats.MaxHealth / 5;
                PlayerStats.Health = Mathf.Clamp(PlayerStats.Health, 0f, PlayerStats.MaxHealth);
                _AIDStack.Pop();
            }
        }

        if (Input.GetKeyDown(KeyCode.K))
            PlayerStats.Health -= 10f;
    }

    // called when someone attempts to damage player
    public void ReceiveDamage(DamageData damageData)
    {
        switch (damageData.DamageType)
        {
            case DamageType.Instant:

                PlayerStats.Health -= damageData.Damage;
                PlayerStats.Health = Mathf.Clamp(PlayerStats.Health, 0f, PlayerStats.MaxHealth);
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
                PlayerStats.Health += PlayerStats.MaxHealth / 5;
                PlayerStats.Health = Mathf.Clamp(PlayerStats.Health, 0f, PlayerStats.MaxHealth);
                yield break;
            }

            PlayerStats.Health -= damagePerFrame * Time.deltaTime;
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