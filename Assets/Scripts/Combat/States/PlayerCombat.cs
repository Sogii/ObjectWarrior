using System.Collections;
using UnityEngine;

public class PlayerCombat : State
{
    private Item item;
    public PlayerCombat(BattleSystem battleSystem, Item item) : base(battleSystem)
    {
        this.item = item;
    }

    public override IEnumerator Start()
    {
        Debug.Log("Player Combat");
        BattleSystem.combatUIManager.ChangeCombatMenuPanel(3);
        Ability ability = item.Abilities[0];
        ability.Execute(BattleSystem._playerData, BattleSystem._enemyData);
        
        string AbilityDescription = item.Abilities[0].AbilityDescription;
        BattleSystem.combatUIManager.PopulatePostCombatPanel(item.ItemName, AbilityDescription);

        yield return new WaitForSeconds(1f);

        BattleSystem.SetState(new EnemyTurn(BattleSystem));
    }
}
