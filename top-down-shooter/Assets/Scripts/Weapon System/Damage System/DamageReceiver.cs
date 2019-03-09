using UnityEngine;

public class DamageReceiver : MonoBehaviour, IDamagable
{
    public bool ReceiveAID(DamageType kitType)
    {
        return false;
    }

    public void ReceiveDamage(DamageData damage)
    {
        Debug.Log("Damaged!!!");
    }
}
