using System.Collections;
using UnityEngine;

// TODO: handle null reference exception when picking AID kit
public class PlayerDamageReveiver : MonoBehaviour, IDamagable
{
    #region Fields 

    // fields
    private PlayerInputController _inputController;          // reference to input controller
    private PlayerStats _stats;                              // reference to player state values

    public PlayerStats Stats
    {
        get => _stats;
        set => _stats = _stats ?? value;
    }

    #endregion

    #region Behaviour 

    public void InitializeComponent()
    {
        return;
    }

    // controls aid kit usage
    private void Update()
    {
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
            Stats.Health -= damagePerFrame * Time.deltaTime;
            damageData.Damage -= damagePerFrame * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }

    // picking up aid
    public void RecieveHealth(float addHealth) => Stats.Health += addHealth;

    #endregion
}