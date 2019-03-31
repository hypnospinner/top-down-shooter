using UnityEngine;

public class DamageSender : MonoBehaviour
{
    #region Fields

    [SerializeField] private DamageData DamageDataPrefab;   // damage data prefab
                                                            
    private DamageData _damageData;                         // actual damage data

    #endregion

    #region Behaviour

    // initializing
    private void Awake()
    {
        _damageData = ScriptableObject.CreateInstance<DamageData>();
        _damageData.SetDamageData(DamageDataPrefab);
    }

    // attempting to damage something
    public void SendDamageTo(GameObject damaged)
    {
        IDamagable damagable = damaged.GetComponentInChildren<IDamagable>(true);
        if (damagable != null)
            damagable.ReceiveDamage(_damageData);
    }

    #endregion
}

