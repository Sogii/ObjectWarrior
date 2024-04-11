using System.Collections;
using UnityEngine;

internal class EnemyTurn : State
{
    public EnemyTurn(BattleSystem battleSystem) : base(battleSystem)
    {
    }

    public override IEnumerator Start()
    {
        Debug.Log("Enemy Turn");
        //Do random enemy attack (10 dmg)
        BattleSystem._playerData.TakeDamage(BattleSystem._enemyData.Damage);
        //Show the damage in the combat text panel
        BattleSystem.combatUIManager.PopulatePostCombatPanel("Enemy Turn", "Enemy dealt " + BattleSystem._enemyData.Damage + " damage");

        //Switch back to player turn
        yield return new WaitForSeconds(1f);
        BattleSystem.SetState(new PlayerTurn(BattleSystem));
    }
}

