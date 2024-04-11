using System.Collections;
using UnityEngine;

public class FirstTurn : State
{
    public FirstTurn(BattleSystem battleSystem) : base(battleSystem)
    {
    }
    public override IEnumerator Start()
    {
        Debug.Log("FirstTurnState");

        //Innitiate player
        GameObject playerObject = BattleSystem.Instantiate(BattleSystem._playerPrefab, BattleSystem._playerBattleStation);
        BattleSystem._playerData = playerObject.GetComponent<PlayerCombatData>();

        BattleSystem.combatUIManager.UpdatePlayerHealthBar(BattleSystem._playerData.CurrentHealth, BattleSystem._playerData._maxHealth);

        //Innitiate enemy
        BattleSystem._enemyData = RandomGenerationUtility.CreateRandomEnemy(BattleSystem.level, BattleSystem.enemySprites[Random.Range(0, BattleSystem.enemySprites.Length)]);
        BattleSystem.enemyGameObject.GetComponent<SpriteRenderer>().sprite = BattleSystem._enemyData.EnemySprite;

        BattleSystem.combatUIManager.UpdateEnemyHealthBar(BattleSystem._enemyData.CurrentHealth, BattleSystem._enemyData.MaxHealth);

        //Add UI update logic
        BattleSystem.combatUIManager.UpdateHealthBarNames();

        yield return new WaitForSeconds(.5f);
        BattleSystem.SetState(new PlayerTurn(BattleSystem));
    }
}