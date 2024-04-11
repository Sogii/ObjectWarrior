using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

internal class PlayerVictory : State
{
    public PlayerVictory(BattleSystem battleSystem) : base(battleSystem)
    {
    }

    public override IEnumerator Start()
    {
        Debug.Log("Player Victory");
        BattleSystem.combatUIManager.PopulatePostCombatPanel("Victory", "You have defeated the enemy!");
        BattleSystem.level++;
        BattleSystem.enemyGameObject.GetComponent<SpriteRenderer>().enabled = false;


        if (BattleSystem.level >= 10)
        {
            BattleSystem.combatUIManager.PopulatePostCombatPanel("Victory", "You have defeated all enemies!");
            yield return new WaitForSeconds(2f);
            BattleSystem.combatUIManager.ChangeCombatMenuPanel(4);
            SceneManager.LoadScene("Main Scene 2");
            yield break;
        
        }

        BattleSystem.combatUIManager.ChangeCombatMenuPanel(4);


        yield return new WaitForSeconds(2f);
        // BattleSystem.SetState(new Begin(BattleSystem));
    }

}
