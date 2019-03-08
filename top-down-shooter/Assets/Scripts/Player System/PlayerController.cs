using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour, IDamagable
{
    #region Fields 

    // fields
    [SerializeField] private float MaxHealth;
    [SerializeField] private float _movementSpeed;
    [SerializeField] private float _rotationSpeed;

    private float _health;
    private PlayerInputController _inputController;
    private Dictionary<DamageType, int> _AIDKits;
    private Stack<DamageType> _AIDStack;

    // properties
    public float Health { get => _health; }
    public float MovementSpeed { get => _movementSpeed; }
    public float RotationSpeed { get => _rotationSpeed; }

    #endregion

    #region Behaviour 

    private void Awake()
    {
        _AIDStack = new Stack<DamageType>();
        _AIDStack.Push(DamageType.Instant);

        _AIDKits = new Dictionary<DamageType, int>();
        _AIDKits[DamageType.Instant] = 0;
        _AIDKits[DamageType.ContinuousFire] = 0;

        _health = MaxHealth;

        _inputController = GetComponent<PlayerInputController>();
        if (_inputController == null)
            Debug.LogError("Player Input Controller is not set!!!");
    }

    private void Update()
    {
        // TODO: show context suggestion if there is possiblity for that
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

    public bool ReceiveAID(DamageType kitType)
    {
        _AIDKits[kitType]++;
        return true;
    }
    #endregion
}