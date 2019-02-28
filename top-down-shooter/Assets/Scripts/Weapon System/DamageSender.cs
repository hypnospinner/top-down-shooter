using UnityEngine;

class DamageSender : MonoBehaviour
{
    #region Fields

    [SerializeField] private DamageData DamageDataPrefab;

    private DamageData _damageData;

    #endregion

    #region Behaviour

    private void Awake()
    {
        _damageData = ScriptableObject.CreateInstance<DamageData>();
        _damageData.SetDamageData(DamageDataPrefab);
    }

    public void SendDamageTo(GameObject damaged)
    {
        IDamagable damagable = damaged.GetComponentInChildren<IDamagable>(true);
        if (damagable != null)
            damagable.ReceiveDamage(_damageData);
    }

    #endregion
}

