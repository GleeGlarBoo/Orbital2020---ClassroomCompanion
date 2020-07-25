using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int m_MaxHealth = 100;
    public int m_CurrentHealth = 100;

    public void TakeDamage(int damageAmt)
    {
        m_CurrentHealth -= damageAmt;
    }

    public bool IsDead()
    {
        return m_CurrentHealth <= 0;
    }

    // This will be convenient for setting UI elements in the future
    public float GetPercentageHealth()
    {
        return (float)m_CurrentHealth / m_MaxHealth;
    }
}
