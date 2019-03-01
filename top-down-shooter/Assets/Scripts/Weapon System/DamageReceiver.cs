using UnityEngine;

public class DamageReceiver : MonoBehaviour, IDamagable
{
    public bool ReceiveAID(DamageType kitType)
    {
        return false;
    }

    // TODO: implement this moq
    public void ReceiveDamage(DamageData damage)
    {
        Debug.Log("Damaged!!!");
    }
}
