using System;
using UnityEngine;

public class BattleEventSystem : MonoBehaviour
{
    public static BattleEventSystem current;

    private void Awake()
    {
        if (current == null)
        {
            current = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public event Action<float, float> OnEnemyHealthChanged;

    public void EnemyHealthChanged(float currentHealth, float maxHealth)
    {
        if (OnEnemyHealthChanged != null)
        {
            OnEnemyHealthChanged(currentHealth, maxHealth);
        }
    }

    public event Action<float, float> OnPlayerHealthChanged;
    public void PlayerHealthChanged(float currentHealth, float maxHealth)
    {
        if (OnPlayerHealthChanged != null)
        {
            OnPlayerHealthChanged(currentHealth, maxHealth);
        }
    }
}
