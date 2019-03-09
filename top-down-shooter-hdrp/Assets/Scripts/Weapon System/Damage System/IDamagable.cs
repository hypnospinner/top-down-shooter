public interface IDamagable
{
    void ReceiveDamage(DamageData damage);

    bool ReceiveAID(DamageType ktiType);
}

public enum DamageType
{
    Instant,            // damage is dealt instantly
    ContinuousFire,     // damage is dealt contimuously (with Fire effect)
}