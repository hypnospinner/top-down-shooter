using UnityEngine;

public class DamageReceiver : MonoBehaviour, IDamagable
{
    // TODO: implement this moq
    public void ReceiveDamage(DamageData damage)
    {
        Debug.Log("Damaged!!!");
    }
}
