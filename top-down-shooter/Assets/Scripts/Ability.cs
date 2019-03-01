﻿using UnityEngine;

public abstract class Ability : MonoBehaviour
{
    #region Fields 

    [SerializeField] private float staminaConsumption;      // how much stamina ability needs to be executed

    protected InputHandler AbilityTrigger;

    public float StaminaConsumption { get => staminaConsumption; }

    #endregion

    #region Behaviour

    private void Update()
    {
        if (AbilityTrigger != null ? AbilityTrigger() : false)
            ExecuteAbility();
    }

    // gets references from other systems
    public abstract void InitializeAbility(GameObject playerGameObject);

    // what happens when player attempts to use ability
    public abstract void ExecuteAbility();

    #endregion
}
