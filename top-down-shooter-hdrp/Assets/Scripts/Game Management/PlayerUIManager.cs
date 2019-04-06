using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

class PlayerUIManager : MonoBehaviour
{
    #region Fields

    // private fields
    [SerializeField] private GameObject PlayerUICanvas;
    [SerializeField] private GameObject EndgameUICanvas;
    [SerializeField] private Image _healthBar;
    [SerializeField] private TMP_Text _energyBar;
    [SerializeField] private Image _weaponIcon;
    [SerializeField] private Image _abilityIcon;

    #endregion

    #region Behaviour

    private void Awake()
    {
        PlayerUICanvas.SetActive(true);
        EndgameUICanvas.SetActive(false);
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
    public void InitializeEngameScreen()
    {
        PlayerUICanvas.SetActive(false);
        EndgameUICanvas.SetActive(true);
    }
    public void ReloadScene () => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    public void GoBackToMenu() => SceneManager.LoadScene(0);

    #endregion
}
