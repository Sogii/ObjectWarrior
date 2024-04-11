using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatData : MonoBehaviour
{
    [SerializeField] public float _maxHealth = 100f;
    [SerializeField] public string PlayerName;
    [SerializeField] public float CurrentHealth { get; private set; }
    public float NextAbilityBuff = 0f;

    private void Start()
    {
        CurrentHealth = _maxHealth;
        BattleEventSystem.current.PlayerHealthChanged(CurrentHealth, _maxHealth);
    }
    public void TakeDamage(float damage)
    {
        CurrentHealth -= damage;
        BattleEventSystem.current.PlayerHealthChanged(CurrentHealth, _maxHealth);
        if (CurrentHealth <= 0)
        {
            Debug.Log("Player defeated");
            BattleSystem.Instance.SetState(new PlayerDefeat(BattleSystem.Instance));
        }
    }

    public void Heal(float healAmount)
    {
        CurrentHealth += healAmount;
        BattleEventSystem.current.PlayerHealthChanged(CurrentHealth, _maxHealth);
        if (CurrentHealth > _maxHealth)
        {
            CurrentHealth = _maxHealth;
        }
    }

    public void Buff()
    {

    }
}
