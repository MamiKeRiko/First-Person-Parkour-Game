using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : TemporalSingleton<UIManager>
{
    public Text currentAmmoText;
    public Text reservedAmmoText;
    public Image healthSlider;
    public Image staminaSlider;

    Player p;
    float barWidth;
    int pMaxHealth;
    int pMaxStamina;

    float pHealth;
    float pStamina;

    Color healthBarColor;
    Color staminaBarColor;

    public override void Awake()
    {
        base.Awake();
        StartCoroutine("CallGameManager");
    }

    private void Start()
    {
        healthSlider.fillAmount = 1;
        staminaSlider.fillAmount = 1;
        healthBarColor = healthSlider.color;
        staminaBarColor = staminaSlider.color;
    }

    IEnumerator CallGameManager()
    {
        while (GameManager.Instance.GetPlayer() == null) yield return new WaitForEndOfFrame();
        p = GameManager.Instance.GetPlayer();
        pMaxHealth = p.GetMaxHealth();
        pMaxStamina = p.GetMaxStamina();

        yield break;
    }

    public void ChangeAmmoText(int currentAmmo, int reservedAmmo)
    {
        currentAmmoText.text = currentAmmo.ToString();
        reservedAmmoText.text = reservedAmmo.ToString();
    }

    public void UpdateHealthBar(float newValue)
    {
        if (pMaxHealth != 0)
        healthSlider.fillAmount = newValue / pMaxHealth;
        pHealth = newValue;
    }

    public void UpdateStaminaBar(float newValue)
    {
        if (pMaxStamina != 0)
        staminaSlider.fillAmount = newValue / pMaxStamina;
        pStamina = newValue;
    }

    public void ToggleInvincible(bool value)
    {
        if (value)
        {
            healthSlider.color = Color.cyan;
            healthSlider.fillAmount = 1;
        }
        else
        {
            healthSlider.color = healthBarColor;
            healthSlider.fillAmount = pHealth / pMaxHealth;
        }
    }

    public void ToggleCanRun(bool value)
    {
        if (value)
        {
            staminaSlider.color = staminaBarColor;
        }
        else
        {
            staminaSlider.color = Color.red;
        }
    }
}
