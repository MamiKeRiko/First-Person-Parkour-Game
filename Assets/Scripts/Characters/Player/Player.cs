using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : CharacterBase
{
    [Header("Player")]
    [SerializeField] protected int m_maxStamina;
    protected float stamina;

    bool invincibleState = false;

    public int GetMaxStamina()
    {
        return m_maxStamina;
    }

    public float GetStamina()
    {
        return stamina;
    }

    protected override void Awake()
    {
        base.Awake();
        stamina = m_maxStamina;
    }

    public void SetInvincible(bool value)
    {
        invincibleState = value;
        UIManager.Instance.ToggleInvincible(value);
    }

    private void OnEnable()
    {
        characterType = CharacterType.Friendly;
        Invoke("InitPlayer", 0.2f);
    }

    void InitPlayer()
    {
        GameManager.Instance.SetPlayer(this);
        UIManager.Instance.UpdateHealthBar((int)health);
    }

    private void OnTriggerEnter(Collider other)
    {
        TriggerObjectBase trigObj = other.GetComponent<TriggerObjectBase>();
        if (trigObj != null) trigObj.OnTriggerWithPlayer(this);
    }

    public override void Hurt(int amount)
    {
        if (!invincibleState)
        {
            base.Hurt(amount);
            UIManager.Instance.UpdateHealthBar((int)health);
        }
    }
    
    public override void Heal(int amount)
    {
        base.Heal(amount);
        if (!invincibleState)
            UIManager.Instance.UpdateHealthBar((int)health);
    }

    public void AddStamina(float amount)
    {
        stamina += amount;
        stamina = Mathf.Clamp(stamina, 0, 100);
        UIManager.Instance.UpdateStaminaBar((int)stamina);
    }

    public void SubtractStamina(float amount)
    {
        AddStamina(-amount);
    }

    public void Restore()
    {
        health = m_maxHealth;
        stamina = m_maxStamina;

        UIManager.Instance.UpdateHealthBar((int)health);
        UIManager.Instance.UpdateStaminaBar((int)stamina);
    }

    protected override void Die()
    {
        GameManager.Instance.PlayerDied();
        base.Die();
    }
}
