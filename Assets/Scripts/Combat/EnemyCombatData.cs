using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyCombatData
{
    public float MaxHealth;
    public string EnemyName;
    public int Damage;
    public float CurrentHealth { get; private set; } = 1;
    public Elements EnemyElement;
    public Sprite EnemySprite;

    public EnemyCombatData(float inputMaxHealth)
    {
        MaxHealth = inputMaxHealth;
        // Debug.Log(MaxHealth + " " + EnemyName + " " + Damage + " " + CurrentHealth + " " + EnemyElement + " " + EnemySprite);
        CurrentHealth = MaxHealth;
        Debug.Log("currentHealth=" + CurrentHealth);
        // Ensure BattleEventSystem.current is not null before subscribing
        if (BattleEventSystem.current != null)
        {
            BattleEventSystem.current.EnemyHealthChanged(CurrentHealth, MaxHealth);
        }
    }

    public void TakeDamage(float damage)
    {
        Debug.Log("Enemy took " + damage + " damage");
        CurrentHealth -= damage;
        // Ensure BattleEventSystem.current is not null before publishing
        if (BattleEventSystem.current != null)
        {
            BattleEventSystem.current.EnemyHealthChanged(CurrentHealth, MaxHealth);
        }
        if (CurrentHealth <= 0)
        {
            Debug.Log("Enemy defeated");
            BattleSystem.Instance.SetState(new PlayerVictory(BattleSystem.Instance));
        }
    }
}
