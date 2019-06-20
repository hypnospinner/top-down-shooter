using System;
using System.Collections;
using UnityEngine;

public class TimeSpaceHoleAbility : Ability
{
    [SerializeField] private float AbilityTimer;
    [SerializeField] private LayerMask TimeAffectedLayerMask;

    private bool _isRunning;

    public override void InitializeAbility(GameObject playerGameObject)
    {
        _isRunning = false;

        AbilityTrigger = () => _inputController.AbilityButton == ButtonState.Down && _isRunning;

        var manager = playerGameObject.GetComponent<PlayerManager>();

        _inputController = manager.PlayerInputController;
    }

    public override void ExecuteAbility() => StartCoroutine(StartTimeSpaceHole());
    
    private IEnumerator StartTimeSpaceHole()
    {
        Func<float, float> timeScale = t => Mathf.Pow((AbilityTimer / t), 2);

        _isRunning = true;

        float timer = 0f;

        while(true)
        {
            if (AbilityTrigger())
            {
                Time.timeScale = timeScale(timer);

                timer += Time.unscaledDeltaTime;

                yield return null;
            } else
            {
                Time.timeScale = timeScale(timer);

                timer -= Time.unscaledDeltaTime;

                if(timer <= 0f)
                {
                    _isRunning = false;
                    yield break;
                }

                yield return null;
            }
        }
    }
}
