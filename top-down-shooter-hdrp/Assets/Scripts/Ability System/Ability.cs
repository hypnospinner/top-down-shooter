﻿using UnityEngine;
using UnityEngine.UI;

public abstract class Ability : MonoBehaviour
{
    #region Fields 

    [SerializeField] private float staminaConsumption;      // how much stamina ability needs to be executed
    [SerializeField] private PlayerClass playerClass;       // player class that can pick up this ability
    [SerializeField] private Sprite abilityIcon;            // what is displayed when player picked up ability

    protected InputHandler AbilityTrigger;                  // describes when we should call ability Execution
    protected PlayerInputController _inputController;       // reference to input controller component

    public PlayerClass PlayerClass => playerClass;
    public Sprite AbilityIcon => abilityIcon;

    #endregion

    #region Behaviour

    // executing ability if there is any trigger for it
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
