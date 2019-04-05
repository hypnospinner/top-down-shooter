using UnityEngine;
using UnityEngine.UI;
using TMPro;

class PlayerUIManager : MonoBehaviour
{
    #region Fields

    // private fields
    [SerializeField] private Image _healthBar;
    [SerializeField] private TMP_Text _energyBar;
    [SerializeField] private Image _weaponIcon;
    [SerializeField] private Image _abilityIcon;

    #endregion

    #region Behaviour

    private void Awake()
    {
        _healthBar.type = Image.Type.Filled;
        _healthBar.fillMethod = Image.FillMethod.Horizontal;
    }

    public void SetWeaponIcon(WeaponData source) {
        Debug.Log("Weapon Icon Changed");
        _weaponIcon.sprite = source.WeaponIcon;
    }
    public void SetAbilityIcon(Sprite source)
    {
        Debug.Log("Abilty Icon Changed");
        _abilityIcon.sprite = source;
    }

    public void UpdateHealth(float newHealthValue)
    {
        _healthBar.fillAmount = newHealthValue / 100f;
    }
    public void UpdateEnergy(float newEnergyValue)
    {
        Debug.Log(newEnergyValue);
        _energyBar.text = newEnergyValue.ToString();
    }
    #endregion
}
