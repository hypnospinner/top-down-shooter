using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour, IDamagable
{
    #region Fields 

    // fields
    [SerializeField] private float MaxHealth;               // top limit for health
    [SerializeField] private float _movementSpeed;          // how quickly player moves
    [SerializeField] private float _rotationSpeed;          // how quickly player rotates
                                                            
    private float _health;                                  // actual health value
    private PlayerInputController _inputController;         // reference to input controller
    private Dictionary<DamageType, int> _AIDKits;           // current amount of AID kits
    private Stack<DamageType> _AIDStack;                    // special queue for continuous damage (maybe it will be better to write custom collection)

    // properties
    public float Health { get => _health; }
    public float MovementSpeed { get => _movementSpeed; }
    public float RotationSpeed { get => _rotationSpeed; }

    #endregion

    #region Behaviour 

    // initializing
    private void Awake()
    {
        _AIDStack = new Stack<DamageType>();
        _AIDStack.Push(DamageType.Instant);

        // seems to be not very good
        _AIDKits = new Dictionary<DamageType, int>();
        _AIDKits[DamageType.Instant] = 0;
        _AIDKits[DamageType.ContinuousFire] = 0;

        _health = MaxHealth;

        _inputController = GetComponent<PlayerInputController>();
        if (_inputController == null)
            Debug.LogError("Player Input Controller is not set!!!");
    }

    // controls aid kit usage
    private void Update()
    {
        if (_AIDStack.Peek() == DamageType.Instant)
        {
            if (_health < MaxHealth && _AIDKits[DamageType.Instant] > 0)
            {
                _AIDKits[DamageType.Instant]--;
                _health += MaxHealth / 5;
                _health = Mathf.Clamp(_health, 0f, MaxHealth);
                _AIDStack.Pop();
            }
        }

        if (Input.GetKeyDown(KeyCode.K))
            _health -= 2;
    }

    // called when someone attempts to damage player
    public void ReceiveDamage(DamageData damageData)
    {
        switch (damageData.DamageType)
        {
            case DamageType.Instant:
                _health -= damageData.Damage;
                _health = Mathf.Clamp(_health, 0f, MaxHealth);
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
                _health += MaxHealth / 5;
                _health = Mathf.Clamp(_health, 0f, MaxHealth);
                yield break;
            }

            _health -= damagePerFrame * Time.deltaTime;
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