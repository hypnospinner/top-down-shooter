public interface IDamagable
{
    void ReceiveDamage(DamageData damage);
}

public enum DamageType
{
    Instant,            // damage is dealt instantly
    ContinuousFire,     // damage is dealt contimuously (with Fire effect)
}