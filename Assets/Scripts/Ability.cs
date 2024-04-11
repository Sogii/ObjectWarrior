public abstract class Ability
{
    public string AbilityDescription;
    public string UpgradeReason;
    public abstract void Execute(PlayerCombatData player, EnemyCombatData enemy);
    public abstract string GetDescription();
    public abstract void UpgradeAbility(int upgradeValue);

}

public class DamageAbility : Ability
{
    public float BaseDamageAmount;
    public Elements Element;

    public override void Execute(PlayerCombatData player, EnemyCombatData enemy)
    {
        float damageAmount = BaseDamageAmount + player.NextAbilityBuff;
        enemy.TakeDamage(damageAmount);
        player.NextAbilityBuff = 0;
    }

    public override string GetDescription()
    {
        string description = $"This item does {BaseDamageAmount} {Element} damage.";
        // if (UpgradeReason != null)
        // {
        //     description += UpgradeReason;
        // }

        return description;
    }

    public override void UpgradeAbility(int upgradeValue)
    {
        BaseDamageAmount += upgradeValue;
    }
}
public class HealAbility : Ability
{
    public float BaseHealAmount;
    public Elements Element;


    public override void Execute(PlayerCombatData player, EnemyCombatData enemy)
    {

        float healAmount = BaseHealAmount + player.NextAbilityBuff;
        player.Heal(healAmount);
        player.NextAbilityBuff = 0;
    }

    public override string GetDescription()
    {
        string description;
        description = $"This item heals {BaseHealAmount} health.";
        if (UpgradeReason != null)
        {
            description += UpgradeReason;
        }

        return description;
    }

    public override void UpgradeAbility(int upgradeValue)
    {
        BaseHealAmount += upgradeValue;
    }
}

public class BuffAbility : Ability
{
    public float BaseBuffAmount;
    public Elements Element;

    public override void Execute(PlayerCombatData player, EnemyCombatData enemy)
    {
        player.NextAbilityBuff += BaseBuffAmount;
    }

    public override string GetDescription()
    {
        return $"This item buffs your stats by {BaseBuffAmount}.";
    }

    public override void UpgradeAbility(int upgradeValue)
    {
        BaseBuffAmount += upgradeValue;
    }

}

