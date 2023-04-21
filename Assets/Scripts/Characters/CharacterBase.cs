using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBase : MonoBehaviour
{
    [SerializeField] protected int m_maxHealth;

    protected float health;

    public CharacterType characterType;

    public int GetMaxHealth()
    {
        return m_maxHealth;
    }

    public float GetHealth()
    {
        return health;
    }

    protected virtual void Awake()
    {
        health = m_maxHealth;
    }

    public virtual void Hurt(int amount)
    {
        Debug.Log(amount);

        health -= amount;
        if (health <= 0) Die();
    }
    public virtual void Heal(int amount)
    {
        health += amount;
        health = Mathf.Clamp(health, 0, 100);
    }

    protected virtual void Die()
    {
        health = 0;
    }
}
