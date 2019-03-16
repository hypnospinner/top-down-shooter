using System.Collections;
using UnityEngine;

public class HyperDashAbility : Ability
{
    #region Fields

    [SerializeField][Range(0f, .5f)] private float DashTime;        // time that player needs to make a dash
    [SerializeField][Range(0f, 5f)] private float DashDistance;     // max distance that player will cover
                                                                    
    private PlayerInputController _inputController;                 // reference to input controller component
    private KinematicCharacterController _characterController;      // reference to KCC

    private float speed;                                            // stores speed (in order no to recalculate)

    #endregion

    #region Behaviour

    // we gurantee that this method wil be called before using ability
    public override void InitializeAbility(GameObject playerGameObject)
    {
        AbilityTrigger = () => _inputController.AbilityButton == ButtonState.Down;

        var manager = playerGameObject.GetComponent<PlayerManager>();

        _inputController = manager.PlayerInputController;

        _characterController = manager.KinematicCharacterController;

        speed = DashDistance / DashTime;
    }

    // actual execution incapsulation
    public override void ExecuteAbility()
    {
        StartCoroutine(DashForward());
    }

    // handles movement with KCC (should be enhanced)
    private IEnumerator DashForward()
    {
        _inputController.Blocked = true;
        float timer = DashTime;

        Vector3 direction = _inputController.PointerPosition - _inputController.transform.position;

        while (timer > 0f)
        {
            _characterController.MovePlayer(direction, speed);
            timer -= Time.deltaTime;
            yield return null;
        }

        _inputController.Blocked = false;

        yield break;
    }

    #endregion
}
