using UnityEngine;

public class DamageReceiver : MonoBehaviour, IDamagable
{
    #region Behaviour

    // get AID
    public bool ReceiveAID(DamageType kitType)
    {
        return false;
    }

    // get damage
    public void ReceiveDamage(DamageData damage)
    {
        Debug.Log("Damaged!!!");
    }

    #endregion
}
