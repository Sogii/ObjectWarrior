using System.Collections;
using UnityEngine;

internal class PlayerDefeat : State
{
    public PlayerDefeat(BattleSystem battleSystem) : base(battleSystem)
    {
    }

    public override IEnumerator Start()
    {
        Debug.Log("Player Defeat");
        BattleSystem.combatUIManager.PopulatePostCombatPanel("Defeat", "You have been defeated!");
        yield return new WaitForSeconds(1f);
        // BattleSystem.SetState(new End(BattleSystem));
    }
}