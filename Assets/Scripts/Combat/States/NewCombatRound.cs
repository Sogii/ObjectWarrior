using System.Collections;
using UnityEngine;

public class NewCombatRound : State
{
    public NewCombatRound(BattleSystem battleSystem) : base(battleSystem)
    {
    }
    public override IEnumerator Start()
    {
        Debug.Log("New Combat Round at level " + BattleSystem.level);

        //Spawn a new enemy (set sprite active)
        

        BattleSystem._enemyData = RandomGenerationUtility.CreateRandomEnemy(BattleSystem.level, BattleSystem.enemySprites[Random.Range(0, BattleSystem.enemySprites.Length-1)]);
        BattleSystem.enemyGameObject.GetComponent<SpriteRenderer>().enabled = true;
        BattleSystem.enemyGameObject.GetComponent<SpriteRenderer>().sprite = BattleSystem._enemyData.EnemySprite;

        BattleSystem._playerData.Heal(25);

        BattleSystem.combatUIManager.UpdateEnemyHealthBar(BattleSystem._enemyData.CurrentHealth, BattleSystem._enemyData.MaxHealth);

        BattleSystem.combatUIManager.UpdateHealthBarNames();


        
        BattleSystem.SetState(new PlayerTurn(BattleSystem));
        yield return new WaitForSeconds(.5f);
    }
}

