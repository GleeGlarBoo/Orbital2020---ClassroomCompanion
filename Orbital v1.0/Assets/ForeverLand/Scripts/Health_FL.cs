using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health_FL : MonoBehaviour
{
    public int m_MaxHealth = 3;
    public int m_CurrentHealth = 3;

    public AudioSource HitSound;

    public void TakeDamage()
    {
        HitSound.Play();
        m_CurrentHealth -= 1;
    }

    public bool IsDead()
    {
        return m_CurrentHealth <= 0;
    }

    // This will be convenient for setting UI elements in the future
    public int GetHealth()
    {
        return m_CurrentHealth;
    }
}
